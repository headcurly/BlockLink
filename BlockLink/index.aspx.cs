using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlockClass;
using Newtonsoft.Json;

namespace BlockLink
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.title.Text = "小区块链程序";
            ///bulidBlock();
        }
        public void bulidBlock()
        {
            //BlockDL bl = new BlockDL();
            //this.console.Value = JsonConvert.SerializeObject(bl.BlockLink);
        }
    }
}