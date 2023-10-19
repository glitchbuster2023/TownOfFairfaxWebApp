namespace Town_of_Fairfax.Data
{
    public class Post
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public string? Department { get; set; } = "Bulletin";

    }
}
