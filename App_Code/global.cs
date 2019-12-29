using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Mustache;
using System.Linq.Dynamic;

public class oOrder
{
    public long CreateDateTime { set; get; }
    public string Domain { set; get; }
    public string Name { set; get; }
    public string Phone { set; get; }
    public string Content { set; get; }
    public oOrder()
    {
        Name = string.Empty;
        Phone = string.Empty;
        Content = string.Empty;
        CreateDateTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
}

public class oView
{
    public string Domain { set; get; }
    public string Name { set; get; }
    public string Html { set; get; }
    public oView()
    {
        Html = string.Empty;
        Name = string.Empty;
        Domain = string.Empty;
    }
}

public class oMenu
{
    public string Domain { set; get; }
    public string Name { set; get; }
    public oMenu()
    {
        Name = string.Empty;
        Domain = string.Empty;
    }
}

public class oArticle
{
    public string Domain { set; get; }
    public string Key { set; get; }
    public string[] Menus { set; get; }
    public string[] Tags { set; get; }
    public string[] Images { set; get; }
    public string ImageThumb { set; get; }
    public string ImageBanner { set; get; }
    public string Title { set; get; }
    public string Price { set; get; }
    public string Description { set; get; }
    public string Content { set; get; }

    public oArticle()
    {
        Domain = string.Empty;
        Key = string.Empty;

        Menus = new string[] { };
        Tags = new string[] { };
        Images = new string[] { };
        ImageThumb = string.Empty;
        ImageBanner = string.Empty;

        Title = string.Empty;
        Price = string.Empty;
        Description = string.Empty;
        Content = string.Empty;
    }
}

public class global : System.Web.HttpApplication
{
    public static oArticle[] ___queryArticles(object values, string condition)
    {
        var a = values as oArticle[];
        var a1 = System.Linq.Queryable.AsQueryable(a).Where(condition);
        var a2 = System.Linq.Queryable.Cast<oArticle>(a1).ToArray();
        //enumerable = a2 as IEnumerable;
        return a2;
        //return new oArticle[] { };
    }

    private sealed class _MT_TEMP_URL_BUILD_TAG_DEFINITION : InlineTagDefinition
    {
        private static readonly string[] VietNamChar = new string[] {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public _MT_TEMP_URL_BUILD_TAG_DEFINITION() : base("url") { }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new TagParameter[] { new TagParameter("param") { IsRequired = false, DefaultValue = 123 } };
        }

        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope contextScope)
        {
            string str = arguments["param"] as string;

            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 1; i < VietNamChar.Length; i++)
                {
                    for (int j = 0; j < VietNamChar[i].Length; j++)
                        str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
                str = "../" + str.Replace(' ', '-').ToLower();
            }

