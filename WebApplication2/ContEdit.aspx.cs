using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Constants_Common;
using Constants_Content;
using FuncProc;
using UserData;

namespace WebApplication2
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst = new Const_Common();          // Объект общих констант и переменных
        //Const_Docs DConst = new Const_Docs();            // Объект констант документа
        Const_Content ContConst = new Const_Content();     // Объект констант контента
        MySqlConnection qryCnn = new MySqlConnection();    // Подключение к базе данных
        MySqlCommand qrySQL = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc CFP = new CommonFuncProc();         // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData = new TUserData();              // Объект данных пользователя

        int ID_Doc = 0;                                    // Уникальный идентификатор документа в журнале
        int ID_Cont = 0;                                   // Уникальный идентификатор контента
        string pageCurrent = "";                           // Эта страница
        int VerContent = 0;                                // Версия изменяемого контента
        int ID_Data = 0;                                   // ID первой версии контента, одинаков для всех версий одного контента. Чтобы просматривать неактуальные версии
        string DateCreate = "";                            // Дата создания контента с версией 1
        int ID_ContOld = 0;                                // ID изменяемого контента

        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            pageCurrent = CConst.pageContEdit;
            lblApp.Text = CConst.App_Ver;
            lblCreater.Text = CConst.CopyRight;

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            GetParams();

            // Проверка на корректность параметров и аутентификацию
            if (!CheckParams())
            {
                Response.Redirect(CConst.pageMain);
            }

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            // Получение и обработка параметров
            ID_ContOld = ID_Cont;                               // Сохранение ID изменяемого контента для получения значений и апдейта полей

            // Проверка на возврат
            if (!IsPostBack)                                    // Если первая загрузка страницы
            {
                //Чтение и загрузка справочников

                // Заполнение ddlContType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprContType, ddlContType, 0, true, "0", "");
                // Заполнение ddlGrif значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprGrif, ddlGrif, 0, true, "0", "");
                // Заполнение ddlStorageBuild значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStorageBuild, ddlStorageBuild, 0, true, "0", "");
                // Заполнение ddlContCarrier значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprContCarrier, ddlContCarrier, 0, true, "0", "");

                DB_Read_Data(ID_Cont);                           // Чтение и загрузка данных документа

                // Присвоение значений переменным, используемым в UserControl AdminContent.ascx
                AdminContent.strWhereExpression = Get_WhereExpression();
                AdminContent.strOrderExpression = "strNameCont";
                // В двоичном представлении показывает последовательность отображаемых столбцов
                AdminContent.ColumnVisible = 251;
            }

        }

        protected void DB_Read_Data(int ID_Cont)
        {
            string errDB = "";

            MySqlConnection qryCnn_Loc = new MySqlConnection();           // Отдельное подключение к базе данных, т.к. основное используется для перестройки справочников
            MySqlCommand qrySQL_Loc = new MySqlCommand();
            qryCnn_Loc.ConnectionString = CConst.strConn;
            qryCnn_Loc.Open();
            qrySQL_Loc.Connection = qryCnn_Loc;

            qrySQL_Loc.CommandText = string.Format(ContConst.sqlContReadEdit, ID_Cont);
            MySqlDataReader RS = qrySQL_Loc.ExecuteReader();

            if (RS.Read())
            {
                edtContName.Text = RS.GetString(RS.GetOrdinal("strName"));
                ddlContType.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Type"));
                ddlGrif.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Grif"));
                ddlStorageBuild.SelectedValue = RS.GetString(RS.GetOrdinal("ID_StorageBuilding"));
                // Перестройка справочника мест хранения в зависимости от выбранного здания
                ddlStorageBuild_SelectedIndexChanged(null, null);
                ddlStoragePlace.SelectedValue = RS.GetString(RS.GetOrdinal("ID_StoragePlace"));
                ddlContCarrier.SelectedValue = RS.GetString(RS.GetOrdinal("ID_ContentCarrier"));
                edtDescription.Text = RS.GetString(RS.GetOrdinal("strDescr"));
            }
            RS.Close();
        }
        
        protected void ReadAppWebConfigiration()
        {
            // Строка подключения
            CConst.strConn = Convert.ToString(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ArchiveConnString"]);
            // Имя и версия приложения
            CConst.App_Ver = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["App_Name"] + " " + System.Web.Configuration.WebConfigurationManager.AppSettings["App_Ver"]);
            CConst.CopyRight = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CopyRight"]);
        }

        protected void GetParams()
        {
            // Входные параметры на страницу
            ID_Doc = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                            // Получение ID_Doc из Session 
            ID_Cont = CFP.GetDigitParam(CConst.sp_ID_Cont, 0);                          // Получение ID_Cont из Session 
            
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 

        }

        // ---- Применение прав доступа к элементам формы ----------------------------------------------------------------------------------------
        protected void Get_UserData_And_Applying_Access_Rights()
        {
            string errDB = "";
            bool qwe = true;
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав

            // Применение прав к главному меню
            AdminMenu.ApplyRight("Docs", true);
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Request", UserData.Rights.bool_Request_Read);
            AdminMenu.ApplyRight("Users", UserData.Rights.bool_User_Read);
            AdminMenu.ApplyRight("Spr", UserData.Rights.bool_Spr_Read);
            AdminMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest) & (ID_Doc > 0));
        }

        // Реакция на изменение значения здания хранения (ddlStorageBuild)
        protected void ddlStorageBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind_StoragePlace();
        }

        // ---- Запрос данных об объектах подразделения ------------------------------------------
        public void DataBind_StoragePlace()
        {
            CFP.ReadSpravochnik(qrySQL, string.Format(ContConst.sqlSprStoragePlace, ddlStorageBuild.SelectedValue), ddlStoragePlace, 0, true, "0", " ");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageDocEdit);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Check_Input_Data())
            {
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("редактирование контента, ID документа = {0}, ID контента = {1}", ID_Doc, ID_Cont));
                DB_Save_Data();
            }
        }

        // Проверка корректности входных данных
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();              // Очистка списка ошибок
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            edtContName.Text = edtContName.Text.Trim();
            if (edtContName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать указать название контента");
            if (ddlContType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип контента");
            if (ddlGrif.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать гриф");
            if (ddlStorageBuild.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать здание, в котором находится документ");
            if (ddlStoragePlace.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать кабинет, в котором находится документ");
            if (ddlContCarrier.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип физического носителя");
            //if (fuFileUpload.FileName == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать файл для загрузки");

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }

        // ---- Запись данных контента в БД -------------------------------------------------
        protected bool DB_Save_Data()
        {
            // Вспомогательные переменные
            string strSQL_Update    = "";
            string strLog           = "";
            string errDB            = "";
            string strRND           = CFP.GetRNDString(30);
            string strNameSystem    = "";
            string strNameReal      = "";
            string strFullFileName  = "";

            // Очистка и загрузка структуры с описанием полей Update-запроса
            CFP.UpdatingField_Clear();
            CFP.UpdatingField_Add("strName",            1, edtContName.Text);
            CFP.UpdatingField_Add("ID_Type",            0, ddlContType.SelectedValue);
            CFP.UpdatingField_Add("ID_Grif",            0, ddlGrif.SelectedValue);
            CFP.UpdatingField_Add("ID_StorageBuilding", 0, ddlStorageBuild.SelectedValue);
            CFP.UpdatingField_Add("ID_StoragePlace",    0, ddlStoragePlace.SelectedValue);
            CFP.UpdatingField_Add("ID_ContentCarrier",  0, ddlContCarrier.SelectedValue);
            CFP.UpdatingField_Add("strDescr",           1, edtDescription.Text);

            // Проверка корректности данных о прикрепляемом файле и их запись в базу
            try
            {
                // Если файл в контенте не меняется, то делать апдейт полей в этом же контенте
                if (fuFileUpload.FileName == "")
                {
                    // Создаем Update-запрос только по измененным полям
                    //Get_SQL_Update(MySqlCommand qrySQL, string TableName, string KeyField_Name, string KeyField_Value, bool NeedModifyInfo, string UserName, ref string strSQL, ref string strLog)
                    if (CFP.Get_SQL_Update(qrySQL, "a_content", "ID", ID_Cont.ToString(), true, "admin", ref strSQL_Update, ref strLog))
                    {
                        // Если есть изменения, то Update-им запись в БД и пишем лог изменений
                        // Внести изменения в таблицу a_content
                        if (!CFP.SQL_ExecuteNonQuery(qrySQL, strSQL_Update, ref errDB))
                            throw new Exception("Запись изменений контента");
                        Response.Redirect(CConst.pageAdminDocView);
                    }
                }
                // Если прикрепляется новый файл, то создавать новую запись в a_content с версией +1, ссылкой на родительский документ, boolIsActual = true, у предыдущих версий установить boolIsActual = false
                else
                {
                    // Формирование имени файла - ГодМесяцДень_ЧасМинутаСекунда.РасширениеФайла
                    strNameSystem = DateTime.Now.ToString("yyMMdd_HHmmss") + fuFileUpload.FileName.Substring(fuFileUpload.FileName.LastIndexOf("."));   // Генерация уникального имени файла для хранения на сервере
                    strNameReal = fuFileUpload.FileName;                                                                                                // Сохранение реального имени загружаемого файла
                    strFullFileName = CConst.ContentPath + strNameSystem;

                    // Получить версию изменяемого контента
                    CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(ContConst.sqlGetVerContent, ID_ContOld), 1, ref VerContent, ref errDB);

                    // Получить дату создания первой версии контента
                    CFP.SQL_GetOneValue_String(qrySQL, string.Format(ContConst.sqlGetDateCreate, ID_ContOld), "", ref DateCreate, ref errDB);

                    // Получить ID_Data первой версии контента. Единое поле для всех версий одного контента, чтобы можно было просматривать неактуальные версии контента
                    CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(ContConst.sqlGetID_Data, ID_ContOld), 1, ref ID_Data, ref errDB);

                    // Запись информации о созданном контенте в БД
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlAttachFile, ID_Doc, edtContName.Text, ddlContType.SelectedValue, ddlGrif.SelectedValue, ddlStorageBuild.SelectedValue,
                                                                       ddlStoragePlace.SelectedValue, ddlContCarrier.SelectedValue, strNameReal, strNameSystem, DateTime.Parse(DateCreate).ToString("yyy-MM-dd HH:mm:ss"), "Admin", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), "Admin", VerContent + 1, strRND), ref errDB))
                        throw new Exception("Запись информации о прикрепляемом файле в базу данных");
                                                                                                   
                    // Установить поле BoolIsActual изменяемого контента в false
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlUpdateBoolIsActual, ID_ContOld), ref errDB))
                        throw new Exception("Изменение видимости предыдущей версии контента");

                    // Получить ID созданной записи
                    if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(ContConst.sqlGetCont_ID, strRND), 0, ref ID_Cont, ref errDB))
                        throw new Exception("Получение идентификатора нового контента");

                    // Записать ID_Data изменяемого документа в поле ID_DATA нового контента, чтобы можно было просматривать неактуальные версии одного контента
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlUpdateID_Data, ID_Data, ID_Cont), ref errDB))
                        throw new Exception("Запись ID родительского контента");

                    /*if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlUpdateID_Data, ID_ContOld, ID_Cont), ref errDB))
                        throw new Exception("Запись ID родительского контента");
                    */
                    
                    // Глобально для проекта сохранить номер созданного контента
                    Session[CConst.sp_ID_Cont] = ID_Cont;

                    // Очистить Примечание от мусора
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlClearDescr, edtDescription.Text, ID_Cont), ref errDB))
                        throw new Exception("Запись данных нового контента (чистка Описания от мусора)");


                    /*-------------------- Загрузка файла --------------------------------*/
                    // Upload файла
                    fuFileUpload.SaveAs(strFullFileName);

                    // Прроверка на существование файла (т.е. все за аплойдилось)                    
                    if (!System.IO.File.Exists(strFullFileName))
                        throw new Exception("Ошибка загрузки файла на сервер (проверка Upload)");

                    /*-------------------- Конец загрузки файла --------------------------------*/

                    CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("редактирование контента, создание новой версии, ID документа = {0}, ID новой версии контента = {1}", ID_Doc, ID_Cont));

                    // Создать запись в таблице a_doc_content, содержащую информацию о связи между контентом и документом
                    //if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlInsertDocContent, ID_Doc, ID_Cont), ref errDB))
                    //    throw new Exception("Запись данных о связи контента и документа");

                    // Скрыть панель ошибок
                    pnlError.Visible = false;

                    Response.Redirect(CConst.pageAdminDocView);
                }
            }
            catch (Exception ex)
            {
                // Если произошла ошибка во время Аплоада файла, то выводим ее
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка прикрепления файла к документу");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ File_Attach() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;

            return true;
        }

        // Формирование строк с ограничениями для SQL запроса на получение данных о контенте
        protected string Get_WhereExpression()
        {
            // Ограничение на вывод контентов с определенным ID
            string strFilter = string.Format("C.ID =  {0} ", ID_Cont);
            return strFilter;
        }
        
    }
}