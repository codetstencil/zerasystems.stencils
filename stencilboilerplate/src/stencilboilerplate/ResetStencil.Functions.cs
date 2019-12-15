using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.Boilerplate
{
    public partial class ResetStencil : ExpansionBase
    {
        private void MainFunction()
        {
            var description = GetExpansionString("TARGET_STENCIL_DESCR");
            var stencil = GetExpansionString("PROJECT_NAME");
            var outputFolder = GetExpansionString("OUTPUT_FOLDER").Replace("[%PROJECT_NAME%]", stencil);
            var stencilFile = Path.Combine(outputFolder, stencil+".codestencil");

            var dataSource = "Data Source=" + stencilFile;
            using (var context = new StencilContext(dataSource))
            {
                var result = context.StencilDetails.First();
                if (result != null)
                {
                    result.StencilName = stencil;
                    result.Description = description;
                    result.OutputFolder = outputFolder;
                    result.FileName = stencilFile;
                    result.StencilType = string.Empty;
                    result.StencilTypeID = 0;
                    context.SaveChanges();
                }
            }

        }







    }

    public class StencilContext : DbContext
    {
        public StencilContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<StencilDetail> StencilDetails { get; set; }
    }
}
