using System.ComponentModel.DataAnnotations;

namespace PoC.DoS.Web.Models
{
    public class DoSViewModel
    {
        [Required]
        [Range(1, 1000000)]
        public int NumberOfRound { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
