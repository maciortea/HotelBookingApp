namespace Web.Models.Reservation
{
    public class FacilityViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public bool FreeOfCharge { get; set; }
        public bool Selected { get; set; }
    }
}
