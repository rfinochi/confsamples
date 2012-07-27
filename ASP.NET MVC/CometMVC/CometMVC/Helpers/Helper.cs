using System.Web;
using System.Web.Mvc;
using System;

public static class Helper
{
    public static string Script(this HtmlHelper html, string scriptName)
    {

        return Script(scriptName);

    }

    public static string Script(string scriptName)
    {

        if (!scriptName.StartsWith("~"))
            scriptName = "~/Scripts/" + scriptName;

        var t = new TagBuilder("script");
        t.Attributes.Add("type", "text/javascript");
        t.Attributes.Add("src", AbsoluteUrl(scriptName));
        return t.ToString(TagRenderMode.Normal);

    }

    public static string AbsoluteUrl(string virtualPath)
    {
        // this is for tests to work
        return HttpContext.Current == null ? virtualPath : VirtualPathUtility.ToAbsolute(virtualPath);
    }

}