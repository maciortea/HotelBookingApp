using ApplicationCore.Common;
using ApplicationCore.Entities;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class AddressTest
    {
        [Fact]
        public void Constructor_WithEmptyArguments_ShouldThrowContractException()
        {
            Func<Address> createAddress1 = () => new Address("", "city", "country", "123");
            Func<Address> createAddress2 = () => new Address("street", "", "country", "123");
            Func<Address> createAddress3 = () => new Address("street", "city", "", "123");
            Func<Address> createAddress4 = () => new Address("street", "city", "country", "");
            Assert.Throws<ContractException>(createAddress1);
            Assert.Throws<ContractException>(createAddress2);
            Assert.Throws<ContractException>(createAddress3);
            Assert.Throws<ContractException>(createAddress4);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var address = new Address("street", "city", "country", "123");
            Assert.Equal("street", address.Street);
            Assert.Equal("city", address.City);
            Assert.Equal("country", address.Country);
            Assert.Equal("123", address.ZipCode);
        }
    }
}
