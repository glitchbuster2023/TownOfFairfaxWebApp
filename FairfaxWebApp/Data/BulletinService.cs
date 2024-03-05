using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FairfaxWebApp.Data
{
    public class BulletinService : IBulletinService
    {

        private readonly ApplicationContext _context;

        public BulletinService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Bulletin>> GetAllBulletins() {
            var list = await _context.Bulletins.ToListAsync();

            return list;
        }

        public async Task<Bulletin> CreateBulletin(Bulletin bulletin)
        {
            var result = _context.Bulletins.Add(bulletin);
            await _context.SaveChangesAsync();

            return bulletin;
        }

        public async Task<Bulletin> RemoveBulletin(int id)
        {
            var match = _context.Bulletins.Find(id);
            if(match != null)
            {
                _context.Bulletins.Remove(match);
                await _context.SaveChangesAsync();
            }else
            {
                return null;
            }

            return match;
        }

    }
}
