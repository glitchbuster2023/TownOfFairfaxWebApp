namespace FairfaxWebApp.Data
{
    public class Bulletin
    {

        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; }

    }
}
