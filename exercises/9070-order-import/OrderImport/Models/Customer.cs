using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderImport.Models
{
    public class Customer
    {
        public int Id { get; set; }
       
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "decimal(8,2)")]
        public decimal CreditLimit { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}
