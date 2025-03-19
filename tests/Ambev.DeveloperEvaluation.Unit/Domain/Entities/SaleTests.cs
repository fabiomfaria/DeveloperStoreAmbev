using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact]
        public void Sale_WithValidParameters_ShouldCreateSuccessfully()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var saleDate = DateTime.Now;

            // Act
            var sale = new Sale(customerId, branchId, saleDate);

            // Assert
            Assert.Equal(customerId, sale.CustomerId);
            Assert.Equal(branchId, sale.BranchId);
            Assert.Equal(saleDate, sale.SaleDate);
            Assert.False(sale.Cancelled);
            Assert.NotEqual(Guid.Empty, sale.Id);
            Assert.Empty(sale.Items);
            Assert.Equal(0, sale.TotalAmount);
        }

        [Fact]
        public void AddItem_WithValidItem_ShouldAddToItemsCollection()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var saleItem = SaleItemTestData.GetValidSaleItem();

            // Act
            sale.AddItem(saleItem);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(saleItem, sale.Items.First());
            Assert.Equal(saleItem.TotalAmount, sale.TotalAmount);
        }

        [Fact]
        public void AddItem_WithMultipleItems_ShouldCalculateTotalAmountCorrectly()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var items = SaleItemTestData.GetMultipleSaleItems(3);
            decimal expectedTotal = 0;

            // Act
            foreach (var item in items)
            {
                sale.AddItem(item);
                expectedTotal += item.TotalAmount;
            }

            // Assert
            Assert.Equal(items.Count, sale.Items.Count);
            Assert.Equal(expectedTotal, sale.TotalAmount);
        }

        [Fact]
        public void Cancel_ShouldSetCancelledToTrue()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.Cancelled);
        }

        [Fact]
        public void Cancel_AlreadyCancelled_ShouldKeepCancelledTrue()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            sale.Cancel();

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.Cancelled);
        }

        [Fact]
        public void RemoveItem_ExistingItem_ShouldRemoveAndUpdateTotal()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var item1 = SaleItemTestData.GetValidSaleItem();
            var item2 = SaleItemTestData.GetValidSaleItem();
            sale.AddItem(item1);
            sale.AddItem(item2);
            decimal expectedTotal = item2.TotalAmount;

            // Act
            sale.RemoveItem(item1.Id);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(expectedTotal, sale.TotalAmount);
        }

        [Fact]
        public void RemoveItem_NonExistentItem_ShouldNotModifyCollection()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var item = SaleItemTestData.GetValidSaleItem();
            sale.AddItem(item);
            decimal initialTotal = sale.TotalAmount;

            // Act
            sale.RemoveItem(Guid.NewGuid());

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(initialTotal, sale.TotalAmount);
        }

        [Fact]
        public void CancelItem_ExistingItem_ShouldCancelItemAndUpdateTotal()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var item = SaleItemTestData.GetValidSaleItem();
            sale.AddItem(item);

            // Act
            sale.CancelItem(item.Id);

            // Assert
            Assert.True(sale.Items.First().Cancelled);
            Assert.Equal(0, sale.TotalAmount);
        }

        [Fact]
        public void CancelItem_NonExistentItem_ShouldNotModifyItems()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var item = SaleItemTestData.GetValidSaleItem();
            sale.AddItem(item);
            decimal initialTotal = sale.TotalAmount;

            // Act
            sale.CancelItem(Guid.NewGuid());

            // Assert
            Assert.False(sale.Items.First().Cancelled);
            Assert.Equal(initialTotal, sale.TotalAmount);
        }

        [Fact]
        public void UpdateCustomer_WithValidId_ShouldUpdateCustomerId()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var newCustomerId = Guid.NewGuid();

            // Act
            sale.UpdateCustomer(newCustomerId);

            // Assert
            Assert.Equal(newCustomerId, sale.CustomerId);
        }

        [Fact]
        public void UpdateBranch_WithValidId_ShouldUpdateBranchId()
        {
            // Arrange
            var sale = SaleTestData.GetValidSale();
            var newBranchId = Guid.NewGuid();

            // Act
            sale.UpdateBranch(newBranchId);

            // Assert
            Assert.Equal(newBranchId, sale.BranchId);
        }
    }
}
