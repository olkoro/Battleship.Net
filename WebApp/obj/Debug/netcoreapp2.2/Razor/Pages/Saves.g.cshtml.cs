#pragma checksum "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "36e6417f4ae681bf2c5412731d70233355a0c6d0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WebApp.Pages.Pages_Saves), @"mvc.1.0.razor-page", @"/Pages/Saves.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Saves.cshtml", typeof(WebApp.Pages.Pages_Saves), null)]
namespace WebApp.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/_ViewImports.cshtml"
using WebApp;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"36e6417f4ae681bf2c5412731d70233355a0c6d0", @"/Pages/Saves.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56255396305d1d1888ad93afc9c47568e44a4220", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Saves : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(32, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 4 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml"
  
    ViewData["Title"] = "Save Loading";

#line default
#line hidden
            BeginContext(78, 25, true);
            WriteLiteral("\n<!DOCTYPE html>\n\n<html>\n");
            EndContext();
            BeginContext(103, 34, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "36e6417f4ae681bf2c5412731d70233355a0c6d03395", async() => {
                BeginContext(109, 21, true);
                WriteLiteral("\n    <title></title>\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(137, 1, true);
            WriteLiteral("\n");
            EndContext();
            BeginContext(138, 614, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "36e6417f4ae681bf2c5412731d70233355a0c6d04574", async() => {
                BeginContext(144, 208, true);
                WriteLiteral("\n<div id=\"tablica\" class=\"content\">\n    <div class=\"text-center\">\n        <h1 class=\"display-5\">Saves 💾</h1>\n    </div>\n     <table id=\"saves\" class=\"tablecolors\" border=\"0\" style=\"width: 50%\">\n     <tbody>\n");
                EndContext();
#line 21 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml"
       foreach (var save in Model.Save)
      {
          var saveString = save.ToString();
          //@Html.DisplayFor(model => saveString)
          //<br/>

#line default
#line hidden
                BeginContext(512, 67, true);
                WriteLiteral("          <tr>\n              <td class=\"my-bold my-bborder element\"");
                EndContext();
                BeginWriteAttribute("onclick", " onclick=\"", 579, "\"", 632, 3);
                WriteAttributeValue("", 589, "location.href=\'Place?cmd=load-", 589, 30, true);
#line 27 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml"
WriteAttributeValue("", 619, save.SaveId, 619, 12, false);

#line default
#line hidden
                WriteAttributeValue("", 631, "\'", 631, 1, true);
                EndWriteAttribute();
                BeginContext(633, 20, true);
                WriteLiteral(">\n                  ");
                EndContext();
                BeginContext(654, 10, false);
#line 28 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml"
             Write(saveString);

#line default
#line hidden
                EndContext();
                BeginContext(664, 37, true);
                WriteLiteral("\n              </td>\n          </tr>\n");
                EndContext();
#line 31 "/Users/olkoro/CodeProjects/battleship-by-oleg-korotkov/WebApp/Pages/Saves.cshtml"
      }

#line default
#line hidden
                BeginContext(709, 36, true);
                WriteLiteral("     </tbody>\n     </table>\n\n</div>\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(752, 8, true);
            WriteLiteral("\n</html>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebApp.Pages.Saves> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<WebApp.Pages.Saves> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<WebApp.Pages.Saves>)PageContext?.ViewData;
        public WebApp.Pages.Saves Model => ViewData.Model;
    }
}
#pragma warning restore 1591
