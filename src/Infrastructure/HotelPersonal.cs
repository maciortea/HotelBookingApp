using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class HotelPersonal : IdentityUser
    {
        public long HotelId { get; set; }
    }
}
