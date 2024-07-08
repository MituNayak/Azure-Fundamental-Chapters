using System.ComponentModel.DataAnnotations;

namespace AzureDemoBlob.Models
{
    public class Container
    {
        [Required]
        public string Name { get; set; }
    }
}
