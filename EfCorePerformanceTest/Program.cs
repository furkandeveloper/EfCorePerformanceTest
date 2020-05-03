using BenchmarkDotNet.Running;
using EfCorePerformanceTest.Context;
using EfCorePerformanceTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EfCorePerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //InitialDb();
            var performance = new PerformanceTesting();
            performance.AutoMapperProjection();
            performance.IncludeEFCore();
            performance.NonFilteredInclude();
            performance.ProjectionManually();

            BenchmarkRunner.Run<PerformanceTesting>();
            Console.WriteLine("Hello World!");
        }

        static void InitialDb()
        {
            var sampleDbContext = new SampleDbContext();
            List<Category> categories = new List<Category>();
            List<Product> products = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                var category = new Category
                {
                    CategoryName = "Category " + i,
                };


                categories.Add(category);
            }
            sampleDbContext.Categories.AddRange(categories);
            sampleDbContext.SaveChanges();

            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    Name = "Product name" + i,
                    Description = "Product Description" + i,
                    Content = "Product Description" + i,
                    CategoryId = categories.OrderByDescending(x=>x.CreateDate).FirstOrDefault().CategoryId
                };

                products.Add(product);
            }

            sampleDbContext.Products.AddRange(products);
            sampleDbContext.SaveChanges();
        }
    }
}
