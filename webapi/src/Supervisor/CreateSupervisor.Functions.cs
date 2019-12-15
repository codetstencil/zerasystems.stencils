using System.Collections.Generic;
using System.Linq;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateSupervisor
    {
        #region Fields

        private string _table;
        private List<ISchemaItem> _columns;
        private ISchemaItem _tableObject;

        //private readonly StringBuilder _snippet = new StringBuilder();
        private string _tableVar;

        #endregion Fields

        private void MainFunction()
        {
            _table = GetTable(Input);
            _tableObject = GetTableObject(Input);
            //_columns = GetColumnsExCalculated(Input);
            _columns = GetColumns(Input);
            _tableVar = _table.ToLower();
            if (_tableObject == null)
                return;

            AppendText();
            AppendText(GetAllAsync());
            AppendText(GetByIdAsync());
            var foreignKeys = GetForeignKeysInTable(_table);
            if (foreignKeys.Any())
                AppendText(GetByForeignKeyIdAsync(foreignKeys));
            AppendText(GetByIdRelatedAsync());
            AppendText(AddAsync());
            AppendText(UpdateAsync());
            AppendText(DeleteAsync());
        }

        // public async Task<IEnumerable<TrackApiModel>> GetAllTrackAsync(CancellationToken ct = default)
        private string GetAllAsync(int indent = 8)
        {
            BuildSnippet(null);
            BuildSnippet("public async Task<IEnumerable<" + _table + "ApiModel>> GetAll" + _table + "Async(CancellationToken ct = default)");
            BuildSnippet("{");
            BuildSnippet("var " + _tableVar.Pluralize() + " = await _" + _tableVar + "Repository.GetAllAsync(ct);", indent + 4);
            BuildSnippet("return " + _tableVar.Pluralize() + ".ConvertAll();", indent + 4);
            BuildSnippet("}");
            return BuildSnippet();
        }

        // public async Task<TrackApiModel> GetTrackByIdAsync(int id, CancellationToken ct = default)
        private string GetByIdAsync(int indent = 8)
        {
            BuildSnippet(null);
            BuildSnippet("public async Task<" + _table + "ApiModel> Get" + _table + "ByIdAsync(int id, CancellationToken ct = default)");
            BuildSnippet("{");
            BuildSnippet("var " + _tableVar + "ApiModel = (await _" + _tableVar + "Repository.GetByIdAsync(id, ct)).Convert;", indent + 4);
            foreach (var column in _columns)
            {
                var valueOrDefault = !column.AllowDbNull ? string.Empty : ".GetValueOrDefault()";
                if (column.IsForeignKey)
                    BuildSnippet(_tableVar + "ApiModel." + column.RelatedTable + " = await Get" + column.RelatedTable +
                                 "ByIdAsync(" + _tableVar + "ApiModel." +
                                 column.ColumnName + valueOrDefault + ", ct);", indent + 4);
                                //GetPrimaryKey(column.RelatedTable) + valueOrDefault + ", ct);", indent + 4);
                else if (column.IsCalculatedColumn)
                    BuildSnippet(_tableVar + "ApiModel." + column.ColumnName + " = " + _tableVar + "ApiModel." +
                                 column.CalculatedColumn.Replace("o.", string.Empty) + ";", indent + 4);
            }
            BuildSnippet("return " + _tableVar + "ApiModel;", indent + 4);
            BuildSnippet("}", indent);
            return BuildSnippet();
        }

        // public async Task<IEnumerable<TrackApiModel>> GetTrackByAlbumIdAsync(int id,CancellationToken ct = default)
        private string GetByForeignKeyIdAsync(IEnumerable<ISchemaItem> foreignKeys, int indent = 8)
        {
            BuildSnippet(null);
            foreach (var key in foreignKeys)
            {
                BuildSnippet("public async Task<IEnumerable<" + _table + "ApiModel>> Get" + _table + "By" + key.ColumnName + "Async(int id, CancellationToken ct = default)");
                BuildSnippet("{");
                BuildSnippet(
                    "var " + _tableVar.Pluralize() + " = await _" + _tableVar +"Repository.GetBy"+key.ColumnName + "Async(id, ct);", indent + 4);
                BuildSnippet("return " + _tableVar.Pluralize() + ".ConvertAll();", indent + 4);
                BuildSnippet("}");
            }
            return BuildSnippet();
        }


        
        private string GetByIdRelatedAsync(int indent = 8)
        {
            BuildSnippet(null);
            foreach (var column in _columns)
            {
                if (column.IsCalculatedColumn)
                {
                    var calcColumn = column.CalculatedColumn.Replace("o.", string.Empty);
                    var calcTable = calcColumn.Substring(0, calcColumn.IndexOf("."));
                    BuildSnippet("public async Task<IEnumerable<" + _table + "ApiModel>> Get" + _table + "ById" + calcTable + "IdAsync(int id, CancellationToken ct = default)");
                    BuildSnippet("{");
                    BuildSnippet("var " + _tableVar.Pluralize() + " = await _" + _tableVar + "Repository.GetBy" + calcTable + "IdAsync(id, ct);", indent + 4);
                    BuildSnippet("return " + _tableVar.Pluralize() + ".ConvertAll();", indent + 4);
                    BuildSnippet("}");
                }
            }
            return BuildSnippet();
        }

        private string AddAsync(int indent = 8)
        {
            BuildSnippet(null);
            BuildSnippet("public async Task<" + _table + "ApiModel> Add" + _table + "Async(" + _table + "ApiModel new" + _table + "ViewModel, CancellationToken ct = default)");
            BuildSnippet("{");
            BuildSnippet("var " + _tableVar + " = new " + _table, indent + 4);
            BuildSnippet("{", indent + 4);
            foreach (var column in _columns)
            {
                if (!column.IsCalculatedColumn)
                {
                    BuildSnippet(column.ColumnName + " = new" + _table + "ViewModel." + column.ColumnName + ",", indent + 8);
                }
            }
            BuildSnippet("};", indent + 4);
            BuildSnippet(_tableVar + " = await _" + _tableVar + "Repository.AddAsync(" + _tableVar + ", ct);", indent + 4);
            BuildSnippet("new" + _table + "ViewModel." + GetPrimaryKey(_table)+" = " + _tableVar + "." + GetPrimaryKey(_table) + ";", indent + 4);
            BuildSnippet("return new" + _table + "ViewModel;", indent + 4);
            BuildSnippet("}", indent);
            return BuildSnippet();
        }

        private string UpdateAsync(int indent = 8)
        {
            BuildSnippet(null);
            BuildSnippet("public async Task<bool> Update" + _table + "Async(" + _table + "ApiModel " + _tableVar + "ViewModel, CancellationToken ct = default)");
            BuildSnippet("{");
            BuildSnippet("var " + _tableVar + " = await " + "_" + _tableVar + "Repository.GetByIdAsync(" + _tableVar + "ViewModel." + GetPrimaryKey(_table) + ", ct);", indent + 4);
            BuildSnippet("", indent + 4);
            BuildSnippet("if (" + _tableVar + " == null) return false;", indent + 4);
            foreach (var column in _columns)
            {
                if (!column.IsCalculatedColumn)
                {
                    BuildSnippet(_tableVar + "." + column.ColumnName + " = " + _tableVar + "ViewModel." + column.ColumnName + ";", indent + 4);
                }
            }
            BuildSnippet("", indent + 4);
            BuildSnippet("return await _" + _tableVar + "Repository.UpdateAsync(" + _tableVar + ", ct);", indent + 4);
            BuildSnippet("}");
            return BuildSnippet();
        }

        private string DeleteAsync(int indent = 8)
        {
            BuildSnippet(null);
            BuildSnippet("public async Task<bool> Delete" + _table + "Async(int id, CancellationToken ct = default)");
            BuildSnippet("{");
            BuildSnippet("return await _" + _tableVar + "Repository.DeleteAsync(id, ct);", indent + 4);
            BuildSnippet("}");
            return BuildSnippet();
        }
    }
}