using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Constants_Common;
using MySql.Data.MySqlClient;
using FuncProc;
using UserData;

namespace WebApplication2
{
    public partial class Admin : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst = new Const_Common();             // Объект общих констант и переменных
        //Const_Docs DConst = new Const_Docs();                 // Объект общих констант и переменных
        MySqlConnection qryCnn = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL = new MySqlCommand();             // Организация работы с базой данных
        CommonFuncProc CFP = new CommonFuncProc();            // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData = new TUserData();                 // Объект данных пользователя

        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            // Получение и обработка параметров
            GetParams();

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            if (UserData.intRole != 2)                          // Проверка на администратора
            {
                Response.Redirect(CConst.pageMain);
            }

            if (!IsPostBack)                                    // Если первая загрузка страницы
            {
                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;    // Вывод отладочной информации

        }

        protected void btnSetTestUID_Click(object sender, EventArgs e)
        {
            Session["FhSsrb5684bhsfgBabtnaVsdv"] = edtTestUID.Text;
            Response.Redirect(CConst.pageMain);
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
            //  Если пользователь не найден, устанавливаем UID гостя
        }

        // ---- Применение прав доступа к элементам формы ----------------------------------------------------------------------------------------
        protected void Get_UserData_And_Applying_Access_Rights()
        {
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав

            // Применение прав к главному меню
            AdminMenu.ApplyRight("Docs", true);
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Users", UserData.Rights.bool_User_Read);
            AdminMenu.ApplyRight("Spr", UserData.Rights.bool_Spr_Read);
        }
    }
}