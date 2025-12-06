using Xunit;
using ExamTwo.Models;
using System.Collections.Generic;

namespace ExamTwo.Tests.Models
{
    public class PaymentTests
    {
        [Fact]
        public void ValidateDenominations_ShouldReturnTrue_WhenAllCoinsAreValid()
        {
            // Arrange
            var payment = new Payment
            {
                Coins = new List<int> { 500, 100, 50, 25 }
            };

            // Act
            var (isValid, error) = payment.ValidateDenominations();

            // Assert
            Assert.True(isValid);
            Assert.Empty(error);
        }

        [Fact]
        public void ValidateDenominations_ShouldReturnFalse_WhenCoinIsInvalid()
        {
            // Arrange
            var payment = new Payment
            {
                Coins = new List<int> { 200 }
            };

            // Act
            var (isValid, error) = payment.ValidateDenominations();

            // Assert
            Assert.False(isValid);
            Assert.Contains("200", error);
        }

        [Fact]
        public void ValidateDenominations_ShouldReturnTrue_WhenAllBillsAreValid()
        {
            // Arrange
            var payment = new Payment
            {
                Bills = new List<int> { 1000, 1000 }
            };

            // Act
            var (isValid, error) = payment.ValidateDenominations();

            // Assert
            Assert.True(isValid);
            Assert.Empty(error);
        }

        [Fact]
        public void ValidateDenominations_ShouldReturnFalse_WhenBillIsInvalid()
        {
            // Arrange
            var payment = new Payment
            {
                Bills = new List<int> { 5000 }
            };

            // Act
            var (isValid, error) = payment.ValidateDenominations();

            // Assert
            Assert.False(isValid);
            Assert.Contains("5000", error);
        }

        [Fact]
        public void CalculateTotalFromDenominations_ShouldReturnCorrectSum()
        {
            // Arrange
            var payment = new Payment
            {
                Coins = new List<int> { 500, 100 },
                Bills = new List<int> { 1000 }
            };

            // Act
            var total = payment.CalculateTotalFromDenominations();

            // Assert
            Assert.Equal(1600, total);
        }
    }
}