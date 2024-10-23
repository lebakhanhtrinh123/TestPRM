using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Request
{
    public class ProductRequest
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }
        public string Image { get; set; }
    }
}
