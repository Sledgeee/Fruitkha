using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fruitkha.Core.Entities
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
    }
}