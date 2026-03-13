using Klimaitis.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShoeStore.Repositories
{
    public class ProductRepository
    {
        private readonly ShoeStoreDbContext _context;

        public ProductRepository()
        {
            _context = new ShoeStoreDbContext();
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Manufacturer)
                .ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Manufacturer)
                .FirstOrDefault(p => p.ProductId == id);
        }

        public Product? GetProductByArticle(string article)
        {
            return _context.Products
                .FirstOrDefault(p => p.Article == article);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.OrderItems)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return false;

            // Проверяем, есть ли товар в заказах
            if (product.OrderItems != null && product.OrderItems.Any())
                return false;

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public List<Manufacturer> GetAllManufacturers()
        {
            return _context.Manufacturers.ToList();
        }
    }
}






//using Klimaitis.Models;
//using Microsoft.EntityFrameworkCore;

//namespace ShoeStore.Repositories
//{
//    public class ProductRepository
//    {
//        private readonly ShoeStoreDbContext _context;

//        public ProductRepository()
//        {
//            _context = new ShoeStoreDbContext();
//        }

//        public List<Product> GetAllProducts()
//        {
//            return _context.Products
//                .Include(p => p.Category)
//                .Include(p => p.Supplier)
//                .Include(p => p.Manufacturer)
//                .ToList();
//        }

//        public Product? GetProductById(int id)
//        {
//            return _context.Products
//                .Include(p => p.Category)
//                .Include(p => p.Supplier)
//                .Include(p => p.Manufacturer)
//                .FirstOrDefault(p => p.ProductId == id);
//        }

//        public Product? GetProductByArticle(string article)
//        {
//            return _context.Products
//                .FirstOrDefault(p => p.Article == article);
//        }

//        public void AddProduct(Product product)
//        {
//            _context.Products.Add(product);
//            _context.SaveChanges();
//        }

//        public void UpdateProduct(Product product)
//        {
//            _context.Products.Update(product);
//            _context.SaveChanges();
//        }

//        public bool DeleteProduct(int id)
//        {
//            var product = _context.Products
//                .Include(p => p.OrderItems)
//                .FirstOrDefault(p => p.ProductId == id);

//            if (product == null)
//                return false;

//            if (product.OrderItems.Any())
//                return false;

//            _context.Products.Remove(product);
//            _context.SaveChanges();
//            return true;
//        }

//        public List<Category> GetAllCategories()
//        {
//            return _context.Categories.ToList();
//        }

//        public List<Supplier> GetAllSuppliers()
//        {
//            return _context.Suppliers.ToList();
//        }

//        public List<Manufacturer> GetAllManufacturers()
//        {
//            return _context.Manufacturers.ToList();
//        }
//    }
//}