namespace ConwaysGameLifeAPI.Models
{
    public class Board
    {
        public Guid Id { get; set; }
        public List<List<bool>> State { get; set; }
        public DateTime LastModified { get; set; }
    }
}
