using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BlockClass;
using Newtonsoft.Json;

namespace BlockLink
{
    /// <summary>
    /// Service 的摘要说明
    /// </summary>
    [WebService(Namespace = "")]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        string BlockPath = HttpContext.Current.Server.MapPath("/") + "blockPool";

        [WebMethod]
        public string HelloWorld(string say)
        {
            return "Hello World," + say;
        }

        [WebMethod]
        public void GetBlockLink()
        {
            BlockDL bl = new BlockDL(BlockPath);
            bl.getBlockFullLink(BlockPath);
            Context.Response.Write(JsonConvert.SerializeObject(bl.BlockLink));
            Context.Response.End();
        }

        [WebMethod]
        public void CreatBlock(string data,string user)
        {
            BlockDL bl = new BlockDL(BlockPath);
            string patch = BlockPath + "\\" + user + "\\";
            bl.getBlockFullLink(BlockPath);
            //bl.writeBlock(bl.BlockLink[0], patch);
            bl.writeBlock(bl.cBlock(bl.BlockLink[bl.BlockLink.Count - 1], data), patch);

            Context.Response.Write(JsonConvert.SerializeObject(bl.BlockLink));
            Context.Response.End();
        }
    }
}
