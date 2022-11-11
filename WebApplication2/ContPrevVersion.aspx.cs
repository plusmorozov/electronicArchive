using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Constants_Common;
using FuncProc;
using MySql.Data.MySqlClient;
using UserData;

namespace WebApplication2
{
    public partial class ContPrevVersion : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst = new Const_Common();               // Объект общих констант и переменных
        CommonFuncProc CFP = new CommonFuncProc();              // Объект с общими вспомогательными функциями и процедурами
        MySqlConnection qryCnn = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL = new MySqlCommand();             // Организация работы с базо
        TUserData UserData = new TUserData();                 // Объект данных пользователя

        int ID_Doc = 0;                                         // Уникальный идентификатор документа в журнале
        int ID_Cont = 0;                                        // Уникальный идентификатор контента
        string pageCurrent = "";                                // Эта страница
        string pageBackURL = "";                                // Страница с которой был совершен переход на эту


        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            pageCurrent = CConst.pageContPrevVersion;

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
                // Присвоение значений переменным, используемым в UserControl Content.ascx
                Content.strWhereExpression = Get_WhereExpression();
                Content.strOrderExpression = "strDateModify";
                // В двоичном представлении показывает последовательность отображаемых столбцов
                Content.ColumnVisible = 507;
                // Передача прав пользователя в Content.ascx
                Content.UR = UserData.Rights;

                FutterSettings();
            }
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest) & (ID_Doc > 0) & (ID_Cont > 0));
        }

        // ---- Применение прав доступа к элементам формы ----------------------------------------------------------------------------------------
        protected void Get_UserData_And_Applying_Access_Rights()
        {
            string errDB = "";
            bool qwe = true;
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав
            // Применение прав к главному меню
            MainMenu.ApplyRight("Docs", true);
            MainMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);
        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }

        protected string Get_WhereExpression()
        {
            // Ограничение на вывод контентов с определенным ID
            string strFilter = string.Format(" boolIsActual=false AND (ID_DATA IN (SELECT ID_DATA FROM a_content WHERE ID={0}))", ID_Cont); // boolIsActual=false AND - чтобы выводить только предыдущие версии, без актуальной
            return strFilter;
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
            ID_Doc = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                           // Получение ID_Doc из Session 
            ID_Cont = CFP.GetDigitParam(CConst.sp_ID_Cont, 0);                         // Получение ID_Cont из Session 

            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageDocView);
        }
    }
}