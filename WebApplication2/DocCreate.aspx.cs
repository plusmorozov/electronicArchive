using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Constants_Common;
using Constants_Docs;
using FuncProc;
using UserData;

namespace WebApplication2
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst     = new Const_Common();          // Объект общих констант и переменных
        Const_Docs DConst       = new Const_Docs();            // Объект констант документа
        MySqlConnection qryCnn  = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL     = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc CFP      = new CommonFuncProc();        // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData      = new TUserData();             // Объект данных пользователя

        string pageCurrent = "";                               // Эта страница

        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            pageCurrent     = CConst.pageDocCreate;

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

            if (!IsPostBack)                                        // Если первая загрузка страницы
            {
                // Заполнение ddlDocType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprDocType,                ddlDocType,        0, true, "0", "");
                // Заполнение ddlSection значениями из базы
                //CFP.ReadSpravochnik(qrySQL, CConst.sqlSprSection,                ddlSection,        0, true, "0", "");
                // Заполнение ddlSection значениями из базы согласно правам пользователя
                CFP.ReadSpravochnik(qrySQL, string.Format(CConst.sqlSectionOfRights, UserData.UID, UserData.Rights.str_Doc_Write), ddlSection, 0, true, "0", "");
                // Заполнение ddlObjectType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprObjectType,             ddlObjectType,     0, true, "0", "");
                // Заполнение ddlCustomer значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprCustomer,               ddlCustomer,       0, true, "0", "");
                // Заполнение ddlExecuter значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprCustomer,               ddlExecuter,       0, true, "0", "");
                // Заполнение ddlStorageBuild значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStorageBuild,           ddlStorageBuild,   0, true, "0", "");
                // Заполнение ddlStatus значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStatus,                 ddlStatus,         0, true, "0", "");
                // Заполнение chbklstDepartment значениями из баз
                //CFP.ReadCheckBoxList(qrySQL, CConst.sqlSprDepartment, chbklstDepartment); 
                //CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlSprDepartmentCheked, 0), chbklstDepartment); 
                CFP.ReadCheckBoxList(qrySQL, CConst.sqlSprDepartment, chbklstDepartment);
                FutterSettings();
            }

            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            if (qryCnn != null) qryCnn.Close();

        }

        protected void GetParams()
        {
            // Входные параметры на страницу
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return (UserData.UID > CConst.UID_Guest);
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
            AdminMenu.ApplyRight("Request", UserData.Rights.bool_Request_Read);
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Users", UserData.Rights.bool_User_Read);
            AdminMenu.ApplyRight("Spr", UserData.Rights.bool_Spr_Read);
            AdminMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);
        }

        protected void ReadAppWebConfigiration()
        {
            // Строка подключения
            CConst.strConn = Convert.ToString(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ArchiveConnString"]);
            // Имя и версия приложения
            CConst.App_Ver = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["App_Name"] + " " + System.Web.Configuration.WebConfigurationManager.AppSettings["App_Ver"]);
            CConst.CopyRight = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CopyRight"]);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageAdmin);
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
             if (Check_Input_Data())
                if (DB_Save_Data())
                {
                    Response.Redirect(CConst.pageContCreate);
                }
        }


        // ---- Проверка корректность ввода данных ------------------------------------------------------------------------------
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();              // Очистка списка ошибок
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            edtDocName.Text = edtDocName.Text.Trim();
            if (edtDocName.Text == "")                CFP.Error_Add_Msg(lstError.Items, "Необходимо указать название документа");
            edtShifr.Text = edtShifr.Text.Trim();
            if (edtShifr.Text == "")                  CFP.Error_Add_Msg(lstError.Items, "Необходимо указать шифр документа");
            if (ddlYear.SelectedValue == "")          CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать год");
            if (ddlDocType.SelectedValue == "0")      CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип документа");
            edtArchivedHuman.Text = edtArchivedHuman.Text.Trim();
            if (edtArchivedHuman.Text == "")          CFP.Error_Add_Msg(lstError.Items, "Необходимо указать человека, сдавшего документ в архив");
            if (ddlSection.SelectedValue == "0")      CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать подразеление, в архиве которого хранится документ");
            if (ddlObjectType.SelectedValue == "0")   CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип объекта, к которому относится документ");
            if (ddlObject.SelectedValue == "0")       CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать объект, к которому относится документ");
            if (ddlCustomer.SelectedValue == "0")     CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать заказчика");
            if (ddlExecuter.SelectedValue == "0")     CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать исполнителя");
            edtNameWork.Text = edtNameWork.Text.Trim();
            if (edtNameWork.Text == "")               CFP.Error_Add_Msg(lstError.Items, "Необходимо указать наименование работ");
            if (ddlStorageBuild.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать здание, в котором находится документ");
            if (ddlStoragePlace.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать кабинет, в котором находится документ");
            if (ddlStatus.SelectedValue == "0")       CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать статус оцифровки документа");
            /*
            if (!IsCheked(chbklstDepartment))
            {
                                                      CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать отделы, к которым относится документ");
            }
             * */
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
            int ID_Doc = 0;
            string errDB = "";
            string   strRND  = CFP.GetRNDString(30);

            try
            {
                // Создать запись в таблице a_doc
                if (!CFP.SQL_ExecuteNonQuery(qrySQL,
                                              string.Format(DConst.sqlInsertDoc, edtDocName.Text, edtShifr.Text, ddlYear.SelectedValue, ddlDocType.SelectedValue, edtArchivedHuman.Text, ddlSection.SelectedValue,
                                                            ddlObjectType.SelectedValue, ddlObject.SelectedValue, ddlCustomer.SelectedValue, ddlExecuter.SelectedValue, edtNameWork.Text,
                                                            ddlStorageBuild.SelectedValue, ddlStoragePlace.SelectedValue, ddlStatus.SelectedValue, strRND), ref errDB))
                    throw new Exception("Вставка записи нового документа");

                // Получить ID созданной записи
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(DConst.sqlGetDoc_ID, strRND), 0, ref ID_Doc, ref errDB))
                    throw new Exception("Получение идентификатора нового документа");

                // Глобально для проекта сохранить номер созданного документа
                Session[CConst.sp_ID_Doc] = ID_Doc;

                // Записать в strDescr документа реальное значение вместо strRND
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlUpdateDescr, edtDescription.Text, ID_Doc), ref errDB))
                    throw new Exception("Запись данных нового документа (чистка Примечания от мусора)");

                // Очистить Примечание от мусора
                //if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlClearDescr, ID_Doc), ref errDB))
                //    throw new Exception("Запись данных нового документа (чистка Примечания от мусора)");

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

                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("создание документа, ID документа = {0}", ID_Doc));
                // Скрыть панель ошибок
                pnlError.Visible = false;
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

        protected bool IsCheked(CheckBoxList chkblst)
        {
         for (int i = 0; i < chkblst.Items.Count; i++)
            {
                if (chkblst.Items[i].Selected) return true;
            }
            return false;
        }



        protected void chbklstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            edtDocName.Text = "Документ " + CFP.GetRNDString(10); ;
            edtShifr.Text = CFP.GetRNDString(5);
            ddlYear.SelectedValue = "2014";
            ddlDocType.SelectedValue = "1";
            edtArchivedHuman.Text = "Вася";
            ddlSection.SelectedValue = "1";
            ddlObjectType.SelectedValue = "2";
            ddlObject.SelectedValue = "1";
            ddlCustomer.SelectedValue = "1";
            ddlExecuter.SelectedValue = "1";
            edtNameWork.Text = "Наименование работ";
            ddlStorageBuild.SelectedValue = "1";
            ddlStoragePlace.SelectedValue = "1";
            ddlStatus.SelectedValue = "1";
            edtDescription.Text = "Описание";
        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }
    }
}