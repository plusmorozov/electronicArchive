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
using FuncProcDate;

namespace WebApplication2
{
    public partial class Jouranal : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common            CConst      = new Const_Common();           // Объект общих констант и переменных
        Const_Docs              DConst      = new Const_Docs();             // Объект общих констант и переменных
        MySqlConnection         qryCnn      = new MySqlConnection();        // Подключение к базе данных
        MySqlCommand            qrySQL      = new MySqlCommand();           // Организация работы с базой данных
        CommonFuncProc          CFP         = new CommonFuncProc();         // Объект с общими вспомогательными функциями и процедурами
        TUserData               UserData    = new TUserData();              // Объект данных пользователя
        DatePeriodExpression    DPE         = new DatePeriodExpression();   // Обработка дат

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
                // Вывод журнала выдачи документов конкретного архивариуса

                Date_DDL();                     // Получение текущего месяца и количества дней в месяце
                Filter_Changed(null, null);     // Перегрузка журнала выдачи в зависимости от выбранного периода времени

                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации
        }

        protected void Date_DDL()               // Получение текущего месяца и количества дней в месяце
        {
            DateTime DT = DateTime.Now;
            ddlStart_MM.SelectedValue = DT.Month.ToString();
            ddlStart_YY.SelectedValue = DT.Year.ToString();
            DataBind_Start_Days(null, null);
            ddlStart_DD.SelectedValue = "1";
            ddlFinish_MM.SelectedValue = DT.Month.ToString();
            ddlFinish_YY.SelectedValue = DT.Year.ToString();
            DataBind_Finish_Days(null, null);
            ddlFinish_DD.SelectedValue = DateTime.DaysInMonth(DT.Year, DT.Month).ToString();    // последний день месяца
        }

        // Реакция на изменение элементов фильтра
        protected void Filter_Changed(object sender, EventArgs e)
        {
            qryControl.SelectCommand = Get_SelectCommand();   // Изменяем условия в строке запроса согласно выбранному периоду времени
        }

        protected string Get_SelectCommand()
        {
            string strWhereAndValue = "{0} AND {1}={2}";
            string strWhereAndDate = "{0} AND {1}";
            string strWhereAndDateSearch = "{0} AND {1} LIKE '{2}'";
            string strFilter = "";

            strFilter = string.Format("R.intStatus != 0 AND sys_DateSolution BETWEEN STR_TO_DATE( '{0}-{1}-{2} 00:00:00', '%Y-%m-%d %H:%i:%s' ) AND STR_TO_DATE( '{3}-{4}-{5} 23:59:59', '%Y-%m-%d %H:%i:%s' ) AND R.ID_Archiver = {6}", ddlStart_YY.SelectedValue, ddlStart_MM.SelectedValue, ddlStart_DD.SelectedValue, ddlFinish_YY.SelectedValue, ddlFinish_MM.SelectedValue, ddlFinish_DD.SelectedValue, UserData.UID);                      

            return string.Format(CConst.sqlJournalList, strFilter);
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

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }

        protected void DataBind_Start_Days(object sender, EventArgs e)
        {
            CFP.DataBind_Days(ddlStart_DD.Items, Convert.ToInt32(ddlStart_YY.SelectedValue), Convert.ToInt32(ddlStart_MM.SelectedValue), true);
            //Filter_Changed(null, null);     // Перегрузка журнала выдачи в зависимости от выбранного периода времени
        }

        protected void DataBind_Finish_Days(object sender, EventArgs e)
        {
            CFP.DataBind_Days(ddlFinish_DD.Items, Convert.ToInt32(ddlFinish_YY.SelectedValue), Convert.ToInt32(ddlFinish_MM.SelectedValue), true);
            //Filter_Changed(null, null);     // Перегрузка журнала выдачи в зависимости от выбранного периода времени
        }

        protected void btn_ShowJournal_click(object sender, EventArgs e)
        {
            qryControl.SelectCommand = Get_SelectCommand();   // Изменяем условия в строке запроса согласно выбранному периоду времени
        }
    }
}