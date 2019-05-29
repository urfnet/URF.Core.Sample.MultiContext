# URF.Core Multiple Contexts Sample

Demonstrates using URF.Core with multiple DbContexts.

1. Create new ASP.NET Core 2.2 Web API project.
   - Add .Api suffix to project name.

2. Add .NET Standard class library project.
   - Add .EF.Abstractions to the project name.
   - Add NuGet package: **URF.Core.Abstractions**
   - Add `IUnitOfWork<TDbContext>` interface.

    ```csharp
    public interface IUnitOfWork<TDbContext> : IUnitOfWork
    {
    }
    ```

    - Add `IRepository<TEntity, TDbContext>` interface.

    ```csharp
    public interface IRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class
    {
    }
    ```

3. Add another .NET Standard class library project with a .EF suffix.
   - Add NuGet package: **URF.Core.EF**
   - Add Nuget package: **Microsoft.EntityFrameworkCore**
   - Add `UnitOfWork<TDbContext>` class that extends `UnitOfWork` and implements `IUnitOfWork<TDbContext>` with `TDbContext` constrained to extend `DbContext`, and update the ctor to accept `TDbContext`.

    ```csharp
    public class UnitOfWork<TDbContext> : UnitOfWork, IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        public UnitOfWork(TDbContext context) : base(context)
        {
        }
    }
    ```

    - Add `Repository<TEntity, TDbContext>` class that extends `Repository<TEntity>` and implements `IRepository<TEntity, TDbContext>`.

    ```csharp
    public class Repository<TEntity, TDbContext> : Repository<TEntity>, IRepository<TEntity, TDbContext>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Repository(TDbContext context) : base(context)
        {
        }
    }
    ```

4. Add *two* .NET Core Class Library projects.
   - Add .EF.Products, .EF.Customers suffix to project names.
   - Reference the .Abstractions project.
   - Add NuGet package: **URF.Core.EF**
   - Add NuGet package: **Microsoft.EntityFrameworkCore.SqlServer**
   - Add NuGet package: **Microsoft.EntityFrameworkCore.Design**
   - Add Product class to .EF.Products project.

    ```csharp
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
    ```

   - Add Customer class to .EF.Customers project.

    ```csharp
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
    }
    ```

5. Add two DbContext classes to each of the .EF.* projects.
   - In .EF.Products project:

    ```csharp
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
    ```

   - In .EF.Customers project:

    ```csharp
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
    ```

6. Add classes to each .EF.* project which implement `IDesignTimeDbContextFactory`.
   - In .EF.Products project:

    ```csharp
    public class ProductsDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MsSqlLocalDb;initial catalog=ProductsDb;Integrated Security=True; MultipleActiveResultSets=True");
            return new ProductsDbContext(optionsBuilder.Options);
        }
    }
    ```

   - In .EF.Customers project:

    ```csharp
    public class CustomersDbContextFactory : IDesignTimeDbContextFactory<CustomersDbContext>
    {
        public CustomersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomersDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MsSqlLocalDb;initial catalog=CustomersDb;Integrated Security=True; MultipleActiveResultSets=True");
            return new CustomersDbContext(optionsBuilder.Options);
        }
    }
    ```

7. In each of the .EF.* projects add an EF migration and update the database.
   - Open a command prompt and cd into the .EF project.
   - This should create the databases and tables.
   - You can then add data to each table.

    ```
    dotnet ef migrations add initial
    dotnet ef database update
    ```

8. In each .EF.* project add an interface that extends `IUnitOfWork<TDbContext>` with properties for each related repository.
  
    ```csharp
    public interface ICustomersUnitOfWork : IUnitOfWork<CustomersDbContext>
    {
        IRepository<Customer, CustomersDbContext> CustomersRepository { get; }
    }
    ```

    ```csharp
    public interface IProductsUnitOfWork : IUnitOfWork<ProductsDbContext>
    {
        IRepository<Product, ProductsDbContext> ProductsRepository { get; }
    }
    ```

   - Add a classes that implement `ICustomersUnitOfWork` and `IProductsUnitOfWork`.

    ```csharp
    public class CustomersUnitOfWork : UnitOfWork<CustomersDbContext>, ICustomersUnitOfWork
    {
        public CustomersUnitOfWork(CustomersDbContext context, IRepository<Customer, CustomersDbContext> repository) : base(context)
        {
            CustomersRepository = repository;
        }

        public IRepository<Customer, CustomersDbContext> CustomersRepository { get; }
    }
    ```

    ```csharp
    public class ProductsUnitOfWork : UnitOfWork<ProductsDbContext>, IProductsUnitOfWork
    {
        public ProductsUnitOfWork(ProductsDbContext context, IRepository<Product, ProductsDbContext> repository) : base(context)
        {
            ProductsRepository = repository;
        }

        public IRepository<Product, ProductsDbContext> ProductsRepository { get; }
    }
    ```

9.  In Visual Studio open the **appsettings.json** file in the .Api project to add connection strings for each database.

    ```json
    "ConnectionStrings": {
    "CustomersDbContext": "Data Source=(localdb)\\MsSqlLocalDb;initial catalog=CustomersDb;Integrated Security=True; MultipleActiveResultSets=True",
    "ProductsDbContext": "Data Source=(localdb)\\MsSqlLocalDb;initial catalog=ProductsDb;Integrated Security=True; MultipleActiveResultSets=True"
    }
    ```

10. Update `ConfigureServices` in `Startup` in the .Api project to get each connection string from config.
    - Then register each `DbContext` and `IUnitOfWork`.

    ```csharp
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
    ```

11. Add `CustomersController` to the Controllers folder in the .Api project, selecting **API Controller with Actions using Entity Framework**.
    - Select `Customer` for the model class, `CustomersDbContext` for the data context class.
    - Update the ctor and backing field to use `ICustomersUnitOfWork`.
    - Update Get methods to use the Customers repository.

    ```csharp
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersUnitOfWork _unitOfWork;

        public CustomersController(ICustomersUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _unitOfWork.CustomersRepository.Query().SelectAsync();
            return Ok(customers);
        }
            
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _unitOfWork.CustomersRepository.FindAsync(id);
            if (customer == null)
                return NotFound();
            return customer;
        }
    ```

   - Update Put, Post and Delete methods to use Customers repository.

    ```csharp
    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, Customer customer)
    {
        if (id != customer.Id)
            return BadRequest();
        _unitOfWork.CustomersRepository.Update(customer);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CustomerExists(id))
                return NotFound();
            else
                throw;
        }
        return Ok(customer);
    }
            
    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        _unitOfWork.CustomersRepository.Insert(customer);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var result = await _unitOfWork.CustomersRepository.DeleteAsync(id);
        if (!result)
            return NotFound();
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> CustomerExists(int id)
    {
        return await _unitOfWork.CustomersRepository.ExistsAsync(id);
    }
    ```

   - Repeat for `ProductsController`.

12. Using Postman or another REST client, test the Post, Put and Delete methods of each controller.
    - Post: Raw JSON body
      - Expected: 201 with location header

    ```json
    {
        "customerName": "Google",
        "city": "Palo Alto"
    }
    ```

    - Put: Raw JSON body
      - Use id generated from Post
      - Include all fields
      - Update city
      - Expected: 200 with updated entity

    ```json
    {
        "id": 6,
        "customerName": "Google",
        "city": "San Jose"
    }
    ```

    - Delete: Entity id
    - Expected: 204 No Content
    - Execute Get with id, return 404 to verify deletion
