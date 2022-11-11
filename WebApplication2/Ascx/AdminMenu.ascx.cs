using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Constants_Common;

namespace WebApplication2
{
    public partial class WebUserControl2 : System.Web.UI.UserControl
    {
        // Создание объектов
        Const_Common CConst = new Const_Common();             // Объект общих констант и переменных

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AdminMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (AdminMenu.SelectedValue)
            {
                case "Docs":
                    Response.Redirect(CConst.pageMain);
                    break;
                case "Request":
                    Response.Redirect(CConst.pageRequest);
                    break;
                case "Journal":
                    Response.Redirect(CConst.pageJournal);
                    break;
                case "Users":
                    Response.Redirect(CConst.pageUsers);
                    break;
                case "Admin":
                    Response.Redirect(CConst.pageAdmin);
                    break;
                case "Archive":
                    Response.Redirect(CConst.pageSprArchive);
                    break;
                case "Company":
                    Response.Redirect(CConst.pageSprCompany);
                    break;
                case "Storage_Building":
                    Response.Redirect(CConst.pageSprStorageBuilding);
                    break;
                case "Storage_Cabinet":
                    Response.Redirect(CConst.pageSprStorageCabinet);
                    break;
                case "Object":
                    Response.Redirect(CConst.pageSprObject);
                    break;
                case "Department":
                    Response.Redirect(CConst.pageSprDepartment);
                    break;
                case "Status":
                    Response.Redirect(CConst.pageSprStatus);
                    break;
                case "Object_Type":
                    Response.Redirect(CConst.pageSprObjectType);
                    break;
                case "Doc_Type":
                    Response.Redirect(CConst.pageSprDocType);
                    break;

            }
        }

        public void ApplyRight(string ItemName, bool Access)
        {
            MenuItem MI = AdminMenu.FindItem(ItemName);
            if (MI != null)
            {
                MI.Enabled = Access;
            }
        
        }

    }
}