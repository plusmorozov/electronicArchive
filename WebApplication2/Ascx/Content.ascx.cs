using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Constants_Common;
using Constants_Content;
using FuncProc;
using UserData;
using System.DirectoryServices;

namespace Content
{
    public partial class ContentView : System.Web.UI.UserControl
    {
        // Созданние объектов
        MySqlConnection     qryCnn      = new MySqlConnection();    // Подключение к базе данных
        MySqlCommand        qrySQL      = new MySqlCommand();       // Организация работы с базой данных
        Const_Common        CConst      = new Const_Common();       // Объект общих констант и переменных
        Const_Content       ContConst   = new Const_Content();      // Объект констант контента
        CommonFuncProc      CFP         = new CommonFuncProc();     // Объект с общими вспомогательными функциями и процедурами
        TUserData           UserData    = new TUserData();          // Объект данных пользователя

        int ID_Doc = 0;                                             // Уникальный идентификатор документа в журнале
        int ID_Cont = 0;                                            // Уникальный идентификатор контента
        int ID_User = 0;
        string strLogin = "";

        // Запрос для получения данных о контенте
        public string sqlContent = "SELECT C.ID, C.strName as strNameCont, CT.strName as strNameType, G.strName as strGrif, G.ID as intGrif, SB.strName as strStorageBuilding, " +
            //"C.sys_DateCreate as strDateCreate, C.sys_DateModify as strDateModify, C.sys_verContent as intVerContent, ((G.ID IN ({0}))&(D.ID_Section IN ({1}))) as GR " +
            //"C.sys_DateCreate as strDateCreate, C.sys_DateModify as strDateModify, C.sys_verContent as intVerContent, (((G.ID IN ({0}))&(D.ID_Section IN ({1}))) OR ((R.intStatus = 1)&(NOW()>R.sys_DateStart)&(NOW()<R.sys_DateFinish))) as GR " +
                                   "C.sys_DateCreate as strDateCreate, C.sys_DateModify as strDateModify, C.sys_verContent as intVerContent, GetContentAction({0}, C.ID, D.ID) as GR " +
                                   "FROM a_content C " +
                                   "LEFT JOIN a_content_type CT ON (C.ID_Type = CT.ID) " +
                                   "LEFT JOIN a_doc D ON (D.ID = C.ID_Doc) " +
                                   "LEFT JOIN a_grif G ON (C.ID_Grif = G.ID) " +
                                   "LEFT JOIN a_spr_storage_building SB ON (C.ID_StorageBuilding = SB.ID)";
                                   //"LEFT JOIN a_request R ON (R.ID_Content = C.ID)";

        // Объявление свойств для создания ограничений в SQL запросе и отображении столбцов GridView
        public string   strWhereExpression  { get; set; }
        public string   strOrderExpression  { get; set; }
        public int      ColumnVisible       { get; set; }
        public TUserData.TUserRight UR      { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Чтение параметров приложения из Web.config
            ReadAppWebConfigiration();
            // Загрузка свойств
            ViewState_Load();

            qryCnn.ConnectionString = CConst.strConn;
            qryCnn.Open();
            qrySQL.Connection = qryCnn;
            
            GetParams();                                        // Получение и обработка параметров
            
            // Получение данных о пользователе и его прав, Применение прав, Вывод данных о пользователе
            Get_UserData_And_Applying_Access_Rights();

            if (!IsPostBack)
            {
                Content_DataBinding(null, null);
                //dbgAttachedFiles
            }
        }

        // ---- Применение прав доступа к элементам формы ----------------------------------------------------------------------------------------
        protected void Get_UserData_And_Applying_Access_Rights()
        {
            // --- Чтение данных и прав пользователя
            UserData.GetUserData(qrySQL);                                                                   // Получение данных о пользователе и его прав
        }

        protected void Content_DataBinding(object sender, EventArgs e)
        {
            qryContent.SelectCommand = Get_SelectCommand();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState_Save();
        }

