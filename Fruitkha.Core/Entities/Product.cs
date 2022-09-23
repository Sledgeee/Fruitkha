using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Fruitkha.Core.Entities
{
    public class Product : Base
    {
        public Product()
        {
            Category = new();
        }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Articul { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        public string Vendor { get; set; }

        [AllowNull]
        public string BriefDescription { get; set; }

        [Required]
        public uint PriceUah { get; set; }

        [Required]
        public uint PriceUsd { get; set; }

        [Required]
        public string SmallImage { get; set; }

        [Required]
        public string MediumImage { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}