            writer.Write(str);
        }
    }

    const string _ROOT_ARTICLES = @"custom\articles";
    static List<oArticle> _ARTICLES = new List<oArticle>() { };
    static List<oMenu> _MENUS = new List<oMenu>() { };
    static List<oView> _VIEWS = new List<oView>() { };
    static List<oOrder> _ORDERS = new List<oOrder>() { };

    void ___cache(string domain, string root)
    {
        string file, path, text;

        text = "[]";
        path = Path.Combine(root, _ROOT_ARTICLES);
        if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
        if (Directory.Exists(Path.Combine(root, "view")) == false) Directory.CreateDirectory(Path.Combine(root, "view"));

        _ARTICLES = _ARTICLES.Where(x => x.Domain != domain).ToList();
        _MENUS = _MENUS.Where(x => x.Domain != domain).ToList();
        _VIEWS = _VIEWS.Where(x => x.Domain != domain).ToList();

        string[] dirs = Directory.GetDirectories(path);
        if (dirs.Length > 0)
        {
            string[] menus = new string[] { };
            string[] lines;
            string f;
            for (int i = 0; i < dirs.Length; i++)
            {
                f = Path.Combine(dirs[i], "doc.txt");
                if (File.Exists(f))
                {
                    lines = File.ReadAllLines(f);
                    if (lines.Length > 3)
                    {
                        try
                        {
                            menus = lines[0].Split(';').Select(x => x.Trim()).ToArray();
                            for (int k = 0; k < menus.Length; k++)
                            {
                                if (_MENUS.Exists(x => x.Domain == domain && x.Name.ToLower() == menus[k].ToLower()) == false)
                                {
                                    _MENUS.Add(new oMenu() { Domain = domain, Name = menus[k] });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }

                        try
                        {
                            oArticle o = new oArticle() { Domain = domain, Menus = menus };
                            o.Key = Path.GetFileName(dirs[i]);
                            if(lines.Length > 0) o.Tags = lines[1].Split(';');
                            if (lines.Length > 1) o.Title = lines[2];

                            string[] imgs = Directory.GetFiles(dirs[i], "*.jpg")
                                .Select(x => x.Replace(root, string.Empty).Replace("\\", "/").ToLower()).ToArray();
                            if (imgs.Length > 0)
                            {
                                o.ImageThumb = imgs.Where(x => x.Contains("thumb")).SingleOrDefault();
                                o.ImageBanner = imgs.Where(x => x.Contains("banner")).SingleOrDefault();
                                o.Images = imgs.Where(x => !x.Contains("thumb")).ToArray();
                            }

                            if (lines.Length > 2) o.Price = lines[3];
                            if (lines.Length > 3) o.Description = lines[4];
                            if (lines.Length > 4) o.Content = string.Join(Environment.NewLine, lines.Where((x, ii) => ii > 4));

                            _ARTICLES.Add(o);
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }
                    }
                }
            }
        }

        dirs = Directory.GetFiles(Path.Combine(root, "view"));
        if (dirs.Length > 0)
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                file = Path.GetFileName(dirs[i]);
                _VIEWS.Add(new oView()
                {
                    Domain = domain,
                    Name = file.Substring(0, file.Length - 5),
                    Html = File.ReadAllText(dirs[i])
                });
            }
        }
    }

    string ___render(string domain, string text, object article) {

        var views = _VIEWS.Where(x => x.Domain == domain).ToArray();
        for (int i = 0; i < views.Length; i++) text = text.Replace("{{#!" + views[i].Name + "}}", views[i].Html);

        FormatCompiler compiler = new FormatCompiler();
        compiler.RegisterTag(new _MT_TEMP_URL_BUILD_TAG_DEFINITION(), true);
        Generator generator = compiler.Compile(text);
        generator.KeyNotFound += (obj, args) =>
        {
            args.Substitute = string.Empty;
            args.Handled = true;
        };

        text = generator.Render(new
        {
            domain = domain,
            menus = _MENUS.Where(x => x.Domain == domain).ToArray(),
            articles = _ARTICLES.Where(x => x.Domain == domain).ToArray(),
            article = article
        });

        return text;
    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        string url = Request.Path.ToLower(),
            domain = Request.Url.Host,
            root = Path.Combine(Server.MapPath("~/"), domain + "\\"),
            key = url.Substring(1), file, path, text;

        if (key != "cache" && _ARTICLES.Count == 0) ___cache(domain, root);

        switch (key)
        {
            case "":
                #region
                if (domain == "onghutvn.com" || domain == "iot.vn")
                {
                    file = Path.Combine(root, "index.html");
                    if (File.Exists(file))
                    {
                        text = File.ReadAllText(file);
                        text = ___render(domain, text, new oArticle());
                        
                        base.Response.ContentType = "text/html";
                        base.Response.Write(text);
                        base.CompleteRequest();
                    }
                }
                else
                {
                    Context.RewritePath("/index.html");
                }
                #endregion
                break;
            case "cache":
                #region

                ___cache(domain, root);

                text = JsonConvert.SerializeObject(new
                {
                    domain = domain,
                    menus = _MENUS.Where(x => x.Domain == domain).ToArray(),
                    articles = _ARTICLES.Where(x => x.Domain == domain).ToArray(),
                    views = _VIEWS.Where(x => x.Domain == domain).ToArray(),
                });

                base.Response.ContentType = "application/json";
                base.Response.Write(text);
                base.CompleteRequest();

                #endregion
                break;
            case "admin":
            case "admin/":
            case "admin.html":
                Context.RewritePath("/admin.htm");
                break;
            case "api/order/submit":
                #region

                string name = Context.Request.Form["name"],
                    phone = Context.Request.Form["phone"],
                    comment = Context.Request.Form["comment"];

                if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(name)) {
                    _ORDERS.Add(new oOrder() { Content = comment, Domain = domain, Name = name , Phone = phone });
                }

                base.Response.Redirect("/");
                base.CompleteRequest();
                #endregion
                break;
            case "api/order/list":
                #region 
                text = JsonConvert.SerializeObject(_ORDERS.Where(x => x.Domain == domain));
                base.Response.ContentType = "text/html";
                base.Response.Write(text);
                base.CompleteRequest();
                #endregion
                break;
            case "api/test":
                #region 
                //text = JsonConvert.SerializeObject(_ORDERS.Where(x => x.Domain == domain));
                //base.Response.ContentType = "text/html";
                //base.Response.Write(text);
                base.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", domain));
                base.CompleteRequest();
                #endregion
                break;
            default:
                #region 
                if (domain == "onghutvn.com" || domain == "iot.vn")
                {
                    if (key.IndexOf('/') == -1)
                    {
                        oArticle article = _ARTICLES.Where(x => x.Domain == domain && x.Key == url.Substring(1)).SingleOrDefault();
                        if (article != null)
                        {
                            file = Path.Combine(root, "article.html");
                            if (File.Exists(file))
                            {
                                text = File.ReadAllText(file);
                                text = ___render(domain, text, article);

                                base.Response.ContentType = "text/html";
                                base.Response.Write(text);
                                base.CompleteRequest();
                            }
                        }
                    }
                }
                else
                { 
                }
                #endregion
                break;
        }



    }

}
