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
    public partial class SprArchive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Закрытие соединения с базой данных
            //if (qryCnn != null) qryCnn.Close();

        }
      
    }
}