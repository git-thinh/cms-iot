//using api.Interface;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web;
using System.Linq;
using System.Web.Hosting;
using System.IO;

public class global : System.Web.HttpApplication
{
    public override void Init()
    {
        //this.BeginRequest += page.OnBeginRequest;
    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        string url = Request.Path.ToLower(), domain, file, text;
        switch (url)
        {
            case "/":
                domain = Request.Url.Host;
                //Context.RewritePath("/index.html");

                //Context.Response.StatusCode = 200;
                ////Context.Response.Status = "";

                //Response.Write("LAST END: ");
                //Response.Write(" END COUNT: ");

                //var httpApplication = sender as HttpApplication;
                //httpApplication.CompleteRequest();

                file = Path.Combine(Server.MapPath("~/"), domain, "index.html");
                if (File.Exists(file))
                {
                    text = File.ReadAllText(file);

                    HttpResponse response = base.Response;
                    response.ContentType = "text/html";
                    response.Write(text);
                    base.CompleteRequest();
                }

                break;
            case "/admin":
            case "/admin/":
            case "/admin.html":
                Context.RewritePath("/admin.htm");
                break;
        }
    }


    protected void Application_Start(object sender, EventArgs e)
    {
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Session_Start(object sender, EventArgs e)
    {
    }

    void Session_End(object sender, EventArgs e)
    {
    }


}
