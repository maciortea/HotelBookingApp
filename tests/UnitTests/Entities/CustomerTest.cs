using ApplicationCore.Common;
using ApplicationCore.Entities;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class CustomerTest
    {
        [Fact]
        public void Constructor_WithEmptyArguments_ShouldThrowContractException()
        {
            Func<Customer> createCustomer1 = () => new Customer("", "name", "123");
            Func<Customer> createCustomer2 = () => new Customer("name", "", "123");
            Func<Customer> createCustomer3 = () => new Customer("name", "name", "");
            Assert.Throws<ContractException>(createCustomer1);
            Assert.Throws<ContractException>(createCustomer2);
            Assert.Throws<ContractException>(createCustomer3);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var customer = new Customer("first", "last", "123");
            Assert.Equal("first", customer.FirstName);
            Assert.Equal("last", customer.LastName);
            Assert.Equal("123", customer.Phone);
        }
    }
}
