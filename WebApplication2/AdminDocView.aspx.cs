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
    public partial class WebForm7 : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common CConst     = new Const_Common();          // Объект общих констант и переменных
        Const_Docs DConst       = new Const_Docs();            // Объект констант документа
        Const_Content ContConst = new Const_Content();         // Объект констант контента
        MySqlConnection qryCnn  = new MySqlConnection();       // Подключение к базе данных
        MySqlCommand qrySQL     = new MySqlCommand();          // Организация работы с базой данных
        CommonFuncProc CFP      = new CommonFuncProc();        // Объект с общими вспомогательными функциями и процедурами
        TUserData UserData = new TUserData();                  // Объект данных пользователя

        int ID_Doc          = 0;                               // Уникальный идентификатор документа в журнале
        int ID_Cont         = 0;                               // Уникальный идентификатор контента
        string pageBackURL  = "";                              // Страница с которой был совершен переход на эту
        string pageCurrent  = "";                              // Эта страница

        protected void Page_Load(object sender, EventArgs e)
        {
            //pnlBreak.Visible = true;
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

            // Проверка на возврат
            if (!IsPostBack)                                    // Если первая загрузка страницы
            {
                DB_Read_Data(ID_Doc);                           // Чтение и загрузка данных документа
                // Присвоение значений переменным, используемым в UserControl AdminContent.ascx
                AdminContent.strWhereExpression = Get_WhereExpression();
                AdminContent.strOrderExpression = "strNameCont";

                // В двоичном представлении показывает последовательность отображаемых столбцов
                AdminContent.ColumnVisible = 247;

                // Передача прав пользователя в AdminContent.ascx
                AdminContent.UR = UserData.Rights;

                FutterSettings();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            if (qryCnn != null) qryCnn.Close();

        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest) & (ID_Doc > 0));
        }

        // ---- Применение прав доступа к элементам формы ----------------------------------------------------------------------------------------
        protected void Get_UserData_And_Applying_Access_Rights()
        {
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав
            
            // Применение прав к главному меню
            AdminMenu.ApplyRight("Docs", true);
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Request", UserData.Rights.bool_Request_Read);
            AdminMenu.ApplyRight("Users", UserData.Rights.bool_User_Read);
            AdminMenu.ApplyRight("Spr", UserData.Rights.bool_Spr_Read);
            AdminMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);

            //btnAddContent.Visible = UserData.Rights.bool_Cont_Create;
        }

        // Установки Подвала
        protected void FutterSettings()
        {
            Futter.App = CConst.App_Ver;                             // Вывод данных о приложении и версии
            Futter.Creater = CConst.CopyRight;                       // Вывод данных о создателе (копирайт)
            Futter.User = UserData.GetUserInfo();                    // Вывод данных о пользователе
            Futter.DebugVisible = UserData.Rights.bool_DebugInfo;    // Применение прав пользователя на отладочную информацию
        }

        protected void DB_Read_Data(int ID_Doc)
        {
            string errDB = "";
            
            qrySQL.Connection  = qryCnn;
            qrySQL.CommandText = string.Format(DConst.sqlReadDocView, ID_Doc);
            MySqlDataReader RS = qrySQL.ExecuteReader();
            if (RS.Read())
            {
                edtDocName.Text         = RS.GetString(RS.GetOrdinal("strDocName"));
                edtShifr.Text           = RS.GetString(RS.GetOrdinal("strShifr"));
                ddlYear.Text            = RS.GetString(RS.GetOrdinal("intYear"));
                ddlDocType.Text         = RS.GetString(RS.GetOrdinal("strDocType"));
                edtArchivedHuman.Text   = RS.GetString(RS.GetOrdinal("strArchivedHuman"));
                ddlSection.Text         = RS.GetString(RS.GetOrdinal("strSection"));
                ddlObjectType.Text      = RS.GetString(RS.GetOrdinal("strObjectType"));
                ddlObject.Text          = RS.GetString(RS.GetOrdinal("strObject"));
                ddlCustomer.Text        = RS.GetString(RS.GetOrdinal("strCustomer"));
                ddlExecuter.Text        = RS.GetString(RS.GetOrdinal("strExecutor"));
                edtNameWork.Text        = RS.GetString(RS.GetOrdinal("strNameWork"));
                ddlStorageBuild.Text    = RS.GetString(RS.GetOrdinal("strStorageBuilding"));
                ddlStoragePlace.Text    = RS.GetString(RS.GetOrdinal("strStoragePlace"));
                ddlStatus.Text          = RS.GetString(RS.GetOrdinal("strStatus"));
                chbklstDepartment.Text  = RS.GetString(RS.GetOrdinal("strDepartment"));
                edtDescription.Text     = RS.GetString(RS.GetOrdinal("strDescription"));
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageAdmin);
        }

        // ---- Чтение и обработка входных параметров
        protected void GetParams()
        {
            // Входные параметры на страницу
            ID_Doc  = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                           // Получение ID_Doc из Session 
            
            // Входные параметры на страницу
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
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