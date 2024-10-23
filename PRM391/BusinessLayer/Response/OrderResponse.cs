using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Response
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        
        public List<OrderItermReponse> OrderItems { get; set; }
    }
}
