using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Constants_Common;

namespace WebApplication2
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        // Создание объектов
        Const_Common CConst = new Const_Common();             // Объект общих констант и переменных

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void MainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (MainMenu.SelectedValue)
            {
                case "Docs":
                    Response.Redirect(CConst.pageMain);
                    break;
                case "Admin":
                    Response.Redirect(CConst.pageAdmin);
                    break;

            }
        }

        public void ApplyRight(string ItemName, bool Access)
        {
            MenuItem MI = MainMenu.FindItem(ItemName);
            if (MI != null)
            {
                MI.Enabled = Access;
            }
        
        }

    }
}