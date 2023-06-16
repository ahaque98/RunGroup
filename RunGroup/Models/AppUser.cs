using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroup.Models
{
    public class AppUser : IdentityUser
    {        
        public int? Pace { get; set; }   
        public int? Mileage { get; set; }
         
        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Club> Clubs { get; set; }  //one(User) to many(Clubs)
        public ICollection<Race> Races { get; set; }
    }
}
