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
    public partial class Users : System.Web.UI.Page
    {
        // Запрос на создание нового пользователя
        string sqlInsertUser = "INSERT INTO a_Users ( strLogin, strFam, strName, strOtch, strPodr, strDepartment, strPost, strPhone, strEmail, strDescr, boolEnable, sys_UserModify, sys_DateModify ) VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', NOW() )";
        //string sqlInsertRole = "INSERT INTO a_Params ( Id_User, intCode, intValue, strValue, strDescription ) VALUES( {0}, {1}, {2}, '{3}', '{4}' )";
        string sqlGetNewUser = "SELECT ID FROM a_Users WHERE strDescr='{0}'";
        string sqlUpdateDescr = "UPDATE a_Users SET strDescr=CONCAT('Создана ', NOW(), ' - {1}') WHERE ID={0}";

        // Создание объектов
        Const_Common CConst = new Const_Common();             // Объект общих констант и переменных
        Const_Docs DConst = new Const_Docs();                 // Объект общих констант и переменных
        MySqlConnection qryCnn = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL = new MySqlCommand();             // Организация работы с базой данных
        CommonFuncProc CFP = new CommonFuncProc();            // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData = new TUserData();                 // Объект данных пользователя

        string pageCurrent = "";                              // Эта страница

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
                Response.Redirect(CConst.pageAdmin);
            }

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();


            // Проверка на возврат
            if (!IsPostBack)                                        // Если первая загрузка страницы
            {
                //Чтение инофрмации из базы и ее загрузка в элементы формы
                
                // Заполнение ddlDocType значениями из базы
                //CFP.ReadSpravochnik(qrySQL, CConst.sqlUserLogin, ddlUserLogin, 0, true, "0", "");


                // первоначальная загрузка данных в элементы
                Fill_Data_Spravochnik(qrySQL);

                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации
        }

        // ---- Чтение и установка данных справочников формы -------------------------------------------------------------------------------------------------------
        protected void Fill_Data_Spravochnik(MySqlCommand qrySQL)
        {
            DataBind_ddlUserLogin();
            ddlUserLogin_SelectedIndexChanged(null, null);
        }

        // Чтение данных для выбора Пользователей
        protected void DataBind_ddlUserLogin()
        {
            CFP.ReadSpravochnik(qrySQL, CConst.sqlUsers, ddlUserLogin, 0, true, "0", " ");
            ddlUserLogin.Items.Insert(1, new ListItem { Value = "-999", Text = "Новый пользователь" });
        }

        // Выбор пользователя
        protected void ddlUserLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Res;

            switch (ddlUserLogin.SelectedValue)
            {
                // Ничего не выбрано
                case "0":
                    DB_Read_UserData(Convert.ToInt32(ddlUserLogin.SelectedValue));
                    btnUserCreate.Visible = false;
                    btnUserUpdate.Visible = false;
                    btnUserCancel.Visible = false;
                    break;
                
                // Новый пользователь
                case "-999":
                    //Data_New_User();
                    btnUserCreate.Visible = true;
                    btnUserUpdate.Visible = false;
                    btnUserCancel.Visible = false;
                    break;

                // Редактирование пользователя
                default:
                    Res = DB_Read_UserData(Convert.ToInt32(ddlUserLogin.SelectedValue));
                    btnUserCreate.Visible = false;
                    btnUserUpdate.Visible = Res;
                    btnUserCancel.Visible = Res;
                    break;
            }
        }

        // ---- Проверка корректность ввода данных ------------------------------------------------------------------------------
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();

            edtLogin.Text = edtLogin.Text.Trim();
            if (edtLogin.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Логин\" пользователя");
            edtFam.Text = edtFam.Text.Trim();
            if (edtFam.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Фамилию \" пользователя");
            edtName.Text = edtName.Text.Trim();
            if (edtName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Имя \" пользователя");
            edtOtch.Text = edtOtch.Text.Trim();
            if (edtOtch.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Отчество \" пользователя");
            edtPodr.Text = edtPodr.Text.Trim();
            if (edtPodr.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Подразделение\" пользователя");
            edtDepartment.Text = edtDepartment.Text.Trim();
            if (edtDepartment.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Отдел\" пользователя");
            edtPost.Text = edtPost.Text.Trim();
            if (edtPost.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Должность\" пользователя");
            edtPhone.Text = edtPhone.Text.Trim();
            if (edtPhone.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Телефон\" пользователя");
            edtEmail.Text = edtEmail.Text.Trim();
            if (edtEmail.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Почтовый ящик\" пользователя");
            if (ddlRole.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать \"Роль\" пользователя");
            
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }

        // ---- Запись данных нового пользователя в БД -------------------------------------------------
        protected bool DB_Save_Data_NewUser(ref int ID_User)
        {

            string errDB = "";
            string strRND = CFP.GetRNDString(30);
            ID_User = 0;

            try
            {
                // Создать запись в таблице a_Users
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsertUser, edtLogin.Text.Replace("\\", "\\\\"), edtFam.Text, edtName.Text, edtOtch.Text, edtPodr.Text, edtDepartment.Text, edtPost.Text, edtPhone.Text, edtEmail.Text, strRND, cbUserEnable.Checked.ToString(), UserData.UserFam +" "+ UserData.UserName +" "+ UserData.UserOtch), ref errDB))
                    throw new Exception("Вставка записи нового пользователя");

                // Получить ID созданной записи
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(sqlGetNewUser, strRND), 0, ref ID_User, ref errDB))
                    throw new Exception("Получение идентификатора нового пользователя");
                /*
                // Создать запись в таблице a_params c ролью пользователя
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsertRole, ID_User, 100, ddlRole.SelectedValue, ddlRole.SelectedItem.Text, "Право - Идентификатор роли пользователя"), ref errDB))
                    throw new Exception("Вставка записи роли нового пользователя");
                */
                // Обновить Примечание 
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlUpdateDescr, ID_User, UserData.UserName), ref errDB))
                    throw new Exception("Запись данных нового документа (обновление Примечания )");

                // ----- Запись прав нового пользователя -----------------------------------------------------------------------------------------------
                DB_Save_UserRigths(ID_User);

                // Скрыть панель ошибок
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ DB_Save_Data_NewUser() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }

        // ---- Запись измененных данных пользователя в БД -------------------------------------------------
        protected bool DB_Save_Data_UpdateUser()
        {
            // Вспомогательные переменные
            string strSQL_Update = "";
            string strLog = "";
            string errDB = "";

            // Очистка и загрузка структуры с описанием полей Update-запроса
            CFP.UpdatingField_Clear();
            CFP.UpdatingField_Add("strLogin", 1, edtLogin.Text.Replace("\\", "\\\\"));
            CFP.UpdatingField_Add("strFam", 1, edtFam.Text);
            CFP.UpdatingField_Add("strName", 1, edtName.Text);
            CFP.UpdatingField_Add("strOtch", 1, edtOtch.Text);
            CFP.UpdatingField_Add("strPodr", 1, edtPodr.Text);
            CFP.UpdatingField_Add("strDepartment", 1, edtDepartment.Text);
            CFP.UpdatingField_Add("strPost", 1, edtPost.Text);
            CFP.UpdatingField_Add("strPhone", 1, edtPhone.Text);
            CFP.UpdatingField_Add("strEmail", 1, edtEmail.Text);
            CFP.UpdatingField_Add("boolEnable", 0, cbUserEnable.Checked.ToString());

            // Создаем Update-запрос только по измененным полям
            if (CFP.Get_SQL_Update(qrySQL, "a_Users", "ID", ddlUserLogin.SelectedValue, true, UserData.UserName, ref strSQL_Update, ref strLog))
            {
                // Если есть изменения, то Update-им запись в БД и пишем лог изменений
                try
                {
                    // Внести изменения в таблицу tblSMJournal
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, strSQL_Update, ref errDB))
                        throw new Exception("Запись изменений пользователя");
                    // Запись в лог 
                    //CFP.AddLog(qrySQL, CC.App_Ver, UserData.UID, string.Format("таб Пользователи - изменение [ ID={0}, {1} ]", ddlUserLogin.SelectedValue, strLog));

                    // Скрыть панель ошибок
                    pnlError.Visible = false;
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка во время Апдейта, то выводим ее
                    lstError.Items.Clear();
                    CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                    CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ DB_Save_Data_UpdateUser() ]");
                    CFP.Error_Add_Msg(lstError.Items, errDB);
                    pnlError.Visible = lstError.Items.Count != 0;
                    return false;
                }
            }

            // ----- Запись прав пользователя -----------------------------------------------------------------------------------------------
            DB_Save_UserRigths(Convert.ToInt32(ddlUserLogin.SelectedValue));

            return true;
        }

        // ---- Запись прав/параметров пользователя -------------------------------------------------
        protected bool DB_Save_UserRigths(int UID)
        {

            //Save_Param_Int_for_User(MySqlCommand qrySQL, int UserUID, int intCode, int intValue)
            UserData.Save_Param_Int_for_User(qrySQL, UID, 100, Convert.ToInt32(ddlRole.SelectedValue));                         // Код роли пользователя   
            UserData.Save_Param_Str_for_User(qrySQL, UID, 100, ddlRole.SelectedItem.Text);                                      // Роль пользователя
            UserData.Save_Param_Int_for_User(qrySQL, UID, 114, CFP.BoolToInt(cbAdmin.Checked));                                 // Доступ к странице администрирования
            UserData.Save_Param_Int_for_User(qrySQL, UID, 105, CFP.BoolToInt(cbRequest.Checked));                               // Просмотр запросов на доступ
            UserData.Save_Param_Int_for_User(qrySQL, UID, 104, CFP.BoolToInt(cbViewDocIssue.Checked));                          // Доступ к журналу выдачи документов
            UserData.Save_Param_Int_for_User(qrySQL, UID, 106, CFP.BoolToInt(cbUserAdmin.Checked));                             // Доступ к странице администрирования
            UserData.Save_Param_Int_for_User(qrySQL, UID, 109, CFP.BoolToInt(cbSpr.Checked));                                   // Доступ к странице справочников
            UserData.Save_Param_Int_for_User(qrySQL, UID, 115, CFP.BoolToInt(cbDebug.Checked));                                 // Доступ к отладочной информации
            UserData.Save_Param_Str_for_User(qrySQL, UID, 101, CFP.Get_List_Selected_Value(cblGrif.Items, "-1"));               // Доступные грифы
            //UserData.Save_Param_Str_for_User(qrySQL, UID, 102, CFP.Get_List_Selected_Value(cblDocType.Items, "-1"));          // Доступные типы документов
            UserData.Save_Param_Str_for_User(qrySQL, UID, 103, CFP.Get_List_Selected_Value(cblArchiveRead.Items, "-1"));        // Доступные архивы для скачивания
            UserData.Save_Param_Int_for_User(qrySQL, UID, 113, CFP.BoolToInt(cbDocCreate.Checked));                             // Разрешение на создание документов
            UserData.Save_Param_Str_for_User(qrySQL, UID, 111, CFP.Get_List_Selected_Value(cblArchiveDocCreate.Items, "-1"));   // Доступные архивы для создания документов
            UserData.Save_Param_Int_for_User(qrySQL, UID, 116, CFP.BoolToInt(cbContCreate.Checked));                            // Разрешение на создание контентов
            //UserData.Save_Param_Str_for_User(qrySQL, UID, 112, CFP.Get_List_Selected_Value(cblContCreate.Items, "-1"));         // Доступные для создания контентов архивы
            UserData.Save_Param_Str_for_User(qrySQL, UID, 108, CFP.Get_List_Selected_Value(cblArchiveDocIssue.Items, "-1"));    // Доступные для выдачи документов архивы
            return true;
        }

        // ---- Чтение данных о пользователе
        protected bool DB_Read_UserData(int UID)
        {
            TUserData User = new TUserData();
            User.UserRightParamClear();
            User.UID = UID;
            User.GetUserData(qrySQL);

            edtLogin.Text = User.UserLogin;
            edtFam.Text = User.UserFam;
            edtName.Text = User.UserName;
            edtOtch.Text = User.UserOtch;
            edtPodr.Text = User.UserPodr;
            edtDepartment.Text = User.UserDepartment;
            edtPost.Text = User.UserPost;
            edtPhone.Text = User.UserPhone;
            edtEmail.Text = User.UserEmail;
            ddlRole.SelectedValue = User.intRole.ToString();
            cbUserEnable.Checked = User.UserEnable;

            // Заполнение cblGrif значениями из базы и установка текущих прав
            CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsGrifRead, User.Rights.str_Grif_Read), cblGrif);
            // Заполнение cblDocType значениями из базы и установка текущих прав
            //CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsDocTypeRead, User.Rights.str_DocType_Read), cblDocType);
            // Заполнение cblArchiveRead значениями из базы и установка текущих прав
            CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsArchRead, User.Rights.str_Arch_Read), cblArchiveRead);

            cbContCreate.Checked = User.Rights.bool_Cont_Create;

            //CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsContCreate, User.Rights.str_Cont_Write), cblContCreate);

            cbRequest.Checked = User.Rights.bool_Request_Read;
            cbViewDocIssue.Checked = User.Rights.bool_Journal_Read;
            cbAdmin.Checked = User.Rights.bool_Admin;
            cbDocCreate.Checked = User.Rights.bool_Doc_Write;

            CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsDocCreate, User.Rights.str_Doc_Write), cblArchiveDocCreate);

            cbUserAdmin.Checked = User.Rights.bool_User_Read;

            CFP.ReadCheckBoxList(qrySQL, string.Format(CConst.sqlRightsDocDelivery, User.Rights.str_Doc_Delivery), cblArchiveDocIssue);

            cbSpr.Checked = User.Rights.bool_Spr_Read;
            cbDebug.Checked = User.Rights.bool_DebugInfo;
            

            return User.UserLogin.Trim() != "";
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

        // ---- Чтение и обработка входных параметров
        protected void GetParams()
        {
            // Входные параметры на страницу
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
            AdminMenu.ApplyRight("Request", UserData.Rights.bool_Request_Read);
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Users", UserData.Rights.bool_User_Read);
            AdminMenu.ApplyRight("Spr", UserData.Rights.bool_Spr_Read);
            AdminMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);
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
            return (UserData.UID > CConst.UID_Guest);
        }

        protected void btnUserCreate_Click(object sender, EventArgs e)
        {
            int ID_User = 0;
            if (Check_Input_Data())
            { 
                // Запись данных о пользователе и его прав в базу
                if (DB_Save_Data_NewUser(ref ID_User))
                {
                    DataBind_ddlUserLogin();
                    CFP.DDL_SetValue(ddlUserLogin, ID_User.ToString());
                    ddlUserLogin_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void btnUserUpdate_Click(object sender, EventArgs e)
        {
            if (Check_Input_Data())
            {
                string ID_User = ddlUserLogin.SelectedValue;
                if (DB_Save_Data_UpdateUser())
                {
                    DataBind_ddlUserLogin();
                    CFP.DDL_SetValue(ddlUserLogin, ID_User);
                    ddlUserLogin_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void btnUserCancel_Click(object sender, EventArgs e)
        {
            // Переход на страницу администрирования
            Response.Redirect(CConst.pageAdmin);
        }
    }
}