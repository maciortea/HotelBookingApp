using ApplicationCore.Entities;
using CSharpFunctionalExtensions;
using Xunit;

namespace UnitTests.Entities
{
    public class EurosTest
    {
        [Fact]
        public void Create_WithNegativeAmount_ShouldReturnFailure()
        {
            Result<Euros> euros = Euros.Create(-1m);
            Assert.True(euros.IsFailure);
        }

        [Fact]
        public void Create_WithExceedingMaximumAmount_ShouldReturnFailure()
        {
            Result<Euros> euros = Euros.Create(1_000_001);
            Assert.True(euros.IsFailure);
        }

        [Fact]
        public void Create_WithPartOfACent_ShouldReturnFailure()
        {
            Result<Euros> euros = Euros.Create(1.234m);
            Assert.True(euros.IsFailure);
        }

        [Fact]
        public void Create_WithValidAmount_ShouldReturnSuccess()
        {
            Result<Euros> euros = Euros.Create(10m);
            Assert.True(euros.IsSuccess);
            Assert.Equal(10m, euros.Value);
        }

        [Fact]
        public void Create_WithZeroAmount_IsZeroShouldBeTrue()
        {
            Euros euros = Euros.Of(0m);
            Assert.True(euros.IsZero);
        }

        [Fact]
        public void AddingTwoEuros_ShouldCreateNewEuroAsSumOfBoth()
        {
            Euros euro1 = Euros.Of(10m);
            Euros euro2 = Euros.Of(5m);
            Euros result = euro1 + euro2;
            Assert.Equal(Euros.Of(15m), result);
        }

        [Fact]
        public void MultiplyingTwoEuros_ShouldCreateNewEuroAsProductOfBoth()
        {
            Euros euro1 = Euros.Of(10m);
            Euros euro2 = Euros.Of(5m);
            Euros result = euro1 * euro2;
            Assert.Equal(Euros.Of(50m), result);
        }
    }
}
