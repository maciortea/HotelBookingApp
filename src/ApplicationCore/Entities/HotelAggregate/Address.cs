using System.Collections.Generic;
using ApplicationCore.Common;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Address : ValueObject
    {
        public string Street { get; }
        public string City { get; }
        public string Country { get; }
        public string ZipCode { get; }

        public Address(string street, string city, string country, string zipCode)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(street), "Street is required");
            Contract.Require(!string.IsNullOrWhiteSpace(city), "City is required");
            Contract.Require(!string.IsNullOrWhiteSpace(country), "Country is required");
            Contract.Require(!string.IsNullOrWhiteSpace(zipCode), "Zip code is required");

            Street = street;
            City = city;
            Country = country;
            ZipCode = zipCode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return Country;
            yield return ZipCode;
        }
    }
}
