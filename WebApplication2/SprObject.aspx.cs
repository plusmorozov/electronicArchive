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
    public partial class SprObject : System.Web.UI.Page
    {
        const string sqlObjectType = "SELECT ID, strName FROM a_Object_Type ORDER BY strName";

        const string sqlInsert = "INSERT INTO a_Object( ID_Object_Type, strName, strInvNum, boolVisible ) VALUES( {0}, '{1}', '{2}', True )";
        const string sqlGetID = "SELECT ID FROM a_Object WHERE strName='{0}'";
        const string sqlUpdate = "UPDATE a_Object SET strName='{1}' WHERE ID={0}";
        
        // Создание объектов
        Const_Common    CConst      = new Const_Common();          // Объект общих констант и переменных
        Const_Docs      DConst      = new Const_Docs();            // Объект общих констант и переменных
        MySqlConnection qryCnn      = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand    qrySQL      = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc  CFP         = new CommonFuncProc();        // Объект с общими вспомогательными функциями и процедурами
        TUserData       UserData    = new TUserData();             // Объект данных пользователя

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
                CFP.ReadSpravochnik(qrySQL, sqlObjectType, ddlObjectType, 0, true, "0", "Все");
                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации

            qryObject.SelectCommand = Get_Table_SelectCommand();

        }

        protected string Get_Table_SelectCommand()
        {
            const string sqlTable = "SELECT O.ID, O.ID_Object_Type as ID_OT, O.strName, IFNULL(O.strInvNum, '') as strInvNum, OT.strNameShort as strNameShort " +
                                    "FROM a_Object O " +
                                    "INNER JOIN a_object_type OT ON ( OT.ID = O.ID_Object_Type ) " +
                                    "WHERE (O.ID_Object_Type={0} OR {0}=0) " +
                                    "ORDER BY O.strName ";
          
            return string.Format(sqlTable, ddlObjectType.SelectedValue);
        }

        // ----------------------------------------------------------------------------------------------------------------
        //      Обработчики сообщений от визуальных элементов (реакции на кнопки, выбор из списка и т.д.)
        // ----------------------------------------------------------------------------------------------------------------

        // Реакция нажатия на кнопку - Отмена
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageAdmin);
        }

        // -------- Справочник Организаций ------------------------------------------------------------------------------------------------------------------------------------------------------

        // Проверка на корректность введенных данных - Организации
        protected bool dbgTable_Check_Input_Data(int ID, DropDownList ddlObjectType, TextBox edtName, TextBox edtInvNum)
        {
            edtName.Text = edtName.Text.Trim();
            edtInvNum.Text = edtInvNum.Text.Trim();
            // Очистка списка ошибок
            lstError.Items.Clear();
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            if (ID <= 0) CFP.Error_Add_Msg(lstError.Items, "Ошибка идентификации данных. Обратитесь к администратору.");
            if (ddlObjectType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать \"Тип объекта\"");
            if (edtName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо ввести \"Наименование\" объекта");

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
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
                // Создать запись в таблице a_Objects
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsert, ddlType.SelectedValue, strRND, edtInvNum.Text), ref errDB))
                    throw new Exception("Вставка записи объекта");
                // Получить ID созданной записи
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(sqlGetID, strRND), 0, ref ID, ref errDB))
                    throw new Exception("Получение идентификатора новой записи");
                // Изменить служебные данные на пользовательские
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlUpdate, ID, edtName.Text), ref errDB))
                    throw new Exception("Вставка записи объекта (чистка служебных данных)");
                // Запись в лог 
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("спр Объекты - добавление [ ID={0} ]", ID));
                
                // Скрыть панель ошибок
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ dbgTable_Insert_Data() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }
        
        // Запись измененных данных - Организации
        
        protected bool dbgTable_Update_Data(int ID, GridViewRow Row)
        {
         
            string strSQL_Update = "";
            string strLog = "";
            string errDB = "";
            bool IsMyObject = true;

            CFP.UpdatingField_Clear();
            CFP.UpdatingField_Add("ID_Object_Type", 0, ((DropDownList)Row.FindControl("ddlType")).Text);
            CFP.UpdatingField_Add("strName", 1, ((TextBox)Row.FindControl("edtName")).Text);
            CFP.UpdatingField_Add("strInvNum", 1, ((TextBox)Row.FindControl("edtInvNum")).Text);

            try
            {
                // Создаем Update-запрос только по измененным полям
                if (CFP.Get_SQL_Update(qrySQL, "a_Object", "ID", Convert.ToString(ID), false, UserData.UserName, ref strSQL_Update, ref strLog))
                {
                    // Если есть изменения, то Update-им запись в БД и пишем лог изменений
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, strSQL_Update, ref errDB))
                        throw new Exception("Запись изменений данных объекта");
                    // Запись в лог 
                    CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("спр Объекты - изменение [ ID={0}, {1} ]", ID, strLog));
                }
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка во время Апдейта, то выводим ее
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных. Обратитесь к администратору.");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ dbgTable_Update_Data ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }

        // Обработка события сохранения измененных данных - Организации 
        protected void dbgTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(e.Keys[0]);
            GridViewRow Row = dbgTable.Rows[e.RowIndex];
            if (dbgTable_Check_Input_Data(ID, ((DropDownList)Row.FindControl("ddlType")), ((TextBox)Row.FindControl("edtName")),
                                              ((TextBox)Row.FindControl("edtInvNum")))
                && dbgTable_Update_Data(ID, Row))
            {
                dbgTable_RowCancelingEdit(null, null);
            }
            else e.Cancel = true;
        }

        protected void dbgTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lstError.Items.Clear();
            pnlError.Visible = false;
            btnTable_Create.Visible = true;
            pnlTableInsert.Visible = false;
            dbgTable.EditIndex = -1;
        }

        protected void dbgTable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                btnTable_Create.Visible = false;
                pnlTableInsert.Visible = false;
            }
        }

/*        protected void ddlCompany_DataBound(object sender, EventArgs e)
        {
        }
        */
        // Реакция нажатия на кнопку - Добавить
        protected void btnTable_Create_Click(object sender, EventArgs e)
        {
            dbgTable.EditIndex = -1;
            btnTable_Create.Visible = false;
            pnlTableInsert.Visible = true;

            edtName.Text = "";
            edtInvNum.Text = "";
            ddlObjectType.SelectedValue = "0";
        }

        // Обработка события вставки новых данных
        protected void btnTable_Create_Submit_Click(object sender, EventArgs e)
        {
            if (dbgTable_Check_Input_Data(9999, ddlType, edtName, edtInvNum)
                && dbgTable_Insert_Data())
            {
                dbgTable.DataBind();
                dbgTable_RowCancelingEdit(null, null);
            }
        }

        protected void btnTable_Create_Cancel_Click(object sender, EventArgs e)
        {
            dbgTable_RowCancelingEdit(null, null);
        }

        protected void chbOnlyMy_CheckedChanged(object sender, EventArgs e)
        {
            dbgTable.DataBind();
        }

        protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbgTable.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dbgTable.DataBind();
        }

        /*protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbgTable.DataBind();
        }
         * */

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
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

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest));
        }

        // ---- Чтение и обработка входных параметров
        protected void GetParams()
        {
            // Входные параметры на страницу
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
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