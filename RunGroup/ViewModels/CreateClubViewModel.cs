using RunGroup.Data.Enums;
using RunGroup.Models;

namespace RunGroup.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; }
        public Address Address { get; set; } 
        public IFormFile Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
