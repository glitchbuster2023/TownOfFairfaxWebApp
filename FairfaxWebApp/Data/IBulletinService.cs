namespace FairfaxWebApp.Data
{
    public interface IBulletinService
    {

        public Task<List<Bulletin>> GetAllBulletins();
        public Task<Bulletin> CreateBulletin(Bulletin bulletin);

        public Task<Bulletin> RemoveBulletin(int id);

    }
}
