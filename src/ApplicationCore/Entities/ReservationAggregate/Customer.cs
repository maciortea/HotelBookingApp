using System.Collections.Generic;
using ApplicationCore.Common;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Customer : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string FullName => $"{FirstName} {LastName}";

        public Customer(string firstName, string lastName, string phone)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(firstName), "First name is required");
            Contract.Require(!string.IsNullOrWhiteSpace(lastName), "Last name is required");
            Contract.Require(!string.IsNullOrWhiteSpace(phone), "Phone number is required");

            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return Phone;
        }
    }
}
