using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace NopFramework.Web.Framework
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Widget(this HtmlHelper helper, string widgetZone, object additionalData = null, string area = null)
        {
            return helper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone, additionalData = additionalData, area = area });
            //return helper.Action("WidgetsByZone", "Home", new { widgetZone = widgetZone, additionalData = additionalData, area = area });
        }

        #region 表单字段
        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            //create tag builder
            var builder = new TagBuilder("div");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "ico-help");
            var icon = new StringBuilder();
            icon.Append("<i class='fa fa-question-circle'></i>");
            builder.InnerHtml = icon.ToString();
            //render tag
            return MvcHtmlString.Create(builder.ToString());
            //sdf 
        }
        /// <summary>
        /// 自定义lable
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="displayHint"></param>
        /// <returns></returns>
        public static MvcHtmlString NopFrameowrkLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper,
               Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;
            result.Append(helper.LabelFor(expression, new { title = hintResource, @class = "control-label" }));
            object value;
            //if (metadata.AdditionalValues.TryGetValue("NopFrameworkResourceDisplayName", out value))
            //{
            //    result.Append(helper.Hint(value.ToString()).ToHtmlString());
            //}
            result.Append(helper.Hint("hello").ToHtmlString());

            var laberWrapper = new TagBuilder("div");
            laberWrapper.Attributes.Add("class", "label-wrapper");
            laberWrapper.InnerHtml = result.ToString();
            return MvcHtmlString.Create(laberWrapper.ToString());
        }
        public static MvcHtmlString NopFrameowrkEditorFor<TModel, TValus>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValus>> expression, bool? renderFormControlClass = null)
        {
            var result = new StringBuilder();
            object htmlAttributes = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
                (renderFormControlClass.HasValue && renderFormControlClass.Value))
                htmlAttributes = new { @class = "form-control" };
            result.Append(helper.EditorFor(expression, new { htmlAttributes }));
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString NopFramoworkDropDownList<TModel>(this HtmlHelper<TModel> helper, string name,
            IEnumerable<SelectListItem> itemList, object htmlAttributes = null,bool renderFormControlClass = true)
        {
            var result = new StringBuilder();
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if(renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);
            result.Append(helper.DropDownList(name, itemList, attrs));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString NopFramoworkDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> itemList,
            object htmlAttributes = null, bool renderFormControlClass = true)
        {
            var result = new StringBuilder();
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);
            result.Append(helper.DropDownListFor(expression, itemList, attrs));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString NopFramoworkDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression)
        {
            var result = new TagBuilder("div");
            result.Attributes.Add("class", "form-text-row");
            result.InnerHtml = helper.DisplayFor(expression).ToString();
            return MvcHtmlString.Create(result.ToString());
        }
        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control";
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control";

            return htmlAttributes as RouteValueDictionary;
        }
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }
        public static MvcHtmlString RenderBootstrapTabHeader(this HtmlHelper helper, string currentTabName,
            string title, bool isDefaultTab = false, string tabNameToSelect = "", string customCssClass = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;
            var a = new TagBuilder("a")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-tab-name",currentTabName),
                    new KeyValuePair<string, string>("href",string.Format("#{0}", currentTabName)),
                    new KeyValuePair<string, string>("data-toggle", "tab"),
                },
                InnerHtml = title
            };
            var liClassValue = "";
            if(tabNameToSelect==currentTabName)
            {
                liClassValue = "active";
            }
            if (!String.IsNullOrEmpty(customCssClass))
            {
                if (!String.IsNullOrEmpty(liClassValue))
                    liClassValue += " ";
                liClassValue += customCssClass;
            }
            var li = new TagBuilder("li")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("class", liClassValue),
                },
                InnerHtml = a.ToString(TagRenderMode.Normal)
            };

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
        /// <summary>
        /// 获取所选的选项卡名称
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string GetSelectedTabName(this HtmlHelper helper)
        {
            var tabName = string.Empty;
            const string dataKey = "nop.selected-tab-name";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.Controller.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.Controller.TempData[dataKey].ToString();

            return tabName;
        }
        /// <summary>
        /// 渲染所选索引内容的CSS样式
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="currentTabName"></param>
        /// <param name="content"></param>
        /// <param name="isDefaultTab"></param>
        /// <param name="tabNameToSelect"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderBootstrapTabContent(this HtmlHelper helper, string currentTabName,
           HelperResult content, bool isDefaultTab = false, string tabNameToSelect = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;

            var tag = new TagBuilder("div")
            {
                InnerHtml = content.ToHtmlString(),
                Attributes =
                {
                    new KeyValuePair<string, string>("class",string.Format("tab-pane{0}",tabNameToSelect==currentTabName?" active":"")),
                    new KeyValuePair<string, string>("id",string.Format("{0}", currentTabName))
                }
            };
            //tag.GenerateId(currentTabName);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));

        }
        #endregion
    }
}
