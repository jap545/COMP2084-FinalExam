using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DotNetDrinksTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ProductsController _controller;//null
        private ApplicationDbContext _context;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // mock some data (1 restaurant and 3 products)
            var brand = new Brand { Id = 1, Name = "Mocktails" };
            var product1 = new Product { Id = 1, Name = "Coke", Price = 10, Brand = brand };
            var product2 = new Product { Id = 2, Name = "Virgin Mojito", Price = 8, Brand = brand };
            var product3 = new Product { Id = 3, Name = "Cranberry Splash", Price = 20, Brand = brand };

            _context.Brands.Add(brand);
            _context.SaveChanges();
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.SaveChanges();
            // add products to local list to compare when making modifications
            _products = new List<Product>();
            _products.Add(product1);
            _products.Add(product2);
            _products.Add(product3);
            // initialize controller object
            _controller = new ProductsController(_context);

            
        }
        //For Delete Get method
        [TestMethod]
        public void DeleteReturnsDeleteView()
        {
            // Arrange
            var controller = new ProductsController(_context);

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsNotNull(result);
        }

        //For DeleteConfirmed Post method
        [TestMethod]
        public void DeleteConfirmedRemovesProductFromDB()
        {
            // Arrange: Get a product object from the in-memory database
            var productToRemove = _context.Products.FirstOrDefault();

            // Act: Call DeleteConfirmed with the product's ID
            _controller.DeleteConfirmed(productToRemove.Id);

            // Assert: Check if the product is removed from the database
            Assert.IsNull(_context.Products.Find(productToRemove.Id));
        }
    }
}
