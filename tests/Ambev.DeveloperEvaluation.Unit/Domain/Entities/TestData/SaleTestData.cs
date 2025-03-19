using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public class SaleTestData
    {
        private static readonly Faker _faker = new Faker();
        
        public static Sale GetValidSale()
        {
            return new Sale(
                Guid.NewGuid(),
                Guid.NewGuid(),
                _faker.Date.Recent()
            );
        }
        
        public static Sale GetValidSaleWithItems(int itemCount = 3)
        {
            var sale = GetValidSale();
            
            for (int i = 0; i < itemCount; i++)
            {
                sale.AddItem(SaleItemTestData.GetValidSaleItem());
            }
            
            return sale;
        }
        
        public static Sale GetCancelledSale()
        {
            var sale = GetValidSale();
            sale.Cancel();
            return sale;
        }
        
        public static List<Sale> GetMultipleSales(int count = 5)
        {
            var sales = new List<Sale>();
            
            for (int i = 0; i < count; i++)
            {
                sales.Add(GetValidSaleWithItems(_faker.Random.Int(1, 5)));
            }
            
            return sales;
        }
        
        public static List<Sale> GetSalesForCustomer(Guid customerId, int count = 3)
        {
            var sales = new List<Sale>();
            
            for (int i = 0; i < count; i++)
            {
                var sale = new Sale(
                    customerId,
                    Guid.NewGuid(),
                    _faker.Date.Recent()
                );
                
                var itemCount = _faker.Random.Int(1, 5);
                for (int j = 0; j < itemCount; j++)
                {
                    sale.AddItem(SaleItemTestData.GetValidSaleItem());
                }
                
                sales.Add(sale);
            }
            
            return sales;
        }
        
        public static List<Sale> GetSalesForBranch(Guid branchId, int count = 3)
        {
            var sales = new List<Sale>();
            
            for (int i = 0; i < count; i++)
            {
                var sale = new Sale(
                    Guid.NewGuid(),
                    branchId,
                    _faker.Date.Recent()
                );
                
                var itemCount = _faker.Random.Int(1, 5);
                for (int j = 0; j < itemCount; j++)
                {
                    sale.AddItem(SaleItemTestData.GetValidSaleItem());
                }
                
                sales.Add(sale);
            }
            
            return sales;
        }
    }
}