        protected void ViewState_Load()
        {
            if (ViewState["Cont_Where"] != null) strWhereExpression = (string)ViewState["Cont_Where"];
            if (ViewState["Cont_Order"] != null) strOrderExpression = (string)ViewState["Cont_Order"];
            if (ViewState["Cont_ColumnVisible"] != null) ColumnVisible = (int)ViewState["Cont_ColumnVisible"];
        }
        
        protected void ViewState_Save()
        {
            ViewState["Cont_Where"] = strWhereExpression;
            ViewState["Cont_Order"] = strOrderExpression;
            ViewState["Cont_ColumnVisible"] = ColumnVisible;
        }

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            qryCnn.Close();
        }

        public string Get_SelectCommand()
        {
            string Where = "";
            string Order = "";

            if (strWhereExpression != null && strWhereExpression.Trim() != "") Where = " WHERE " + strWhereExpression;
            if (strOrderExpression != null && strOrderExpression.Trim() != "") Order = " ORDER BY " + strOrderExpression;

            return string.Format(sqlContent, UserData.UID) + Where + Order;
            //return sqlContent + Where + Order;
        }

        // Реакция на клик по контенту
        protected void linkContClick(object sender, CommandEventArgs e)
        {
            string strNameContentSystem = "";
            string strNameContentReal = "";

            qrySQL.Connection = qryCnn;
            qrySQL.CommandText = string.Format(ContConst.sqlNameContent, e.CommandArgument);
            MySqlDataReader RS = qrySQL.ExecuteReader();
            if (RS.Read())
            {
                strNameContentSystem = RS.GetString(RS.GetOrdinal("strNameContentSystem"));
                strNameContentReal = RS.GetString(RS.GetOrdinal("strNameContentReal"));
            }
            RS.Close();

            Response.Clear();
            Response.Charset = "";
            // Как будет называться сохраняемый на локальной машине файл
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strNameContentReal);
            // Где лежит файл на сервере
            Response.WriteFile(CConst.ContentPath + strNameContentSystem);
            Response.End();
        }

        // Реакция на клик по версии контента
        protected void linkVerClick(object sender, CommandEventArgs e)
        {
            //pnlBreak.Visible = true;
            //pnlPrevContent.Visible = true;
            // Сохраняем значение ID контента по версии которого был клик
            Session[CConst.sp_ID_Cont] = e.CommandArgument;
            // Редирект на страницу просмотра предыдущих версий
            Response.Redirect(CConst.pageContPrevVersion);

        }

        // Реакция на клик по кнопке "Закрыть"
        protected void btnClose_Click(object sender, EventArgs e)
        {
            pnlBreak.Visible = false;
            pnlAddUser.Visible = false;
        }

        // Реакция на клик по кнопке "Закрыть"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка заполнения необходимых полей
            if (Check_Input_Data())
                // Создание пользователя
                if (DB_Save_Data())
                {
                    // Отправка запроса на доступ к контенту от имени нового пользователя
                    SendRequest(CFP.GetDigitParam(CConst.sp_ID_Cont, 0), ID_User);
                    // Редирект на главную страницу
                    Response.Redirect(CConst.pageMain);
                }
        }

        // ---- Проверка корректность ввода данных ------------------------------------------------------------------------------
        protected bool Check_Input_Data()
        {
            lstError.Items.Clear();              // Очистка списка ошибок
            // --- Проверка корректности введенных о пользователе данных ---------------------------------------------------------
            edtLogin.Text = edtLogin.Text.Trim();
            if (edtLogin.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать логин пользователя");
            edtFam.Text = edtFam.Text.Trim();
            if (edtFam.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать фамилию пользователя");
            edtName.Text = edtName.Text.Trim();
            if (edtName.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать имя пользователя");
            edtOtch.Text = edtOtch.Text.Trim();
            if (edtOtch.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать отчество пользователя");
            edtPodr.Text = edtPodr.Text.Trim();
            if (edtPodr.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать подразделение");
            edtDepartment.Text = edtDepartment.Text.Trim();
            if (edtDepartment.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать отдел");
            edtDolzh.Text = edtDolzh.Text.Trim();
            if (edtDolzh.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать должность");
            edtPhone.Text = edtPhone.Text.Trim();
            if (edtPhone.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать Телефон");
            edtEmail.Text = edtEmail.Text.Trim();
            if (edtEmail.Text == "") CFP.Error_Add_Msg(lstError.Items, "Необходимо указать Email");
            // --- Завершение проверки данных элементов ----------------------------------------------------------------------
            
            // Если есть ошибки ввода данных, то вывести список ошибок
            pnlError.Visible = lstError.Items.Count != 0;
            return lstError.Items.Count == 0;
        }

        // ---- Запись данных пользователя в БД -------------------------------------------------
        protected bool DB_Save_Data()
        {
            // Вспомогательные переменные
            string errDB = "";
            string strRND = CFP.GetRNDString(30);
            string sqlAddUser = "INSERT INTO a_users ( strLogin, strFam, strName, strOtch, strPodr, strDepartment, strPost, strPhone, strEmail, boolEnable, sys_UserModify, sys_DateModify, strDescr) VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', true, '{9}', NOW(), '{10}' )";
            string sqlClearDescr = "UPDATE a_users SET strDescr='' WHERE ID={0}";

            try
            {
                // Создать запись в таблице a_users
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlAddUser, edtLogin.Text.Replace("\\", "\\\\"), edtFam.Text, edtName.Text, edtOtch.Text, edtPodr.Text, edtDepartment.Text, edtDolzh.Text, edtPhone.Text, edtEmail.Text, edtFam.Text + ' ' + edtName.Text + ' ' + edtOtch.Text, strRND ), ref errDB))
                    throw new Exception("Вставка записи о новом пользователе");
                
                // Получить ID созданного пользователя
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format("SELECT ID FROM a_users WHERE strDescr='{0}'", strRND), 0, ref ID_User, ref errDB))
                    throw new Exception("Получение идентификатора нового пользователя");
                
                // Очистить strDescription
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlClearDescr, ID_User), ref errDB))
                    throw new Exception("Очистка примечания от strRND)");
                
                // Добавить в таблицу a_params запись с intCode = 100, value = 4, чтобы создать роль пользователя
                UserData.Save_Param_Int_for_User(qrySQL, ID_User, 100, 4);

                /*
                                                
                // Записать в strDescr документа реальное значение вместо strRND
                if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlUpdateDescr, edtDescription.Text, ID_Doc), ref errDB))
                    throw new Exception("Запись данных нового документа (чистка Примечания от мусора)");

                // Очистить Примечание от мусора
                //if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(DConst.sqlClearDescr, ID_Doc), ref errDB))
                //    throw new Exception("Запись данных нового документа (чистка Примечания от мусора)");
                */
                
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("создание пользователя, логин = {0}", strLogin));
                // Скрыть панель ошибок
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                lstError.Items.Clear();
                CFP.Error_Add_Msg(lstError.Items, "Ошибка выполнения запроса к базе данных");
                //CFP.Error_Add_Msg(lstError.Items, ex.Message + " [ DB_Save_Data_SM_Osn() ]");
                CFP.Error_Add_Msg(lstError.Items, errDB);
                pnlError.Visible = lstError.Items.Count != 0;
                return false;
            }
            return true;
        }
        

        // Ограничение на показ колонок
        protected void gwTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ColumnVisible = ColumnVisible | 1;  // всегда показывать поле с нименованием документа - первое поле - 0h00000001

            int V = ColumnVisible;
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Visible = (V & 1) == 1;
                V = V >> 1;
            }
        }

        protected void ReadAppWebConfigiration()
        {
            // Строка подключения
            CConst.strConn = Convert.ToString(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ArchiveConnString"]);
            // Имя и версия приложения
            CConst.App_Ver = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["App_Name"] + " " + System.Web.Configuration.WebConfigurationManager.AppSettings["App_Ver"]);
            CConst.CopyRight = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CopyRight"]);
        }

        protected void SendRequest(int ID_Cont, int UID)
        {
            // Действия при запросе доступа к контенту
            string errDB = "";
            int CountRequest = 0;
            try
            {
                // Проверка существования запроса на доступ к контенту (чтобы не создавать несколько одинаковых записей в a_request)
                if (!CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(CConst.sqlCountRequest, ID_Cont, UID), 0, ref CountRequest, ref errDB))
                    throw new Exception("Проверка существования запроса на доступ к контенту");

                if (CountRequest == 0)
                {
                    // Запись информации о запросе доступа к контенту в базу данных
                    if (!CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(CConst.sqlUserRequest, UID, ID_Cont), ref errDB))
                        throw new Exception("Запись информации о запросе доступа к контенту в базу данных");
                }

                // Запуск перерисовки content.ascx, чтобы при запросе на доступ сразу вместо замка показывалась пиктограмма загрузки
                Content_DataBinding(null, null);

                CFP.AddLog(qrySQL, CConst.App_Ver, UID, string.Format("запрос на доступ, ID документа - {0}, ID контента = {1}", ID_Doc, ID_Cont));
            }
            catch (Exception ex)
            {
            }
        }

        protected void dbgAttachedFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ContDownload")
            {
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("скачивание контента, ID документа - {0}, ID контента = {1}", ID_Doc, e.CommandArgument));

                string strNameContentSystem = "";
                string strNameContentReal = "";

                qrySQL.Connection = qryCnn;
                qrySQL.CommandText = string.Format(ContConst.sqlNameContent, e.CommandArgument);
                MySqlDataReader RS = qrySQL.ExecuteReader();
                if (RS.Read())
                {
                    strNameContentSystem = RS.GetString(RS.GetOrdinal("strNameContentSystem"));
                    strNameContentReal = RS.GetString(RS.GetOrdinal("strNameContentReal"));
                }

                RS.Close();

                Response.Clear();
                Response.Charset = "";
                // Как будет называться сохраняемый на локальной машине файл
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strNameContentReal);
                // Где лежит файл на сервере
                Response.WriteFile(CConst.ContentPath + strNameContentSystem);
                Response.End();

                
            }
            if (e.CommandName == "ContRequest")
            {
                // ID контента к которому запрашивается доступ
                Session[CConst.sp_ID_Cont] = e.CommandArgument;

                // Если пользователь Гость, то выводить форму для регистрации нового пользователя
                if (UserData.UID == 1)
                {
                    // Показать панель pnlAddUser для заполнения пользователем данных о себе
                    pnlBreak.Visible = true;
                    pnlAddUser.Visible = true;

                    // Подключение к AD
                    System.DirectoryServices.DirectoryEntry ADEntry = new System.DirectoryServices.DirectoryEntry("WinNT://" + HttpContext.Current.User.Identity.Name.Replace("\\", "/"));
                    // Получение имени учетной записи пользователя из AD
                    edtLogin.Text = "OGP\\" + ADEntry.Properties["Name"].Value.ToString();
                    // Получение ФИО пользователя из AD
                    string FullName = ADEntry.Properties["FullName"].Value.ToString();

                    // Разделение ФИО на отдельные слова
                    string[] FIO = FullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in FIO)
                    {
                        edtFam.Text = FIO[0];
                        edtName.Text = FIO[1];
                        edtOtch.Text = FIO[2];
                    }
                }
                // Если не Гость, отправить запрос на доступ к контенту
                else
                {
                    SendRequest(Convert.ToInt32(e.CommandArgument), UserData.UID);
                }
            }

        }

        protected void gwTable_Sorting(object sender, GridViewSortEventArgs e)
        {
            qryContent.DataBind();
        }

        public void Refresh()
        {
            qryContent.DataBind();
        }

        // ---- Чтение и обработка входных параметров
        protected void GetParams()
        {
            // Входные параметры на страницу
            ID_Doc = CFP.GetDigitParam(CConst.sp_ID_Doc, 0);                           // Получение ID_Doc из Session 
            // Входные параметры на страницу
            UserData.GetUserUID_for_Login(qrySQL, HttpContext.Current.User.Identity.Name.Replace("\\", "\\\\"), CConst.UID_Guest);    //  - UID    - Ищем идентификатор пользователя (по нему определяем права) 
        }

        protected void dbgAttachedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}