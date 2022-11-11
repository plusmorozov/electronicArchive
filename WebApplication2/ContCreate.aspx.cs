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
    public partial class WebForm3 : System.Web.UI.Page
    {
        // Создание объектов
        Const_Common    CConst      = new Const_Common();       // Объект общих констант и переменных
        Const_Content   ContConst   = new Const_Content();      // Объект констант контента
        Const_Docs      DConst      = new Const_Docs();         // Объект констант документа
        MySqlConnection qryCnn      = new MySqlConnection();    // Подключение к базе данных
        MySqlCommand    qrySQL      = new MySqlCommand();       // Организация работы с базой данных
        CommonFuncProc  CFP         = new CommonFuncProc();     // Объект с общими вспомогательными функциями и процедурами
        TUserData       UserData    = new TUserData();          // Объект данных пользователя

        int    ID_Doc      = 0;                                 // Уникальный идентификатор документа в журнале
        string pageCurrent = "";                                // Эта страница

        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();

            pageCurrent = CConst.pageDocCreate;
            lblApp.Text = CConst.App_Ver;
            lblCreater.Text = CConst.CopyRight;

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;

            int ID_Doc = 0;

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
                // Заполнение ddlContType значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprContType, ddlContType, 0, true, "0", "");
                // Заполнение ddlGrif значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprGrif, ddlGrif, 0, true, "0", "");
                // Заполнение ddlStorageBuild значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprStorageBuild, ddlStorageBuild, 0, true, "0", "");
                // Заполнение ddlStorageBuild значениями из базы
                CFP.ReadSpravochnik(qrySQL, CConst.sqlSprContCarrier, ddlContCarrier, 0, true, "0", "");
            }
        }

        protected void GetParams()
        {
            // Входные параметры на страницу
            ID_Doc = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                           // Получение ID_Doc из Session 
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        // Проверка входных параметров
        protected bool CheckParams()
        {
            return ((UserData.UID > CConst.UID_Guest) & (ID_Doc > 0));
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
            AdminMenu.ApplyRight("Journal", UserData.Rights.bool_Journal_Read);
            AdminMenu.ApplyRight("Request", UserData.Rights.bool_Request_Read);
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

        // Реакция на изменение значения типа объекта (ddlDocType)
        protected void ddlStorageBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind_StoragePlace();
        }

        // ---- Запрос данных об объектах подразделения ------------------------------------------
        public void DataBind_StoragePlace()
        {
            CFP.ReadSpravochnik(qrySQL, string.Format(DConst.sqlSprStoragePlace, ddlStorageBuild.SelectedValue), ddlStoragePlace, 0, true, "0", " ");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageDocCreate);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Check_Input_Data())
                if (DB_Save_Data())
                {
                    pnlBreak.Visible = true;
                    pnlAddContent.Visible = true;
                }
        }
   
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            edtContName.Text = "Контент " + CFP.GetRNDString(10); ;
            ddlContType.SelectedValue = "1";
            ddlGrif.SelectedValue = "1";
            ddlStorageBuild.SelectedValue = "1";
            ddlStoragePlace.SelectedValue = "1";
            ddlContCarrier.SelectedValue = "1";
        }

        // Проверка корректности входных данных
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();              // Очистка списка ошибок
            // --- Проверка данных элементов ---------------------------------------------------------------------------------------
            edtContName.Text = edtContName.Text.Trim();
            if (edtContName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать указать название контента");
            if (ddlContType.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип контента");
            if (ddlGrif.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать гриф");
            if (ddlStorageBuild.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать здание, в котором находится документ");
            if (ddlStoragePlace.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать кабинет, в котором находится документ");
            if (ddlContCarrier.SelectedValue == "0") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать тип физического носителя");
            if (fuFileUpload.FileName == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо выбрать файл для загрузки");

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }


        // ---- Запись данных контента в БД -------------------------------------------------
        protected bool DB_Save_Data()
        {
            // Вспомогательные переменные
            int ID_Cont = 0;
            string errDB = "";
            string strRND = CFP.GetRNDString(30);
            string strNameSystem = "";
            string strNameReal = "";
            string strFullFileName = "";

          
            // Проверка корректности данных о прикрепляемом файле и их запись в базу
            try
            {
                // Формирование имени файла - ГодМесяцДень_ЧасМинутаСекунда.РасширениеФайла
                strNameSystem = DateTime.Now.ToString("yyMMdd_HHmmss") + fuFileUpload.FileName.Substring(fuFileUpload.FileName.LastIndexOf("."));   // Генерация уникального имени файла для хранения на сервере
                strNameReal = fuFileUpload.FileName;                                                                                                // Сохранение реального имени загружаемого файла
                strFullFileName = CConst.ContentPath + strNameSystem;
                //Путь к месту хранения файлов на сервере
                //strFullFileName = "10.43.61.5\\admin\\SHD_Test\\" + strFileName;                                                                  // Полный путь к файлу на сервере
                
                // Запись информации о загруженном файле в базу данных
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlAttachFile, ID_Doc, edtContName.Text, ddlContType.SelectedValue, ddlGrif.SelectedValue, ddlStorageBuild.SelectedValue,
                                                                   ddlStoragePlace.SelectedValue, ddlContCarrier.SelectedValue, strNameReal, strNameSystem, DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), "Admin", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), "Admin", 1, strRND), ref errDB))
                    throw new Exception("Запись информации о прикрепляемом файле в базу данных");

                // Получить ID созданной записи
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(ContConst.sqlGetCont_ID, strRND), 0, ref ID_Cont, ref errDB))
                    throw new Exception("Получение идентификатора нового контента");

                // Глобально для проекта сохранить номер созданного контента
                Session[CConst.sp_ID_Cont] = ID_Cont;

                // Записать ID_Cont в поле ID_DATA контента, чтобы отслеживать версионность
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlUpdateID_Data, ID_Cont, ID_Cont), ref errDB))
                    throw new Exception("Запись ID родительского контента");

                // Очистить Примечание от мусора
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlClearDescr, edtDescription.Text, ID_Cont), ref errDB))
                    throw new Exception("Запись данных нового контента (чистка Описания от мусора)");


                /*-------------------- Загрузка файла --------------------------------*/
                // Upload файла
                fuFileUpload.SaveAs(strFullFileName);

                // Прроверка на существование файла (т.е. все за аплойдилось)                    
                if (!System.IO.File.Exists(strFullFileName))
                    throw new Exception("Ошибка загрузки файла на сервер (проверка Upload)");

                /*-------------------- Конец загрузки файла --------------------------------*/


                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("создание контента, ID документа - {0}, ID контента = {1}", ID_Doc, ID_Cont));
                // Создать запись в таблице a_doc_content, содержащую информацию о связи между контентом и документом
                //if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(ContConst.sqlInsertDocContent, ID_Doc, ID_Cont), ref errDB))
                //    throw new Exception("Запись данных о связи контента и документа");


                // Скрыть панель ошибок
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка во время Аплоада файла, то выводим ее
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка прикрепления файла к документу");
                CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ File_Attach() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }

            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;

            return true;
        }

        protected void btnAddContent_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageContCreate);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect(CConst.pageAdmin);
        }

    }
}