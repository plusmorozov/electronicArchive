using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Ascx
{
    public partial class Futter : System.Web.UI.UserControl
    {
        public string Debug1
        {
            get { return lblDebug1.Text; }
            set { lblDebug1.Text = value; }
        }

        public string Debug2
        {
            get { return lblDebug2.Text; }
            set { lblDebug2.Text = value; }
        }

        public string User
        {
            get { return lblUser.Text; }
            set { lblUser.Text = value; }
        }

        public string Creater
        {
            get { return lblCreater.Text; }
            set { lblCreater.Text = value; }
        }

        public string App
        {
            get { return lblApp.Text; }
            set { lblApp.Text = value; }
        }

        public bool DebugVisible
        {
            get { return pnlDebug.Visible; }
            set { pnlDebug.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}