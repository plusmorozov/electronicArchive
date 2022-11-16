using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Constants_Common;
using Constants_Content;
using Constants_Docs;
using FuncProc;
using UserData;

namespace WebApplication2
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst     = new Const_Common();          // Объект общих констант и переменных
        Const_Docs DConst       = new Const_Docs();            // Объект констант документа
        Const_Content ContConst = new Const_Content();         // Объект констант контента
        MySqlConnection qryCnn  = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL     = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc CFP      = new CommonFuncProc();        // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData      = new TUserData();             // Объект данных пользователя

        int ID_Doc          = 0;                               // Уникальный идентификатор документа в журнале
        int ID_Cont         = 0;                               // Уникальный идентификатор контента
        string pageBackURL  = "";                              // Страница с которой был совершен переход на эту
        string pageCurrent  = "";                              // Эта страница
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            pageCurrent = CConst.pageDocEdit;

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            GetParams();                                        // Получение и обработка параметров

            // Проверка на корректность параметров и аутентификацию
            if (!CheckParams())
            {
                Response.Redirect(CConst.pageMain);
            }

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            // Проверка на возврат
            if (!IsPostBack)                                    // Если первая загрузка страницы
            {
                //Чтение и загрузка справочников

                // Заполнение ddlDocType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprDocType, ddlDocType, 0, true, "0", "");
                // Заполнение ddlSection значениями из базы
                //CFP.ReadSpravochnik(qrySQL, CConst.sqlSprSection, ddlSection, 0, true, "0", "");
                CFP.ReadSpravochnik(qrySQL, string.Format(CConst.sqlSectionOfRights, UserData.UID, UserData.Rights.str_Doc_Write), ddlSection, 0, true, "0", "");
                // Заполнение ddlObjectType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprObjectType, ddlObjectType, 0, true, "0", "");
                // Заполнение ddlCustomer значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprCustomer, ddlCustomer, 0, true, "0", "");
                // Заполнение ddlExecuter значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprCustomer, ddlExecuter, 0, true, "0", "");
                // Заполнение ddlStorageBuild значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStorageBuild, ddlStorageBuild, 0, true, "0", "");
                // Заполнение ddlStatus значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStatus, ddlStatus, 0, true, "0", "");
                
                DB_Read_Data(ID_Doc);                           // Чтение и загрузка данных документа

                //Присвоение значений переменным, используемым в UserControl Content.ascx
                AdminContent.strWhereExpression = Get_WhereExpression();
                AdminContent.strOrderExpression = "strNameCont";
                // В двоичном представлении показывает последовательность отображаемых столбцов
                //AdminContent.ColumnVisible = 503;


                // Если есть право на создание контента, то показывать столбец с кнопками для редактирования и скачивания контента
                if (UserData.Rights.bool_Cont_Create)
                {
                    // В двоичном представлении показывает последовательность отображаемых столбцов
                    AdminContent.ColumnVisible = 1015;
                }
                else
                {
                    // Если права на создание контента нет, то не показывать столбец с кнопками для редактирования контента
                    AdminContent.ColumnVisible = 759;
                }

                // Передача прав пользователя в Content.ascx
                AdminContent.UR = UserData.Rights;

                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            if (qryCnn != null) qryCnn.Close();

        }

        protected void DB_Read_Data(int ID_Doc)
        {
            string errDB = "";

            MySqlConnection qryCnn_Loc = new MySqlConnection();           // Отдельное подключение к базе данных, т.к. основное используется для перестройки справочников
            MySqlCommand qrySQL_Loc = new MySqlCommand();
            qryCnn_Loc.ConnectionString = CConst.strConn;
            qryCnn_Loc.Open();
            qrySQL_Loc.Connection = qryCnn_Loc;
            
            qrySQL_Loc.CommandText = string.Format(DConst.sqlDocReadEdit, ID_Doc);
            MySqlDataReader RS = qrySQL_Loc.ExecuteReader();

            if (RS.Read())
            {
                edtDocName.Text = RS.GetString(RS.GetOrdinal("strName"));
                edtShifr.Text = RS.GetString(RS.GetOrdinal("strShifr"));
                ddlYear.SelectedValue = RS.GetString(RS.GetOrdinal("intYear"));
                ddlDocType.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Type"));
                edtArchivedHuman.Text = RS.GetString(RS.GetOrdinal("strArchived_Human"));
                ddlSection.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Section"));
                ddlObjectType.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Object_Type"));
                // Перестройка справочника объектов в зависимости от выбранного типа объектов
                ddlObjectType_SelectedIndexChanged(null, null);
                ddlObject.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Object"));
                ddlCustomer.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Customer"));
                ddlExecuter.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Executor"));
                edtNameWork.Text = RS.GetString(RS.GetOrdinal("strName_Work"));
                ddlStorageBuild.SelectedValue = RS.GetString(RS.GetOrdinal("ID_StorageBuild"));
                // Перестройка справочника мест хранения в зависимости от выбранного здания
                ddlStorageBuild_SelectedIndexChanged(null, null);
                ddlStoragePlace.Text = RS.GetString(RS.GetOrdinal("ID_StoragePlace"));
                ddlStatus.SelectedValue = RS.GetString(RS.GetOrdinal("ID_Status"));
                
                //chbklstDepartment.Text = RS.GetString(RS.GetOrdinal("strDepartment"));
                edtDescription.Text = RS.GetString(RS.GetOrdinal("strDescr"));
            }
            RS.Close();

            // Заполнение chbklstDepartment значениями из базы
            CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlSprDepartmentCheked, ID_Doc), chbklstDepartment);
            }

        /*protected void AttachedContent(object sender, EventArgs e)
        {
            qryControl.SelectCommand = string.Format(ContConst.sqlContent, ID_Doc);
        }*/


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
            ID_Doc = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                           // Получение ID_Doc из Session 
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest) & (ID_Doc > 0));
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

            btnAddContent.Visible = UserData.Rights.bool_Cont_Create;
        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }

        // Реакция на изменение значения типа объекта (ddlDocType)
        protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind_Object();
        }

        // ---- Запрос данных об объектах подразделения ------------------------------------------
        public void DataBind_Object()
        {
            CFP.ReadSpravochnik(qrySQL, string.Format(DConst.sqlSprObject, ddlObjectType.SelectedValue), ddlObject, 0, true, "0", " ");
        }

        // Реакция на изменение значения типа объекта (ddlDocType)
        protected void ddlStorageBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind_StoragePlace();
        }

        // ---- Запрос данных о кабинетах хранения ------------------------------------------
        public void DataBind_StoragePlace()
        {
            CFP.ReadSpravochnik(qrySQL, string.Format(DConst.sqlSprStoragePlace, ddlStorageBuild.SelectedValue), ddlStoragePlace, 0, true, "0", " ");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageAdmin);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Response.Redirect(CConst.pageMain);
            if (Check_Input_Data())
            {
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("редактирование документа, ID документа = {0}", ID_Doc));
                DB_Save_Data();
            }
        }

        // Проверка корректности входных данных
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();              // Очистка списка ошибок
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            edtDocName.Text = edtDocName.Text.Trim();
            if (edtDocName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать указать название документа");
            edtShifr.Text = edtShifr.Text.Trim();
            if (edtShifr.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать шифр документа");
            if (ddlYear.SelectedValue == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать год");
            if (ddlDocType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип документа");
            edtArchivedHuman.Text = edtArchivedHuman.Text.Trim();
            if (edtArchivedHuman.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать человека, сдавшего документ в архив");
            if (ddlSection.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать подразеление, в архиве которого хранится документ");
            if (ddlObjectType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип объекта, к которому относится документ");
            if (ddlObject.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать объект, к которому относится документ");
            if (ddlCustomer.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать заказчика");
            if (ddlExecuter.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать исполнителя");
            edtNameWork.Text = edtNameWork.Text.Trim();
            if (edtNameWork.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать наименование работ");
            if (ddlStorageBuild.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать здание, в котором находится документ");
            if (ddlStoragePlace.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать кабинет, в котором находится документ");
            if (ddlStatus.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать статус оцифровки документа");
            /*
            if (!IsCheked(chbklstDepartment))
            {
                CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать отделы, к которым относится документ");
            }
            */
            edtDescription.Text = edtDescription.Text.Trim();
            if (edtDescription.Text == "")
                CFP.Error_Add_Msg(lstError.Items, "Необходимо указать описание документа");
            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }

        // ---- Запись данных документа в БД -------------------------------------------------
        protected bool DB_Save_Data()
        {
            // Вспомогательные переменные
            string errDB = "";
            string strRND = CFP.GetRNDString(30);
            string strSQL_Update = "";
            string strLog = "";
            string ErrDB = "";

            // Очистка и загрузка структуры с описанием полей Update-запроса
            CFP.UpdatingField_Clear();

            // {Имя поля, Тип поля: 1 - str, 0 - int, Новое значение, Нужно ли обновление});
            CFP.UpdatingField_Add("strName", 1, edtDocName.Text);
            CFP.UpdatingField_Add("strShifr", 1, edtShifr.Text);
            CFP.UpdatingField_Add("intYear", 0, ddlYear.SelectedValue);
            CFP.UpdatingField_Add("ID_Type", 0, ddlDocType.SelectedValue);
            CFP.UpdatingField_Add("strArchived_Human", 1, edtArchivedHuman.Text);
            CFP.UpdatingField_Add("ID_Section", 0, ddlSection.SelectedValue);
            CFP.UpdatingField_Add("ID_Object_Type", 0, ddlObjectType.SelectedValue);
            CFP.UpdatingField_Add("ID_Object", 0, ddlObject.SelectedValue);
            CFP.UpdatingField_Add("ID_Customer", 0, ddlCustomer.SelectedValue);
            CFP.UpdatingField_Add("ID_Executor", 0, ddlExecuter.SelectedValue);
            CFP.UpdatingField_Add("strName_Work", 1, edtNameWork.Text);
            CFP.UpdatingField_Add("ID_StorageBuild", 0, ddlStorageBuild.SelectedValue);
            CFP.UpdatingField_Add("ID_StoragePlace", 0, ddlStoragePlace.SelectedValue);
            CFP.UpdatingField_Add("ID_Status", 0, ddlStatus.SelectedValue);
            CFP.UpdatingField_Add("strDescr", 1, edtDescription.Text);
            
            try
            {
                // Создаем Update-запрос только по измененным полям
                //Get_SQL_Update(MySqlCommand qrySQL, string TableName, string KeyField_Name, string KeyField_Value, bool NeedModifyInfo, string UserName, ref string strSQL, ref string strLog)
                if (CFP.Get_SQL_Update(qrySQL, "a_doc", "ID", ID_Doc.ToString(), true, "admin", ref strSQL_Update, ref strLog))
                {
                    // Если есть изменения, то Update-им запись в БД и пишем лог изменений
                    // Внести изменения в таблицу a_content
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, strSQL_Update, ref errDB))
                        throw new Exception("Запись изменений документа");
                }

                // Процедура обновления таблицы a_doc_department
                Update_Department();

                // Скрыть панель ошибок
                pnlError.Visible = false;

                Response.Redirect(CConst.pageAdmin);
            }
            catch (Exception ex)
            {
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                //CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ DB_Save_Data_SM_Osn() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }

        protected void Update_Department()
        {
            string SelectedDepartmentOld = "";
            string SelectedDepartmentNew = "";
            string errDB = "";

            // Получить ID отделов к которым относится документ из таблицы a_doc_department
            CFP.SQL_GetOneValue_String(qrySQL, string.Format(DConst.sqlSelectDepartmentDoc, ID_Doc), "", ref SelectedDepartmentOld, ref errDB);
            // Получить ID отделов из чекбокса на странице
            SelectedDepartmentNew = CFP.Get_List_Selected_Value(chbklstDepartment.Items, "");

            // Если изменились, то удалить из таблицы a_doc_department информацию об относящихся к этому документу отделах
            if (SelectedDepartmentOld != SelectedDepartmentNew)
            {
                // Удаляем строки
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlDeleteDepartmentDoc, ID_Doc), ref errDB))
                    throw new Exception("Удаление строк из таблицы a_doc_department");

                // Добавление записей в таблицу a_doc_department о документах и отделах
                for (int i = 0; i < chbklstDepartment.Items.Count; i++)
                {
                    if (chbklstDepartment.Items[i].Selected)
                    {
                        // Создать запись в таблице a_doc_department, содержащую информацию об отделе, имеющем отношение к документу
                        if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlInsertDocDepartment, ID_Doc, chbklstDepartment.Items[i].Value), ref errDB))
                            throw new Exception("Запись данных об отделе, имеющем отношение к документу");
                    }
                }
            }
        }

        protected void BtnContDelClick(object sender, CommandEventArgs e)
        {
            
        }

        protected void dbgAttachedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected bool IsCheked(CheckBoxList chkblst)
        {
            for (int i = 0; i < chkblst.Items.Count; i++)
            {
                if (chkblst.Items[i].Selected) return true;
            }
            return false;
        }

        // Формирование строк с ограничениями для SQL запроса на получение данных о контенте
        protected string Get_WhereExpression()
        {
            // Ограничение на вывод контентов с определенным ID
            string strFilter = string.Format("ID_Doc={0} AND boolIsActual", ID_Doc);
            return strFilter;
        }

        protected void BtnAddContentClick(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageContCreate);
        }
    }
}