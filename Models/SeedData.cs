using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Jeremiah_SupermarketOnline.Data;
using System;
using System.Linq;

namespace Jeremiah_SupermarketOnline.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Jeremiah_SupermarketOnlineContext(serviceProvider.GetRequiredService<DbContextOptions<Jeremiah_SupermarketOnlineContext>>()))
            {
                if (context.Customer.Any() || context.Product.Any() || context.Order.Any())
                {
                    return;
                }

                var customers = new List<Customer>
                {
                    new Customer { Name = "harry", Password = "password", Address = "Trelew 14", UserType = 0 },
                    new Customer { Name = "bob", Password = "password", Address = "Trelew 10", UserType = 0 },
                    new Customer { Name = "admin", Password = "password", Address = "something", UserType = 1 }
                };

                var products = new List<Product>
                {
                    new Product { Name = "chicken breast", Description = "chicken breast", Price = 2.10M },
                    new Product { Name = "rice", Description = "jasmine rice", Price = 1.10M },
                    new Product { Name = "pork", Description = "pig meat", Price = 3.10M },
                    new Product { Name = "bread", Description = "wholewheat bread", Price = 1.10M },
                    new Product { Name = "salt", Description = "table salt", Price = 0.50M },
                    new Product { Name = "pepper", Description = "black pepper", Price = 0.50M },
                    new Product { Name = "sugar", Description = "sugar", Price = 0.99M },
                    new Product { Name = "soy sauce", Description = "lee kum kee soy sauce", Price = 2.10M },
                    new Product { Name = "chicken", Description = "chicken on the bone", Price = 6.00M }
                };

                context.Customer.AddRange(customers);
                context.SaveChanges(); // Save customers to generate identity values

                context.Product.AddRange(products);
                context.SaveChanges(); // Save products to generate identity values

                var orders = new List<Order>
                {
                    new Order { OrderDate = new DateTime(2024, 12, 25, 10, 30, 50), Quantity = 10, CustomerId = customers[0].Id, ProductId = products[0].Id },
                    new Order { OrderDate = new DateTime(2024, 10, 15, 2, 10, 20), Quantity = 5, CustomerId = customers[1].Id, ProductId = products[0].Id },
                    new Order { OrderDate = new DateTime(2024, 12, 25, 10, 30, 50), Quantity = 21, CustomerId = customers[0].Id, ProductId = products[2].Id },
                    new Order { OrderDate = new DateTime(2024, 1, 1, 12, 30, 50), Quantity = 10, CustomerId = customers[1].Id, ProductId = products[1].Id }
                };

                context.Order.AddRange(orders);
                context.SaveChanges();
            }
        }
    }
}
