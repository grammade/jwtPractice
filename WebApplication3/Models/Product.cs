using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication3.Model
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Price { get; set; }
        public int? Qty { get; set; }
    }
}
