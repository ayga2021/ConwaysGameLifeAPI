using ConwaysGameLifeAPI.Models;

namespace ConwaysGameLifeAPI.Repositories
{
    public interface IBoardRepository
    {
        public Guid CreateBoard(List<List<bool>> state);
        public void UpdateBoard(Board board);
        public Board? GetBoard(Guid boardId);
    }
}
