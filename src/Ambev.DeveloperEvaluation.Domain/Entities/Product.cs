using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get; private set; }

        // For EF
        protected Product() { }

        public Product(string name, string sku, decimal price)
        {
            Name = name;
            Sku = sku;
            Price = price;
        }
    }
}
