using Microsoft.AspNetCore.Identity;

namespace BookAPI.Data;

public class ApplicationUser:IdentityUser
{
    public string Name { get; set; }
    public int TotalCarts { get; set; }
    // public int PhoneNumber{ get; set; }
    
}