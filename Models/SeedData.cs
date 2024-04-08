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
                    new Customer { Name = "harry", Address = "Trelew 14" },
                    new Customer { Name = "bob", Address = "Trelew 10" },
                    new Customer { Name = "lee", Address = "something" }
                };

                var products = new List<Product>
                {
                    new Product { Name = "coil", Description = "a coil", Price = 2.10M },
                    new Product { Name = "bread", Description = "wholewheat bread", Price = 1.10M },
                    new Product { Name = "rotisserie chicken", Description = "chicken roasted healthily", Price = 6.00M }
                };

                context.Customer.AddRange(customers);
                context.SaveChanges(); // Save customers to generate identity values

                context.Product.AddRange(products);
                context.SaveChanges(); // Save products to generate identity values

                var users = new List<User>
                {
                    new User { Username = "admin", Password = "password", UserType = 2 },
                    new User { Username = "user", Password = "password", UserType = 1 }
                };

                context.Users.AddRange(users);
                context.SaveChanges();

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
