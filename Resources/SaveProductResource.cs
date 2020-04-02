using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Resources
{
    public class SaveProductResource
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Please enter valid quantity")]
        public int QuantityInPackage { get; set; }

        [Required]
        public EUnitOfMeasurement UnitOfMeasurement { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}