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
    public partial class Request : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common    CConst      = new Const_Common();       // Объект общих констант и переменных
        Const_Docs      DConst      = new Const_Docs();         // Объект общих констант и переменных
        MySqlConnection qryCnn      = new MySqlConnection();    // Подключение к базе данных
        MySqlCommand    qrySQL      = new MySqlCommand();       // Организация работы с базой данных
        CommonFuncProc  CFP         = new CommonFuncProc();     // Объект с общими вспомогательными функциями и процедурами
        TUserData       UserData    = new TUserData();          // Объект данных пользователя

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

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            // Проверка на возврат
            if (!IsPostBack)                                        // Если первая загрузка страницы
            {
                // Вывод запросов на доступ к контенту согласно правам архивариуса
                qryControl.SelectCommand = string.Format(CConst.sqlShowRequest, UserData.Rights.str_Doc_Delivery);
                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            if (qryCnn != null) qryCnn.Close();

        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
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

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest));
        }

        protected void dbgRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Вспомогательные переменные
            int     ID_Cont = 0;
            string  errDB   = "";
            string  strFIO  ="";


            // Создание объекта строки
            GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            // Получение ID контента к которому запрашивается доступ
            if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format("SELECT ID_Content FROM a_request WHERE id = {0}", e.CommandArgument), 0, ref ID_Cont, ref errDB))
                throw new Exception("Получение идентификатора контента к которому запрашивается доступ");

            // Получение ФИО пользователя запросившего доступ
            if (!CFP.SQL_GetOneValue_String(qrySQL, string.Format("SELECT  CONCAT(GETFIO(U.strFam, U.strName, U.strOtch,2), ' ', U.strPost, ' ', U.strDepartment, ' ',  U.strPodr) as strUserInfo FROM a_users U LEFT JOIN a_request R ON (R.ID_User = U.ID) WHERE R.id = {0}", e.CommandArgument), "", ref strFIO, ref errDB))
                throw new Exception("Получение ФИО пользователя запросившего доступ");

            if (e.CommandName == "AllowAccess")
            {
                // Действия при разрешении доступа к контенту
                // Обновление запроса на доступ
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(CConst.sqlUpdateRequest, ((DropDownList)Row.FindControl("ddlDay")).SelectedValue, 1, e.CommandArgument, UserData.UID), ref errDB))
                    throw new Exception("Обновление запроса на доступ - разрешение доступа");
                
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("разрешение доступа к контенту для {0}, дней = {1} , ID контента = {2}", strFIO, ((DropDownList)Row.FindControl("ddlDay")).SelectedValue, ID_Cont));
                Response.Redirect(CConst.pageRequest);
            }
            if (e.CommandName == "DenyAccess")
            {
                // Действия при запрете доступа к контенту
                // Обновление запроса на доступ
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(CConst.sqlUpdateRequest, 0, 2, e.CommandArgument, UserData.UID), ref errDB))
                    throw new Exception("Обновление запроса на доступ - запрет доступа");

                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("отказ в разрешении доступа к контенту для {0}, ID контента = {1}", strFIO, ID_Cont));
                Response.Redirect(CConst.pageRequest);
            }
            
        }

    }
}