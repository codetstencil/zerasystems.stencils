using System.ComponentModel.DataAnnotations;

namespace ZeraSystems.CodeNanite.Boilerplate
{
    public class StencilDetail
    {

        [Key]
        public int StencilDetailID { get; set; }

        [MaxLength(100)]
        public string StencilName { get; set; }

        [MaxLength(100)]
        public string Creator { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Comments { get; set; }

        [MaxLength(100)]
        public string Version { get; set; }

        [MaxLength(1000)]
        public string FileName { get; set; }

        [MaxLength(1000)]
        public string OutputFolder { get; set; }

        [MaxLength(100)]
        public string StencilType { get; set; }

        public int StencilTypeID { get; set; }

        public bool IsLocked { get; set; }

        public bool NodeGenerationOnly { get; set; }
    }
}