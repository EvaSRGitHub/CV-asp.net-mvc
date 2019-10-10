using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Helpers
{
    [HtmlTargetElement("tinymceEditor", Attributes = "asp-for")]
    public class TinymceEditorTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        public string Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "textarea";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "form-control tinymiceTextarea");
            output.Attributes.Add("id", "tinymiceTextarea");
            output.Attributes.Add("row", "5");
            output.Attributes.Add("name", this.AspFor.Name);
            output.Content.Append(Placeholder);
        }
    }
}
