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
    public partial class SprCompany : System.Web.UI.Page
    {
        string sqlCompanyType = "SELECT ID, strName FROM a_Company_Type";

        const string sqlInsert = "INSERT INTO a_Company( ID_Company_Type, strName, strNameShort, strNameFull, strAddress ) VALUES( {0}, '{1}', '{2}', '{3}', '{4}' )";
        const string sqlGetID  = "SELECT ID FROM a_Company WHERE strName='{0}'";
        const string sqlUpdate = "UPDATE a_Company SET strName='{1}' WHERE ID={0}";

        // Создание объектов
        Const_Common    CConst   = new Const_Common();          // Объект общих констант и переменных
        Const_Docs      DConst   = new Const_Docs();            // Объект общих констант и переменных
        MySqlConnection qryCnn   = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand    qrySQL   = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc  CFP      = new CommonFuncProc();        // Объект с общими вспомогательными функциями и процедурами
        TUserData       UserData = new TUserData();             // Объект данных пользователя

        // Загрузка страницы
        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            // Получение и обработка параметров
            GetParams();

            // Проверка на корректность параметров и аутентификацию
            if (!CheckParams())
            {
                Response.Redirect(CConst.pageMain);
            }

            // Получение данных о пользователе и его прав, Применение прав
            Get_UserData_And_Applying_Access_Rights();

            // Проверка на возврат
            if (!IsPostBack)                                        // Если первая загрузка страницы
            {
                CFP.ReadSpravochnik(qrySQL, string.Format(sqlCompanyType), ddlCompanyType, 0, true, "0", " ");
                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            if (qryCnn != null) qryCnn.Close();

        }
        
        // Проверка на корректность введенных данных - Организации
        protected bool dbgTable_Check_Input_Data(int ID, DropDownList ddlCompanyType, TextBox edtName, TextBox edtNameShort, TextBox edtNameFull, TextBox edtAddress)
        {
            edtName.Text = edtName.Text.Trim();
            edtNameShort.Text = edtNameShort.Text.Trim();
            edtNameFull.Text = edtNameFull.Text.Trim();
            edtAddress.Text = edtAddress.Text.Trim();
            // Очистка списка ошибок
            lstError.Items.Clear();
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            if (ID <= 0) CFP.Error_Add_Msg(lstError.Items, "Ошибка идентификации данных. Обратитесь к администратору.");
            if (ddlCompanyType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать \"Тип организации\"");
            if (edtName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Наименование\" организации");
            if (edtNameShort.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Сокращенное наименование\" организации");
            if (edtNameFull.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Полное наименование\" организации");
            if (edtAddress.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Адрес\" организации");

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest));
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

        // ---- Чтение и обработка входных параметров
        protected void GetParams()
        {
            // Входные параметры на страницу
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        // Реакция нажатия на кнопку - Добавить
        protected void btnTable_Create_Click(object sender, EventArgs e)
        {
            dbgTable.EditIndex = -1;
            btnTable_Create.Visible = false;
            pnlTableInsert.Visible = true;

            edtName.Text = "";
            edtNameShort.Text = "";
            edtNameFull.Text = "";
            edtAddress.Text = "";
        }

        // Обработка события вставки новых данных
        protected void btnTable_Create_Submit_Click(object sender, EventArgs e)
        {
            if (dbgTable_Check_Input_Data(9999, ddlCompanyType, edtName, edtNameShort, edtNameFull, edtAddress)
                && dbgTable_Insert_Data())
            {
                dbgTable.DataBind();
                dbgTable_RowCancelingEdit(null, null);

            }
        }

        // Запись новых  данных - Организации
        protected bool dbgTable_Insert_Data()
        {

            // Вспомогательные переменные
            string errDB = "";
            string strRND = CFP.GetRNDString(30);
            int ID = 0;

            try
            {
                // Создать запись в таблице tblControl
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsert, ddlCompanyType.SelectedValue, strRND, edtNameShort.Text, edtNameFull.Text, edtAddress.Text), ref errDB))
                    throw new Exception("Вставка записи организации");
                // Получить ID созданной записи
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(sqlGetID, strRND), 0, ref ID, ref errDB))
                    throw new Exception("Получение идентификатора новой записи");
                // Изменить служебные данные на пользовательские
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlUpdate, ID, edtName.Text), ref errDB))
                    throw new Exception("Вставка записи организации (чистка служебных данных)");
                // Запись в лог 
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("спр Организации - добавление ID={0}", ID));
                // Скрыть панель ошибок
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ DB_Save_Data_SM_Osn() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }

        protected void btnTable_Create_Cancel_Click(object sender, EventArgs e)
        {
            dbgTable_RowCancelingEdit(null, null);
        }

        protected void dbgTable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                btnTable_Create.Visible = false;
                pnlTableInsert.Visible = false;
            }
        }

        protected void dbgTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lstError.Items.Clear();
            pnlError.Visible = false;
            btnTable_Create.Visible = true;
            pnlTableInsert.Visible = false;
            dbgTable.EditIndex = -1;
        }

        // Обработка события сохранения измененных данных - Организации 
        protected void dbgTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(e.Keys[0]);
            GridViewRow Row = dbgTable.Rows[e.RowIndex];

            if (dbgTable_Check_Input_Data(ID, ddlCompanyType, ((TextBox)Row.FindControl("edtName")), ((TextBox)Row.FindControl("edtNameShort")),
                                                              ((TextBox)Row.FindControl("edtNameFull")), ((TextBox)Row.FindControl("edtAddress")))
                && dbgTable_Update_Data(ID, Row))
            {
                dbgTable_RowCancelingEdit(null, null);
            }
            else e.Cancel = true;
            
        }

        // Запись измененных данных - Организации
        protected bool dbgTable_Update_Data(int ID, GridViewRow Row)
        {
            string strSQL_Update = "";
            string strLog = "";
            string errDB = "";

            CFP.UpdatingField_Clear();
            CFP.UpdatingField_Add("strName", 1, ((TextBox)Row.FindControl("edtName")).Text);
            CFP.UpdatingField_Add("strNameShort", 1, ((TextBox)Row.FindControl("edtNameShort")).Text);
            CFP.UpdatingField_Add("strNameFull", 1, ((TextBox)Row.FindControl("edtNameFull")).Text);
            CFP.UpdatingField_Add("strAddress", 1, ((TextBox)Row.FindControl("edtAddress")).Text);

            // Создаем Update-запрос только по измененным полям
            if (CFP.Get_SQL_Update(qrySQL, "a_Company", "ID", Convert.ToString(ID), false, UserData.UserName, ref strSQL_Update, ref strLog))
            {
                // Если есть изменения, то Update-им запись в БД и пишем лог изменений
                try
                {
                    // Внести изменения в таблицу tblSMJournal
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, strSQL_Update, ref errDB))
                        throw new Exception("Запись изменений данных огранизации");
                    // Запись в лог 
                    CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("спр Организации - изменение ID={0}, {1}", ID, strLog));
                    // Скрыть панель ошибок
                    pnlError.Visible = false;
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка во время Апдейта, то выводим ее
                    lstError.Items.Clear();
                    CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных. Обратитесь к администратору.");
                    CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ dbgCompany_Update_Input_Data ]");
                    CFP.Error_Add_Msg(lstError.Items, errDB);
                    pnlError.Visible = lstError.Items.Count != 0;
                    return false;
                }
            }
            return true;
        }

        // ---- Чтение параметров приложения из Web.config ---------------------------------------------------------------------
        protected void ReadAppWebConfigiration()
        {
            // Строка подключения
            CConst.strConn = Convert.ToString(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ArchiveConnString"]);
            // Имя и версия приложения
            CConst.App_Ver = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["App_Name"] + " " + System.Web.Configuration.WebConfigurationManager.AppSettings["App_Ver"]);
            CConst.CopyRight = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CopyRight"]);
        }
    }
}