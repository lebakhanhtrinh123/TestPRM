using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Response
{
    public  class CartResponse
    {
        public int CartId { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; } 

        public int? UserId { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public bool? Status { get; set; }
        public string? Image { get; set; }
    }
}
