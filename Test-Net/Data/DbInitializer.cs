﻿using Test_Net.Models;

namespace Test_Net.Data
{    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
                var random = new Random();
                if (!context.ProductTypes.Any())
                {
                    var productTypes = new List<ProductType>();
                    for (int i = 1; i <= 20; i++)
                    {
                        productTypes.Add(new ProductType
                        {
                            Ten = "Loại " + i,
                            NgayNhap = DateTime.Now.AddDays(-random.Next(1, 365))
                        });
                    }
                    context.ProductTypes.AddRange(productTypes);
                    context.SaveChanges();
                }
                var productTypesList = context.ProductTypes.ToList();
                if (!context.Products.Any())
                {
                    var products = new List<Product>();

                    for (int i = 1; i <= 10000; i++)
                    {
                        var product = new Product
                        {
                            Ten = "Sản phẩm " + i,
                            Gia = random.Next(1000, 50000),
                            NgayNhap = DateTime.Now.AddDays(-random.Next(1, 365)),
                            ProductTypeId = productTypesList[random.Next(productTypesList.Count)].Id 
                        };
                        products.Add(product);
                    }
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
            }
        }



    }
}
