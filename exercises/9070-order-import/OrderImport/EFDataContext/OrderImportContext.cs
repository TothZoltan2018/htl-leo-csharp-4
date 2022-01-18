using Microsoft.EntityFrameworkCore;
using OrderImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImport
{
    public class OrderImportContext : DbContext
    {
        public OrderImportContext(DbContextOptions<OrderImportContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
