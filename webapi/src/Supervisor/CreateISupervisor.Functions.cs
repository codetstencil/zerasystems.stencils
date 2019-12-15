using System.Collections.Generic;
using System.Linq;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateISupervisor
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
                var foreignKeys = GetForeignKeysInTable(table);
                BuildSnippet(" #region " + table);
                BuildSnippet("Task<IEnumerable<" + table + "ApiModel>> GetAll" + table + "Async(CancellationToken ct = default);");
                BuildSnippet("Task<" + table + "ApiModel> Get" + table + "ByIdAsync(int id, CancellationToken ct = default);");
                if (foreignKeys != null && foreignKeys.Any())
                {
                    foreach (var foreignKey in foreignKeys)
                        BuildSnippet("Task<IEnumerable<" + table + "ApiModel>> Get" + table + "By" + foreignKey.ColumnName + "Async(int id, CancellationToken ct = default);");
                }
                BuildSnippet("Task<" + table + "ApiModel> Add" + table + "Async(" + table + "ApiModel new" + table + "ViewModel, CancellationToken ct = default);");
                BuildSnippet("Task<bool> Update" + table + "Async(" + table + "ApiModel " + table.ToLower() + "ViewModel, CancellationToken ct = default);");
                BuildSnippet("Task<bool> Delete" + table + "Async(int id, CancellationToken ct = default);");

                if (calculatedColumns.Any())
                {
                    foreach (var column in calculatedColumns)
                    {
                        BuildSnippet("Task<IEnumerable<" + table + "ApiModel>> Get" + table + "By" + column.ColumnName + "IdAsync(int id, CancellationToken ct = default);");
                    }
                }
                BuildSnippet(" #endregion ");
                BuildSnippet("");

            }
            AppendText();
            AppendText(BuildSnippet());
        }
    }
}