using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateViewModel
    {
        private void MainFunction()
        {
            AppendText();
            AppendText(Indent(4) + "public class " + Singularize(Input) + "ViewModel");
            AppendText(Indent(4) + "{");
            AddColumns();
            AppendText(Indent(4) + "}");
        }

        private void AddColumns()
        {
            var columns = GetColumnsAndNavigation(Input);
            var theIndent = Indent(8);
            var colsResult = string.Empty;
            var navResult = string.Empty;
            var vmResult = string.Empty;
            foreach (var item in columns)
            {
                if (item.IsForeignKey && item.TableName != Input)
                    navResult += GetCollectionNavigation(theIndent, item);
                else
                {
                    colsResult += GetColumnProperty(theIndent, item);
                    //if (item.IsForeignKey && !item.LookupDisplayColumn.IsBlank())
                    //    colsResult += GetColumnProperty(theIndent, item, item.LookupDisplayColumn);
                }

                if (item.IsForeignKey && item.TableName == Input)
                    vmResult += theIndent + "public " + item.RelatedTable + "ViewModel " + item.RelatedTable + " { get; set; }".AddCarriage();
            }
            AppendText(colsResult.AddCarriage() + navResult + vmResult);
        }

        private static string GetColumnProperty(string theIndent, ISchemaItem item, string columnName = null)
        {
            if (columnName.IsBlank())
                return theIndent + "public " + item.ColumnType + " " + item.ColumnName + " { get; set; }".AddCarriage();
            else
                return theIndent + "public " + item.ColumnType + " " + columnName + " { get; set; }".AddCarriage();
        }

        private string GetCollectionNavigation(string theIndent, ISchemaItem item)
        {
            return theIndent + "public IList<" + Singularize(item.TableName) + "ViewModel> " + Pluralize(item.TableName) + " { get; set; }".AddCarriage();
        }
    }
}