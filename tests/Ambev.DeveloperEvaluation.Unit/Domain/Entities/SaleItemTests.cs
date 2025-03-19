using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact]
        public void SaleItem_WithValidParameters_ShouldCreateSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 10.0m;
            int quantity = 2;
            
            // Act
            var saleItem = new SaleItem(productId, unitPrice, quantity);
            
            // Assert
            Assert.Equal(productId, saleItem.ProductId);
            Assert.Equal(unitPrice, saleItem.UnitPrice);
            Assert.Equal(quantity, saleItem.Quantity);
            Assert.Equal(0m, saleItem.Discount);
            Assert.Equal(unitPrice * quantity, saleItem.TotalAmount);
            Assert.False(saleItem.Cancelled);
            Assert.NotEqual(Guid.Empty, saleItem.Id);
        }
        
        [Theory]
        [InlineData(3, 0)]
        [InlineData(4, 10)]
        [InlineData(9, 10)]
        [InlineData(10, 20)]
        [InlineData(15, 20)]
        [InlineData(20, 20)]
        public void SaleItem_WithDifferentQuantities_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscountPercentage)
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 100.0m;
            
            // Act
            var saleItem = new SaleItem(productId, unitPrice, quantity);
            
            // Assert
            Assert.Equal(expectedDiscountPercentage, saleItem.Discount);
            
            decimal expectedTotal = unitPrice * quantity;
            if (expectedDiscountPercentage > 0)
            {
                expectedTotal = expectedTotal * (1 - expectedDiscountPercentage / 100);
            }
            Assert.Equal(expectedTotal, saleItem.TotalAmount);
        }
        
        [Fact]
        public void SaleItem_WithQuantityAbove20_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 100.0m;
            int quantity = 21;
            
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => new SaleItem(productId, unitPrice, quantity));
            Assert.Contains("Cannot sell more than 20 items", exception.Message);
        }
        
        [Fact]
        public void SaleItem_WithNegativeQuantity_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 100.0m;
            int quantity = -1;
            
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => new SaleItem(productId, unitPrice, quantity));
            Assert.Contains("Quantity must be greater than 0", exception.Message);
        }
        
        [Fact]
        public void SaleItem_WithZeroQuantity_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 100.0m;
            int quantity = 0;
            
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => new SaleItem(productId, unitPrice, quantity));
            Assert.Contains("Quantity must be greater than 0", exception.Message);
        }
        
        [Fact]
        public void SaleItem_WithNegativeUnitPrice_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = -10.0m;
            int quantity = 1;
            
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => new SaleItem(productId, unitPrice, quantity));
            Assert.Contains("Unit price must be greater than 0", exception.Message);
        }
        
        [Fact]
        public void SaleItem_WithZeroUnitPrice_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            decimal unitPrice = 0m;
            int quantity = 1;
            
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => new SaleItem(productId, unitPrice, quantity));
            Assert.Contains("Unit price must be greater than 0", exception.Message);
        }
        
        [Fact]
        public void UpdateQuantity_ValidQuantity_ShouldUpdateQuantityAndRecalculateTotals()
        {
            // Arrange
            var saleItem = SaleItemTestData.GetValidSaleItem(quantity: 2);
            decimal originalUnitPrice = saleItem.UnitPrice;
            
            // Act
            saleItem.UpdateQuantity(5);
            
            // Assert
            Assert.Equal(5, saleItem.Quantity);
            Assert.Equal(10m, saleItem.Discount); // 10% discount for 4+ items
            Assert.Equal(originalUnitPrice * 5 * 0.9m, saleItem.TotalAmount);
        }
        
        [Fact]
        public void UpdateUnitPrice_ValidPrice_ShouldUpdatePriceAndRecalculateTotals()
        {
            // Arrange
            var saleItem = SaleItemTestData.GetValidSaleItem(quantity: 5);
            int originalQuantity = saleItem.Quantity;
            
            // Act
            saleItem.UpdateUnitPrice(20m);
            
            // Assert
            Assert.Equal(20m, saleItem.UnitPrice);
            Assert.Equal(10m, saleItem.Discount); // 10% discount for 4+ items
            Assert.Equal(20m * originalQuantity * 0.9m, saleItem.TotalAmount);
        }
        
        [Fact]
        public void Cancel_ShouldSetCancelledToTrueAndZeroTotalAmount()
        {
            // Arrange
            var saleItem = SaleItemTestData.GetValidSaleItem();
            
            // Act
            saleItem.Cancel();
            
            // Assert
            Assert.True(saleItem.Cancelled);
            Assert.Equal(0m, saleItem.TotalAmount);
        }
        
        [Fact]
        public void Cancel_AlreadyCancelled_ShouldKeepCancelledAndZeroTotal()
        {
            // Arrange
            var saleItem = SaleItemTestData.GetValidSaleItem();
            saleItem.Cancel();
            
            // Act
            saleItem.Cancel();
            
            // Assert
            Assert.True(saleItem.Cancelled);
            Assert.Equal(0m, saleItem.TotalAmount);
        }
    }
}
