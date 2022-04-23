using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuya.PruebaTecnica.Models.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime ExtimatedShipDate { get; set; }
        public int OrderId { get; set; }
    }
}
