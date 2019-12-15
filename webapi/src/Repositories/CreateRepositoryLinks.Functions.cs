using System.Collections.Generic;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateRepositoryLinks
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
                foreach (var column in _columns)
                    if (column.IsForeignKey)
                        AppendText(GetAsyncById(column, 8));
            }
        }

        private string GetAsyncById(ISchemaItem column, int indent)
        {
            var result = Indent(indent) + "public async Task<List<" + column.TableName + ">> GetBy" + column.ColumnName +
                         "Async(int id, CancellationToken ct = default)".AddCarriage();
            result += Indent(indent) + "{".AddCarriage();
            result += Indent(indent + 4) + "return await _context."+ column.TableName+".Where(a => a." + column.ColumnName + " == id).ToListAsync(ct);".AddCarriage();
            result += Indent(indent) + "}".AddCarriage();
            return result;
        }
    }
}

/*
public async Task<List<Track>> GetByAlbumIdAsync(int id, CancellationToken ct = default(CancellationToken))
{
    return await _context.Track.Where(a => a.AlbumId == id).ToListAsync(ct);
}
*/