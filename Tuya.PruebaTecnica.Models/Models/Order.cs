using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuya.PruebaTecnica.Models.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int? DeliveryId { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
