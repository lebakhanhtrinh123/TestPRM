using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Request
{
    public class OrderItemRequest
    {
        public int? CartId { get; set; }
        public int? OrderId { get; set; }
    }
}
