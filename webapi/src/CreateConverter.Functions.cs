using System.Collections.Generic;
using System.Windows.Forms;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;
using static ZeraSystems.CodeNanite.Expansion.General;
  
namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateConverter
    {
        #region Fields

        private List<ISchemaItem> _columns;
        private ISchemaItem _tableObject;

        #endregion Fields

        private void MainFunction()
        {
            GetTable(Input);
            _tableObject = GetTableObject(Input);
            _columns = GetColumns(Input, false);

            AppendText();
            if (_tableObject != null)
            {
                var currentTable = _tableObject.OriginalName;
                AppendText(GetConvert(currentTable, _columns, 8).AddCarriage());
                AppendText(GetConvertList(currentTable, _columns, 8));
            }
        }

        private string GetConvert(string table, IEnumerable<ISchemaItem> columns, int indent)
        {
            BuildSnippet("public static "+table+"ViewModel Convert("+table+" "+table.ToLower()+")");
            BuildSnippet("{");
            BuildSnippet("var "+table.ToLower()+"ViewModel = new " + table + "ViewModel();",indent+4);
            foreach (var column in columns)
            {
                if (column.IsCalculatedColumn) continue;
                BuildSnippet(table.ToLower() + "ViewModel." + column.ColumnName + " = " +table.ToLower()+"."+ column.ColumnName + ";", indent+4);
            }

            BuildSnippet("return " +table.ToLower() + "ViewModel;",indent+4);
            BuildSnippet("}");
            return BuildSnippet();
        }

        private string GetConvertList(string table, IEnumerable<ISchemaItem> columns, int indent)
        {
            BuildSnippet("public static List<" + table + "ViewModel> ConvertList(IEnumerable<" + table + "> " + table.ToLower().Pluralize() + ")");
            BuildSnippet("{");

            BuildSnippet("return " +table.ToLower().Pluralize()+".Select(a =>",indent+4);
            BuildSnippet("{",indent+8);
            BuildSnippet("var model = new " + table + "ViewModel();",indent+12);

            foreach (var column in columns)
            {
                if (column.IsCalculatedColumn) continue;
                BuildSnippet("model." + column.ColumnName + " = a." + column.ColumnName + ";",indent+12);
            }

            BuildSnippet("return model;",indent+12);
            BuildSnippet("})",indent+8);
            BuildSnippet(".ToList();",indent+8);
            BuildSnippet("}",indent);
            return BuildSnippet();
        }

    }
}
