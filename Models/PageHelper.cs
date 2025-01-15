using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace iBlog.Models;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageHelper(IUrlHelperFactory urlFactoryHelper) : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    public string? Category { get; set; }
    public PageInfo? PageModel { get; set; }
    public string? PageAction { get; set; }
    public bool PageClassEnabled { get; set; } = false;
    public bool IsPage { get; set; } = false;
    public string PageSelectedClass { get; set; } = String.Empty;
    public string PageUnselectedClass { get; set; } = String.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PageModel != null && ViewContext != null)
        {
            IUrlHelper urlHelper = urlFactoryHelper.GetUrlHelper(ViewContext);
            TagBuilder div = new("div");
            div.AddCssClass("btn-group");
            TagBuilder first = new("a");
            if (IsPage)
            {
                first.Attributes.Add("href", urlHelper.Page(PageAction, new { blogPage = 1, category = Category }));
            }
            else
                first.Attributes.Add("href", urlHelper.Action(PageAction, new { blogPage = 1, category = Category }));
            first.InnerHtml.Append("First");
            first.AddCssClass("btn btn-purple");
            div.InnerHtml.AppendHtml(first);

            for (int i = Math.Max(1, (PageModel.CurrentPage+1) - 2); i <= Math.Min(PageModel.CurrentPage + 3, PageModel.TotalPages); i++)
            {
                TagBuilder a = new("a");
                if (PageClassEnabled)
                {
                    a.AddCssClass(i == PageModel.CurrentPage ? PageSelectedClass : PageUnselectedClass);
                }
                if (IsPage)
                {
                    a.Attributes.Add("href", urlHelper.Page(PageAction, new { blogPage = i, category = Category }));

                }
                else
                    a.Attributes.Add("href", urlHelper.Action(PageAction, new { blogPage = i, category = Category }));
                a.InnerHtml.Append(i.ToString());
                div.InnerHtml.AppendHtml(a);
            }
            TagBuilder last = new("a");
            if (IsPage)
            {

                last.Attributes.Add("href", urlHelper.Page(PageAction, new { blogPage = PageModel.TotalPages, category = Category }));
            }
            else
                last.Attributes.Add("href", urlHelper.Action(PageAction, new { blogPage = PageModel.TotalPages, category = Category }));
            last.InnerHtml.Append("Last");
            last.AddCssClass("btn btn-purple");
            div.InnerHtml.AppendHtml(last);
            output.Content.AppendHtml(div);
        }
        base.Process(context, output);
    }



}