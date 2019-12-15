using System.Collections.Generic;
using System.Windows.Forms;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;
using static ZeraSystems.CodeNanite.Expansion.General;
  
namespace ZeraSystems.CodeNanite.WebAPI
{
    public partial class CreateController
    {
        #region Fields

        private string _table;
        private List<ISchemaItem> _columns;
        private ISchemaItem _tableObject;

        #endregion Fields

        private void MainFunction()
        {
            _table = GetTable(Input);
            _tableObject = GetTableObject(Input);
            _columns = GetColumnsExCalculated(Input);
            AppendText();
            if (_tableObject != null)
            {
                foreach (var column in _columns)
                    if (column.IsForeignKey)
                        AppendText(GetHttpGet(column, 8));
            }
        }

        private string GetHttpGet(ISchemaItem column, int indent)
        {
            var relatedTable = column.RelatedTable;
            var tableVar = relatedTable.ToLower();
            var table = (tableVar + "/{id}").AddQuotes();

            var result = string.Empty;
            result += Indent(indent) + "[HttpGet("+table+")]".AddCarriage();
            result += Indent(indent) + "[Produces(typeof(List<"+ column.TableName + "ApiModel>))]".AddCarriage();
            result += Indent(indent) + "public async Task<ActionResult<"+ column.TableName + "ApiModel>> GetBy"+ relatedTable + "Id(int id, CancellationToken ct = default)".AddCarriage();
            result += Indent(indent) + "{".AddCarriage();
            result += Indent(indent + 4) + "try".AddCarriage();
            result += Indent(indent + 4) + "{".AddCarriage();
            result += Indent(indent + 8) +      "var "+ tableVar + " = await _"+ GetProjectNameLower()+"Supervisor.Get"+ relatedTable+ "ByIdAsync(id, ct);".AddCarriage();
            result += Indent(indent + 8) +      "if ("+ tableVar + " == null)".AddCarriage();
            result += Indent(indent + 8) +      "{".AddCarriage();
            result += Indent(indent + 12)+          "return NotFound();".AddCarriage();
            result += Indent(indent + 8) +      "}".AddCarriage();
            result += Indent(indent + 8) +      "return Ok("+ tableVar + ");".AddCarriage();
            result += Indent(indent + 4) + "}".AddCarriage();
            result += Indent(indent + 4) + "catch (Exception ex)".AddCarriage();
            result += Indent(indent + 4) + "{".AddCarriage();
            result += Indent(indent + 8) +      "return StatusCode(500, ex);".AddCarriage();
            result += Indent(indent + 4) + "}".AddCarriage();
            result += Indent(indent) + "}".AddCarriage();
            return result;
        }

    }
}
