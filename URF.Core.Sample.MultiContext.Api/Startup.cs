using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using URF.Core.Sample.MultiContext.EF.Customers;
using URF.Core.Sample.MultiContext.EF.Products;
using URF.Core.Sample.MultiContext.Abstractions;

namespace URF.Core.Sample.MultiContext.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var customersConnString = Configuration.GetConnectionString(nameof(CustomersDbContext));
            services.AddDbContext<CustomersDbContext>(options => options.UseSqlServer(customersConnString));
            services.AddScoped<ICustomersUnitOfWork, CustomersUnitOfWork>();
            services.AddScoped<IRepository<Customer, CustomersDbContext>, Repository<Customer, CustomersDbContext>>();

            var productsConnString = Configuration.GetConnectionString(nameof(ProductsDbContext));
            services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(productsConnString));
            services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();
            services.AddScoped<IRepository<Product, ProductsDbContext>, Repository<Product, ProductsDbContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
