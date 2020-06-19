using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.Default
{
    public partial class MegaMenu
    {
        private int _tablesCount;

        private void MainFunction()
        {
            _tablesCount = GetTables().Count;
            var height = (_tablesCount / 3) ;
            height = height * 80;

            //var value1 = GetExpansionString("MEGA_MENU_HEIGHT");
            ExpanderUpdater(height.ToString(), "MEGA_MENU_HEIGHT",height);
            //var value2 = GetExpansionString("MEGA_MENU_HEIGHT");

            var settings = GetExpansionString("GRID_SETTINGS");

            if (!settings.IsBlank())
            {
                //var items = settings.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                //_gridConfiguration = items.Select(item => item.Split('='))
                //    .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1], StringComparer.InvariantCultureIgnoreCase);
            }

            var dropDown = "<a" +
                         Extensions.SetText("href", "#") +
                         Extensions.SetText("class", "dropdown-toggle nav-link text-dark") +
                         Extensions.SetText("data-toggle", "dropdown") +
                         Extensions.SetText("role", "button") +
                         Extensions.SetText("aria-haspopup","true") +
                         Extensions.SetText("aria-expanded","false") +
                         ">Tables</a>";

            var text = dropDown.AddCarriage() + MenuLinks();
            AppendText(text.Li("class", "dropdown dropdown-cols-2 nav-item"));
            
        }

        private string MenuLinks()
        {
            BuildSnippet(null);
            var lists = SplitList<ISchemaItem>(GetTables(), 3);

            foreach (var list in lists)
            {
                var text = string.Empty;
                foreach (var item in list)
                {
                    text += MenuLink(item.TableName, item.TableName.Pluralize()+"/Index")
                        .Li("class", "nav-link text-dark").AddCarriage();
                }

                BuildSnippet(text.Ul().HtmlTag("div"),0);
            }
            return BuildSnippet().HtmlTag("div", "class", "dropdown-menu");
        }

        private string MenuLink(string table, string page)
        {
            var result = "<a"+
                Extensions.SetText("class", "nav-link text-dark") +
                 Extensions.SetText("asp-area", "") +
                 Extensions.SetText("asp-page", "/" + page) + ">" + table + "</a>";
            return result.Indent(44);
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 5)
        {
            for (var i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        private static string SimpleIndent(string text, int indent, int tab)
        {
            var regex = new Regex("<.+?>");
            var matches = regex.Matches(text);
            var i = indent;
            var st = new StringBuilder();
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var a = groups[0].Value;
                st.Append(a.PadLeft(i));
                i += tab;
            }

            return st.ToString();
        }

    }
}