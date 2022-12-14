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

namespace Content
{
    public partial class AdminContentView : System.Web.UI.UserControl
    {
        // Созданние объектов
        MySqlConnection     qryCnn      = new MySqlConnection();    // Подключение к базе данных
        MySqlCommand        qrySQL      = new MySqlCommand();       // Организация работы с базой данных
        Const_Common        CConst      = new Const_Common();       // Объект общих констант и переменных
        Const_Content       ContConst   = new Const_Content();      // Объект констант контента
        CommonFuncProc      CFP         = new CommonFuncProc();     // Объект с общими вспомогательными функциями и процедурами
        TUserData           UserData    = new TUserData();          // Объект данных пользователя

        int ID_Doc = 0;                                             // Уникальный идентификатор документа в журнале

        // Запрос для получения данных о контенте
        public string sqlContent = "SELECT C.ID, C.strName as strNameCont, CT.strName as strNameType, G.strName as strGrif, G.ID as intGrif, SB.strName as strStorageBuilding, " +
                                          //"C.sys_DateCreate as strDateCreate, C.sys_DateModify as strDateModify, C.sys_verContent as intVerContent, (G.ID IN ({0})) as GR, (D.ID_Section IN ({1})) as ER " +
                                           "C.sys_DateCreate as strDateCreate, C.sys_DateModify as strDateModify, C.sys_verContent as intVerContent " +
                                   "FROM a_content C " +
                                   "LEFT JOIN a_content_type CT ON (C.ID_Type = CT.ID) " +
                                   "LEFT JOIN a_grif G ON (C.ID_Grif = G.ID) " +
                                   "LEFT JOIN a_spr_storage_building SB ON (C.ID_StorageBuilding = SB.ID)";

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

            //return string.Format(sqlContent, UR.str_Grif_Read) + Where + Order;
            //return string.Format(sqlContent, UR.str_Grif_Read, UR.str_Cont_Write, ID_Doc) + Where + Order;
            return sqlContent + Where + Order;
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
            Response.Redirect(CConst.pageAdminContPrevVersion);

        }

        // Реакция на клин по кнопке "Закрыть"
        protected void btnClose_Click(object sender, EventArgs e)
        {
            pnlBreak.Visible = false;
            pnlPrevContent.Visible = false;
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

        protected void dbgAttachedFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ContEdit")
            {
                // ID контента из базы
                Session[CConst.sp_ID_Cont] = Convert.ToInt32(e.CommandArgument);
                Response.Redirect(CConst.pageContEdit);
            }
            if (e.CommandName == "ContDownload")
            {
                CFP.AddLog(qrySQL, CConst.App_Ver, UserData.UID, string.Format("скачивание контента, ID документа = {0}, ID контента = {1}, ID_User = {2} ", ID_Doc, e.CommandArgument, UserData.UID));
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
                // Действия при запросе доступа к контенту
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