using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderImport.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Column(TypeName = "Datetime2")]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal OrderValue { get; set; }
    }
}
