using ApplicationCore.Common;

namespace ApplicationCore.Entities
{
    public class Customer : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }

        private Customer()
        {
        }

        public Customer(string firstName, string lastName, string phone)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(firstName), "First name is required");
            Contract.Require(!string.IsNullOrWhiteSpace(lastName), "Last name is required");
            Contract.Require(!string.IsNullOrWhiteSpace(phone), "Phone number is required");

            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }
    }
}
