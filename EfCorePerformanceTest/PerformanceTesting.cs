using AutoMapper;
using AutoMapper.QueryableExtensions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using EfCorePerformanceTest.Context;
using EfCorePerformanceTest.Dtos;
using EfCorePerformanceTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCorePerformanceTest
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class PerformanceTesting
    {
        private readonly IMapper _mapper;

        public PerformanceTesting()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Category, CategoryDto>().ForMember(p => p.Products, opt => opt.MapFrom(x => x.Products.Where(x => x.Name.Contains("Product name"))));
                config.Advanced.BeforeSeal(provider => provider.CompileMappings());
            }).CreateMapper();
        }

        [Benchmark]
        public void AutoMapperProjection()
        {
            var sampleDbContext = new SampleDbContext();
            var categories = sampleDbContext.Categories.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToList();
        }

        [Benchmark]
        public void IncludeEFCore()
        {
            var sampleDbContext = new SampleDbContext();
            var categories = sampleDbContext
                                    .Categories
                                    .AsNoTracking()
                                    .Include(i => i.Products.Where(x => x.Name.Contains("Product name")))
                                    .ToList();
        }

        [Benchmark]
        public void NonFilteredInclude()
        {
            var sampleDbContext = new SampleDbContext();
            var category = sampleDbContext.Categories.AsNoTracking().Include(i => i.Products).ToList();

            category.ForEach(x => x.Products = x.Products.Where(a=>a.Name.Contains("Product name")).ToList());
        }

        [Benchmark]
        public void ProjectionManually()
        {
            var sampleDbContext = new SampleDbContext();
            var categories = sampleDbContext
                                .Categories
                                .AsNoTracking()
                                .Select(s => new
                                {
                                    s.CategoryId,
                                    s.CategoryName,
                                    Products = s.Products.Where(x => x.Name.Contains("Product name")).ToList()
                                }).ToList();
        }
    }
}
