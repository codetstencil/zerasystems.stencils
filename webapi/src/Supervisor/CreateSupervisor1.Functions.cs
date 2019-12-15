using System.Collections.Generic;
using System.Linq;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.WebAPI
{
    public partial class CreateSupervisor1
    {
        #region Fields
        private List<ISchemaItem> _tables;
        #endregion Fields

        private void MainFunction()
        {
            _tables = GetTables();
            var calculatedColumns = _tables.Where(t => t.IsCalculatedColumn).ToList();
            BuildSnippet(null);
            
            foreach (var item in _tables)
            {
                var table = item.TableName;
                BuildSnippet("private readonly I"+table+"Repository _"+table.ToLower()+"Repository;");
            }
            BuildSnippet("");
            BuildSnippet("public "+ GetProjectName() + "Supervisor()");
            BuildSnippet("{");
            BuildSnippet("}");
            BuildSnippet("");

            BuildSnippet("public "+ GetProjectName() + "Supervisor(");
            var x = 0;
            foreach (var item in _tables)
            {
                x++;
                var table = item.TableName;
                var comma = ",";
                if (x == _tables.Count) comma = string.Empty;
                BuildSnippet("I"+table+"Repository "+table.ToLower()+"Repository"+comma, 12);
            }
            BuildSnippet(")");
            BuildSnippet("{");
            foreach (var item in _tables)
            {
                var table = item.TableName;
                BuildSnippet("_"+table.ToLower()+"Repository = "+table.ToLower()+"Repository;", 12);
            }
            BuildSnippet("}");
            
            AppendText();
            AppendText(BuildSnippet());
        }
    }
}