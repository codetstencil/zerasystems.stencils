using System.Data.Entity;
using System.IO;
using System.Linq;
using ZeraSystems.CodeNanite.Expansion;

namespace ZeraSystems.CodeNanite.Boilerplate
{
    public partial class ResetStencil : ExpansionBase
    {
        private void MainFunction()
        {
            string description = GetExpansionString("TARGET_STENCIL_DESCR");
            string stencil = GetExpansionString("PROJECT_NAME");
            string outputFolder = GetExpansionString("OUTPUT_FOLDER").Replace("[%PROJECT_NAME%]", stencil);
            string stencilFile = Path.Combine(outputFolder, stencil + ".codestencil");

            string dataSource = "Data Source=" + stencilFile;
            using (StencilContext context = new StencilContext(dataSource))
            {
                StencilDetail result = context.StencilDetails.First();
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
