using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateEntities
    {
        private string _public = "public ";
        private string _getSet = " { get; set; }";
        private string _apiModel;
        private string _classname;
        List<ISchemaItem> _columns;
        private List<ISchemaItem> _relatedColumns;
        private List<ISchemaItem> _foreignKeys;


        private void MainFunction()
        {
            _columns = GetColumns(Input).Where(t=>t.IsCalculatedColumn==false).ToList();
            _relatedColumns = GetRelatedTables(Input);
            _foreignKeys = GetForeignKeysInTable(Input);
            _classname = Singularize(Input);
            _apiModel = _classname + "ApiModel";
            AppendText();
            BuildSnippet(null);
            BuildSnippet(_public + "class " + _classname + " : IConvertModel<"+ _classname+", "+ _apiModel+">", 4);
            BuildSnippet("{", 4);
            AppendText(GetColumns());
            AppendText(GetApiModel());
            BuildSnippet("}", 4);
            AppendText(BuildSnippet(),"");
        }


        private string GetColumns()
        {
            foreach (var item in _columns)
                BuildSnippet( GetAttributes(item)+_public + item.ColumnType + GetNullSign(item) + " " + item.ColumnName + _getSet);

            BuildSnippet("");
            foreach (var item in _relatedColumns)
                BuildSnippet(_public + "ICollection<" + item.TableName + "> " + Pluralize(item.TableName) + _getSet + " = new HashSet<" + item.TableName + ">();");

            foreach (var item in _foreignKeys)
            {
                if (item.RelatedTable == Input) //A table related to itself
                    BuildSnippet(_public + item.RelatedTable + " " + item.ColumnName + "Navigation" + _getSet);
                else
                    BuildSnippet(_public + item.RelatedTable + " " + item.RelatedTable + _getSet);
            }

            return BuildSnippet();
        }

        private string GetAttributes(ISchemaItem column)
        {
            return !column.ColumnAttribute.IsBlank() ? column.ColumnAttribute.AddCarriage()+Indent(8) : string.Empty;
        }

        private string GetApiModel()
        {
            BuildSnippet(_public + _apiModel + " Convert => new "+_apiModel);
            BuildSnippet("{");
            var comma = ",";
            var x = 0;
            foreach (var item in _columns)
            {
                x++;
                if (item.IsCalculatedColumn) continue;
                if (x == _columns.Count) comma = string.Empty;
                BuildSnippet(item.ColumnName + " = " + item.ColumnName + comma,12);
            }
            BuildSnippet("};",8,true);
            return BuildSnippet();
        }
    }
}