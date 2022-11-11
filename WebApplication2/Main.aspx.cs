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
    public partial class WebForm1 : System.Web.UI.Page
    {
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

            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            
            // Проверка на возврат
            if (!IsPostBack)                                        // Если первая загрузка страницы
            {
                Fill_Data_Spravochnik(qrySQL);                      // Вывод списка документов
                Filter_Changed(null, null);

                FutterSettings();
            }
            Futter.Debug1 = CFP.Get_Session_Params() + "<br />Роль=" + UserData.strRole;            // Вывод отладочной информации

            // Вывод текста из info1, info2, info3 из web.config на странице (например оповещения об изменениях)
            //lblInfo.Text = "";
            /*string S = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["Info1"]).Trim();
            if (S != "") lblInfo.Text += S + "<br />";
            S = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["Info2"]).Trim();
            if (S != "") lblInfo.Text += S + "<br />";
            S = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["Info3"]).Trim();
            if (S != "") lblInfo.Text += S + "<br />";
            */
            //pnlAdminInfo.Visible = lblInfo.Text.Length != 0;

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
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав
            // Применение прав к главному меню
            MainMenu.ApplyRight("Docs", true);
            MainMenu.ApplyRight("Admin", UserData.Rights.bool_Admin);

            // Показывать кнопку тестирования пользователям только администраторам
            if ((UserData.intRole == 1) & (UserData.UID != 0))
            {
                btnTestUser.Visible = true;
            }
            btnStopTest.Visible = CFP.GetDigitParam("FhSsrb5684bhsfgBabtnaVsdv", 0) > 0;
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

        // ---- Чтение и установка данных справочников формы -------------------------------------------------------------------------------------------------------
        protected void Fill_Data_Spravochnik(MySqlCommand qrySQL)
        {
            /// Установка значений по умолчанию для выпадающих списков на "Все"
            CFP.ReadSpravochnik(qrySQL, CConst.sqlSprDocType,   ddlDocType, 0, true, "0", "Все");
            CFP.ReadSpravochnik(qrySQL, CConst.sqlSprSection,   ddlSection, 0, true, "0", "Все");
            CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStatus,    ddlStatus,  0, true, "0", "Все");

            // Установка запомненных значений выпадающих списков при вовзращении на главную страницу
            ddlDocType.SelectedValue = CFP.GetStringParam(CConst.sp_DocType,    "0");
            ddlYear.SelectedValue    = CFP.GetStringParam(CConst.sp_Year,       "0");
            ddlSection.SelectedValue = CFP.GetStringParam(CConst.sp_Section,    "0");
            ddlStatus.SelectedValue  = CFP.GetStringParam(CConst.sp_Status,     "0");
        }

        protected string Get_SelectCommand()
        {
            // Проверка на глобальное разрешение чтения списка документов
            //if (!UserData.Rights.bool_DocList_Read) return sqlDocListNull;

            string strWhereAndValue = "{0} AND {1}={2}";
            string strWhereAndDate  = "{0} AND {1}";
            string strWhereAndDateSearch = "{0} AND {1} LIKE '{2}'";
            string strFilter = "";

            // Фильтр - Подразделения
            if (ddlDocType.SelectedValue != "0") strFilter = string.Format(strWhereAndValue, strFilter, "DT.ID", ddlDocType.SelectedValue);

            // Фильтр по году
            if (ddlYear.SelectedValue != "0") strFilter = string.Format(strWhereAndValue, strFilter, "D.intYear", ddlYear.SelectedValue);

            // Фильтр по подразделению
            if (ddlSection.SelectedValue != "0") strFilter = string.Format(strWhereAndValue, strFilter, "SEC.ID", ddlSection.SelectedValue);

            // Фильтр статусу оцифровки
            if (ddlStatus.SelectedValue != "0") strFilter = string.Format(strWhereAndValue, strFilter, "StatusDoc.ID", ddlStatus.SelectedValue);
            
            // Формирование условия для поиска документа по имени
            if (edtSearch.Text != "") strFilter = string.Format(strWhereAndDateSearch, strFilter, "D.strName", edtSearch.Text);

            return string.Format(DConst.sqlDocList, strFilter);
        }

        // ----------------------------------------------------------------------------------------------------------------
        //      Обработчики сообщений от визуальных элементов (реакции на кнопки, выбор из списка и т.д.)
        // ----------------------------------------------------------------------------------------------------------------

        // Реакция на изменение элементов фильтра
        protected void Filter_Changed(object sender, EventArgs e)
        {
            qryControl.SelectCommand = Get_SelectCommand();                                                   // Изменяем условия в строке запроса согласно установкам фильтров

            // Запомнить настройки фильтра на главной странице
            Session[CConst.sp_DocType]  = ddlDocType.SelectedValue;
            Session[CConst.sp_Year]     = ddlYear.SelectedValue;
            Session[CConst.sp_Section]  = ddlSection.SelectedValue;
            Session[CConst.sp_Status]   = ddlStatus.SelectedValue;
        }

        protected void btnTestUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageTestUser);
        }

        protected void btnStopTest_Click(object sender, EventArgs e)
        {
            Session["FhSsrb5684bhsfgBabtnaVsdv"] = null;
            Response.Redirect(CConst.pageMain);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Реакция на клик по документу
        protected void linkDocClick(object sender, CommandEventArgs e)
        {
            // Указать проекту текущий выбранный документ
            Session[CConst.sp_ID_Doc] = e.CommandArgument;
            //Session[CConst.sp_IsNewDoc] = CConst.spvIsNewDoc_NO;
            Response.Redirect(CConst.pageDocView);
        }

        protected void dbgMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*if (e.CommandName == "DocEdit")
            {
                // Указать проекту текущий выбранный документ
                Session[CConst.sp_ID_Doc] = e.CommandArgument;
                //Session[CConst.sp_IsNewDoc] = CConst.spvIsNewDoc_NO;
                Response.Redirect(CConst.pageDocEdit);
            }*/
        }

        protected void dbgMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageDocEdit);
        }    

    }
}