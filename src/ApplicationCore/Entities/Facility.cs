using ApplicationCore.SharedKernel;

namespace ApplicationCore.Entities
{
    public class Facility : Entity
    {
        public string Name { get; private set; }
        public bool FreeOfCharge { get; private set; }
        public Euros UnitPrice { get; private set; }

        protected Facility()
        {
        }

        public Facility(string name, Euros unitPrice, bool freeOfCharge)
        {
            Name = name;
            UnitPrice = unitPrice;
            FreeOfCharge = freeOfCharge;
        }
    }
}
