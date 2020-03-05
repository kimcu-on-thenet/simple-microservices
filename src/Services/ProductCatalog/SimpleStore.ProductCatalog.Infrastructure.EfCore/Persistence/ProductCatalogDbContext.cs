﻿using Microsoft.EntityFrameworkCore;
using SimpleStore.Infrastructure.EfCore;
using System.Reflection;

namespace SimpleStore.ProductCatalog.Infrastructure.EfCore.Persistence
{
    public class ProductCatalogDbContext : ApplicationDbContextBase
    {
        public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> dbContextOptions) : base(dbContextOptions) { }

        protected override Assembly CurrentAssembly => Assembly.GetExecutingAssembly();
    }
}