using ConwaysGameLifeAPI.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Xml;

namespace ConwaysGameLifeAPI.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private const string StorageFile = "boards.json";
        private ConcurrentDictionary<Guid, Board> _boards;
  
        public BoardRepository()
        {
            _boards = LoadBoards();
        }
        public Guid CreateBoard(List<List<bool>> state)
        {
            var board = new Board { Id = Guid.NewGuid(), State = state, LastModified = DateTime.Now };
            _boards[board.Id] = board;
            AppendBoard(board);
            return board.Id;
        }

        public Board? GetBoard(Guid boardId)
        {
            return _boards.TryGetValue(boardId, out var board) ? board : null; 
        }

        public void UpdateBoard(Board board)
        {
            board.LastModified = DateTime.Now;
            _boards[board.Id] = board;
            SaveBoards();
        }

        private ConcurrentDictionary<Guid, Board> LoadBoards()
        {
            var boards = new ConcurrentDictionary<Guid, Board>();
            if (File.Exists(StorageFile))
            {
                var lines = File.ReadAllLines(StorageFile);
                foreach (var line in lines)
                {
                    var board = JsonConvert.DeserializeObject<Board>(line);
                    if (board != null)
                        boards[board.Id] = board;
                }
            }
            return boards;
        }

        private void AppendBoard(Board board)
        {
            var json = JsonConvert.SerializeObject(board);
            File.AppendAllText(StorageFile, json + Environment.NewLine);
        }

        private void SaveBoards()
        {
            var json = JsonConvert.SerializeObject(_boards);
            File.WriteAllText(StorageFile, json);
        }
    }
}
