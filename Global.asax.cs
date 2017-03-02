using BusinessLogic;
using DAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace TradeApi_WebDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            log.Warn("IIS Application TradeApi_WebDemo Started");
        }

        void Application_End(object sender, EventArgs e)
        {
            log.Warn("IIS Application TradeApi_WebDemo Exit");
        }


        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "SAMEORIGIN");
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");

            if (wcAppConfig.IsLive && (!Request.IsSecureConnection || !Request.Url.Host.Contains("www")))
            {
                string URL = "https://www." + Request.Url.Host.Replace("www.", "") + Request.Url.PathAndQuery.ToLower();
                Response.RedirectPermanent(URL, true);
            }
        }

        void Application_EndRequest(Object sender, EventArgs e)
        {

        }

        void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        void Application_Error(Object sender, EventArgs e)
        {
            string ExtraData = "";

            try
            {
                ExtraData += "<br /><b>SessionID:</b> " + Session.SessionID;
                for (int x = 0; x < Session.Count; x++)
                    ExtraData += Session.Keys[x] + " - " + Session[x];
            }
            catch
            {
                ExtraData += "<br /><b>SessionID:</b> FAILED";
            }

            ErrorLogic.HandleWebApplicationError(Request, Server, ExtraData);
        }
    }
}
