using System.Collections.Generic;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateApiModels
    {
        private string _public = "public ";
        private string _getSet = " { get; set; }";
        private string _classname;
        private List<ISchemaItem> _columns;
        private List<ISchemaItem> _relatedColumns;
        private List<ISchemaItem> _foreignKeys;

        private void MainFunction()
        {
            _columns = GetColumns(Input);
            _relatedColumns = GetRelatedTables(Input);
            _foreignKeys = GetForeignKeysInTable(Input);
            _classname = Singularize(Input) + "ApiModel ";
            AppendText();
            BuildSnippet(null);
            //BuildSnippet(_public + "sealed class " + _classname , 4);
            BuildSnippet(_public + "class " + _classname, 4);
            BuildSnippet("{", 4);
            AppendText(GetColumns(), string.Empty); // This is not allow line feed
            BuildSnippet("}", 4);
            AppendText(BuildSnippet(), string.Empty);
        }

        private string GetColumns()
        {
            foreach (var item in _columns)
                BuildSnippet(_public + item.ColumnType + GetNullSign(item) + " " + item.ColumnName + _getSet);

            BuildSnippet("");
            foreach (var item in _relatedColumns)
                BuildSnippet(_public + "IList<" + item.TableName + "ApiModel> " + Pluralize(item.TableName) + _getSet);
            foreach (var item in _foreignKeys)
                BuildSnippet(_public + item.RelatedTable + "ApiModel " + item.RelatedTable + _getSet);

            return BuildSnippet();
        }
    }
}