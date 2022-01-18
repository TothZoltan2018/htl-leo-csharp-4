using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using OrderImport.EFDataContext;
using OrderImport.Models;

namespace OrderImport
{
    public class Operations
    {
        OrderImportContextFactory factory = new OrderImportContextFactory();

        public async Task Import(string customersFile, string ordersFile)
        {
            try
            {                
                using var importContext = factory.CreateDbContext();

                string[][] customersData = ParseDataFromFile(await File.ReadAllLinesAsync(customersFile));
                string[][] ordersData = ParseDataFromFile(await File.ReadAllLinesAsync(ordersFile));
                 
                List<Customer> customers = new();
                foreach (var item in customersData)
                {
                    customers.Add(new Customer { Name = item[0], CreditLimit = int.Parse(item[1]) });
                }

                // Wtite customers to DB and later we can read the Id given by SQL Server to fill the FK in the Orcers table
                await importContext.AddRangeAsync(customers);
                await importContext.SaveChangesAsync();

                List<Order> orders = new();
                foreach (var item in ordersData)
                {
                    //int custId = customers.Where(cust => cust.Name == item[0]).First().Id;
                    int custIdfromDB = importContext.Customers.Where(c => c.Name == item[0]).First().Id;                    
                    orders.Add(new Order { CustomerId = custIdfromDB, OrderDate = DateTime.Parse(item[1]), OrderValue = decimal.Parse(item[2]) });
                }

                await importContext.Orders.AddRangeAsync(orders);
                await importContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Something went wrong. Command usage: dotnet run -- import customers.txt orders.txt" +
                    $"Exception details: {e.Message}");
            }
        }

        private string[][] ParseDataFromFile(string[] data)
        {
            string[][] elements = new string[data.Length-1][];

            for (int i = 1; i < data.Length; i++)
            {
                elements[i-1] = data[i].Split('\t');
            }

            return elements;
        }

        public async Task Clean()
        {
            using var importContext = factory.CreateDbContext();
            var orders = await importContext.Orders.ToListAsync();
            var customers = await importContext.Customers.ToListAsync();
            importContext.RemoveRange(orders);
            importContext.RemoveRange(customers);
            await importContext.SaveChangesAsync();
        }

        public async Task Check()
        {
            using var importContext = factory.CreateDbContext();

            await importContext.Customers
                .Where(c => c.CreditLimit < c.Orders.Select(o => o.OrderValue).Sum())
                .Select(c => new { c.Name, c.CreditLimit, sumOfOrders = c.Orders.Select(o => o.OrderValue).Sum() })
                .ForEachAsync(d => Console.WriteLine($"{d.Name} is over his {d.CreditLimit} limit by ordering a total of {d.sumOfOrders}"));

            // Same result, but diffucult
            //await importContext.Orders
            //    //.Include(o => o.Customer) // not needed
            //    .Select(o => new
            //    {
            //        o.Customer.Name,
            //        o.Customer.CreditLimit,
            //        sumOfOrdersPerCustomers = o.Customer.Orders
            //        .Sum(o => o.OrderValue)
            //    })
            //    .Distinct()
            //    .Where(x => x.CreditLimit < x.sumOfOrdersPerCustomers)
            //    .ForEachAsync(d => Console.WriteLine($"{d.Name} is over his {d.CreditLimit} limit by ordering a total of {d.sumOfOrdersPerCustomers}"));
        }

        public async Task Full(string customersFile, string ordersFile)
        {
            using var importContext = factory.CreateDbContext();

            await Clean();
            await Import(customersFile, ordersFile);
            await Check();
        }
    }
}