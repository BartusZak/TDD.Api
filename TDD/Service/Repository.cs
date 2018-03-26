using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TDD.Interface;

namespace TDD.Service
{
    public class Repository<T>:IRepository<T>where T : Entity
    {
        private ApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public bool Exist(Func<T, bool> function)
        {
            return _dbSet.Any(function);
        }

        public T GetBy(Func<T, bool> function)
        {
            return _dbSet.FirstOrDefault(function);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

      
    }
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
    public class ApplicationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "../../../../TDD.WebApi");
            var builder = new ConfigurationBuilder().SetBasePath(path)
                .AddJsonFile("appsettings.json", false, true);
            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                throw new Exception(path);
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
