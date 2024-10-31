using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Product : BaseEntity
    {
        

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string SKU { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = new Category();

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class Category : BaseEntity
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; } = new Category();

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class ProductImage : BaseEntity
    {

        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();
    }

    public class ProductVariant:BaseEntity
    {

        public string VariantName { get; set; }
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();
    }
}
