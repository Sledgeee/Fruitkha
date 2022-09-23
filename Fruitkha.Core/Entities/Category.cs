using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fruitkha.Core.Entities
{
    public class Category : Base
    {
        public Category()
        {
            Products = new();
        }

        [Required]
        [DefaultValue(1)]
        public int ParentId { get; set; }

        [Required]
        [DefaultValue(0)]
        public int RealCategory { get; set; }

        [Required]
        [DefaultValue("")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}