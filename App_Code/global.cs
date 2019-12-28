using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Mustache;
using System.Linq.Dynamic;

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

    const string _ROOT_PRODUCTS = @"custom\products";
    static List<oArticle> _ARTICLES = new List<oArticle>() { };
    static List<oMenu> _MENUS = new List<oMenu>() { };
    static List<oView> _VIEWS = new List<oView>() { };

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        string url = Request.Path.ToLower(),
            domain = Request.Url.Host,
            root = Path.Combine(Server.MapPath("~/"), domain),
            file, path, text;
        switch (url)
        {
            case "/":
                #region
                if (domain == "onghutvn.com" || domain == "iot.vn")
                {
                    file = Path.Combine(root, "index.html");
                    if (File.Exists(file))
                    {
                        text = File.ReadAllText(file);

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
                            articles = _ARTICLES.Where(x => x.Domain == domain).ToArray()
                        });

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
            case "/cache":
                #region

                text = "[]";
                path = Path.Combine(root, _ROOT_PRODUCTS);
                if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
                if (Directory.Exists(Path.Combine(root, "view")) == false) Directory.CreateDirectory(Path.Combine(root, "view"));

                _ARTICLES = _ARTICLES.Where(x => x.Domain != domain).ToList();
                _MENUS = _MENUS.Where(x => x.Domain != domain).ToList();
                _VIEWS = _VIEWS.Where(x => x.Domain != domain).ToList();

                string[] dirs = Directory.GetDirectories(path);
                if (dirs.Length > 0)
                {
                    string[] menus;
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
                                menus = lines[0].Split(';').Select(x => x.Trim()).ToArray();
                                for (int k = 0; k < menus.Length; k++)
                                {
                                    if (_MENUS.Exists(x => x.Domain == domain && x.Name.ToLower() == menus[k].ToLower()) == false)
                                    {
                                        _MENUS.Add(new oMenu() { Domain = domain, Name = menus[i] });
                                    }
                                }

                                oArticle o = new oArticle() { Domain = domain, Menus = menus };
                                o.Key = Path.GetFileName(dirs[i]);
                                o.Tags = lines[1].Split(';');
                                o.Title = lines[2];

                                string[] imgs = Directory.GetFiles(dirs[i], "*.jpg").Select(x => x.Replace(root, string.Empty).Replace("\\", "/")).ToArray();
                                if (o.Images.Length > 0)
                                {
                                    o.ImageThumb = imgs.Where(x => x.Contains("thumb")).SingleOrDefault();
                                    o.ImageBanner = imgs.Where(x => x.Contains("banner")).SingleOrDefault();
                                    o.Images = imgs.Where(x => !x.Contains("thumb")).ToArray();
                                }

                                _ARTICLES.Add(o);
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
            case "/admin":
            case "/admin/":
            case "/admin.html":
                Context.RewritePath("/admin.htm");
                break;
        }



    }

}
