using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgDouble.TagHalpers
{
   /* [HtmlTargetElement("td", Attribute ="MyText")]*/
    public class TdTagHelper : TagHelper
    {

        public string  MyText { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", $"my-text-{MyText}");
           /* output.PreElement.SetHtmlContent( "<del>");
            output.PostElement.SetHtmlContent("</del>");*/

        }
    }

  
}
