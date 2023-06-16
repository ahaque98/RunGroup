using Microsoft.EntityFrameworkCore;
using RunGroup.Data;
using RunGroup.Interfaces;
using RunGroup.Models;

namespace RunGroup.Repository //Service Class
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context; 
        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(data => data.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetIdByAsync(int id)
        {
            return await _context.Clubs.Include(data => data.Address).FirstOrDefaultAsync(data => data.Id == id);
        }
        public async Task<Club> GetIdByAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(data => data.Address).AsNoTracking().FirstOrDefaultAsync(data => data.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
