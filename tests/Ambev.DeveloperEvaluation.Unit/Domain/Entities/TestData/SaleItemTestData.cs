using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public class SaleItemTestData
    {
        private static readonly Faker _faker = new Faker();
        
        public static SaleItem GetValidSaleItem(decimal? unitPrice = null, int? quantity = null)
        {
            return new SaleItem(
                Guid.NewGuid(),
                unitPrice ?? _faker.Random.Decimal(1, 1000),
                quantity ?? _faker.Random.Int(1, 3) // Default to below discount threshold
            );
        }
        
        public static SaleItem GetSaleItemWithDiscount10Percent()
        {
            return new SaleItem(
                Guid.NewGuid(),
                _faker.Random.Decimal(10, 1000),
                _faker.Random.Int(4, 9) // 4-9 items gets 10% discount
            );
        }
        
        public static SaleItem GetSaleItemWithDiscount20Percent()
        {
            return new SaleItem(
                Guid.NewGuid(),
                _faker.Random.Decimal(10, 1000),
                _faker.Random.Int(10, 20) // 10-20 items gets 20% discount
            );
        }
        
        public static SaleItem GetCancelledSaleItem()
        {
            var saleItem = GetValidSaleItem();
            saleItem.Cancel();
            return saleItem;
        }
        
        public static List<SaleItem> GetMultipleSaleItems(int count = 3)
        {
            var items = new List<SaleItem>();
            
            for (int i = 0; i < count; i++)
            {
                items.Add(GetValidSaleItem());
            }
            
            return items;
        }
        
        public static List<SaleItem> GetItemsMix()
        {
            return new List<SaleItem>
            {
                GetValidSaleItem(),                  // No discount
                GetSaleItemWithDiscount10Percent(),  // 10% discount
                GetSaleItemWithDiscount20Percent()   // 20% discount
            };
        }
    }
}
