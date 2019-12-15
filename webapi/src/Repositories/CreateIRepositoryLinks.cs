using System.Collections.Generic;
using System.ComponentModel.Composition;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;
 
namespace ZeraSystems.CodeNanite.WebAPI
{
    /// <summary>
    /// There are 10 elements in thHhe String Array used by the
    /// 0  - This is the name of the publisher
    /// 1  - This is the title of the Code Nanite
    /// 2  - This is the description
    /// 3  - Version Number
    /// 4  - Label of the Code Nanite
    /// 5  - Namespace
    /// 6  - Release Date
    /// 7  - Name to use for Expander Label
    /// 9  - RESERVED
    /// 10 - RESERVED
    /// </summary>
    [Export(typeof(ICodeStencilCodeNanite))]
    [CodeStencilCodeNanite(new[]
    {
        "ZERA Systems Inc.",
        "Repository Interface Links Creator",
        "Creates Code Nanites for Repository Interface links",
        "1.0",
        "CreateIRepositoryLinks",
        "ZeraSystems.CodeNanite.WebAPI",
        "10/04/2019",
        "CREATE_IREPOSITORY_LINKS",
        "1",
        "",
        ""
    })]
    public partial class CreateIRepositoryLinks: ExpansionBase, ICodeStencilCodeNanite
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public int Counter { get; set; }
        public List<string> OutputList { get; set; }
        public List<ISchemaItem> SchemaItem { get; set; }
        public List<IExpander> Expander { get; set; }
        public List<string> InputList { get; set; }

        public void ExecutePlugin()
        {
            Initializer(SchemaItem, Expander);
            MainFunction();
            Output = ExpandedText.ToString();
        }
    }
}
