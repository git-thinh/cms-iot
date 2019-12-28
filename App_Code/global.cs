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
using System.Collections.Concurrent;

public class oProduct
{
    public string Domain { set; get; }
    public string Key { set; get; }
    public string[] Images { set; get; }
    public string Title { set; get; }
    public string Price { set; get; }
    public string Description { set; get; }
    public string Content { set; get; }
}

public class global : System.Web.HttpApplication
{
    public override void Init()
    {
        //this.BeginRequest += page.OnBeginRequest;
    }

    const string _ROOT_PRODUCTS = @"custom\products";
    static ConcurrentDictionary<string, oProduct> _STORE = new ConcurrentDictionary<string, oProduct>() { };

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        string url = Request.Path.ToLower(), domain, file, text;
        switch (url)
        {
            case "/":
                domain = Request.Url.Host;
                if (domain == "onghutvn.com" || domain == "iot.vn")
                {
                    file = Path.Combine(Server.MapPath("~/"), domain, "index.html");
                    if (File.Exists(file))
                    {
                        text = File.ReadAllText(file);
                        text = text.Replace("___DOMAIN", domain);

                        HttpResponse response = base.Response;
                        response.ContentType = "text/html";
                        response.Write(text);
                        base.CompleteRequest();
                    }
                }
                else
                {
                    Context.RewritePath("/index.html");
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
