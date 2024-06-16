namespace ConwaysGameLifeAPI.Services
{
    public class GameManager
    {
        public List<List<bool>> GetNextState(List<List<bool>> currentState)
        {
            int width = currentState.Count;
            int height = width > 0 ? currentState[0].Count : 0;
            var nextState = CreateEmptyBoard(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int liveNeighbors = CountLiveNeighbors(currentState, x, y, width, height);

                    if (currentState[x][y])
                    {
                        nextState[x] [y] = liveNeighbors == 2 || liveNeighbors == 3;
                    }
                    else
                    {
                        nextState[x][y] = liveNeighbors == 3;
                    }
                }
            }

            return nextState;
        }
        public List<List<bool>> GetStateAfterGenerations(List<List<bool>> initialState, int x)
        {
            var currentState = initialState;

            for (int i = 0; i < x; i++)
            {
                currentState = GetNextState(currentState);
            }

            return currentState;
        }

        public List<List<bool>> GetFinalState(List<List<bool>> initialState, int maxGenerations, out bool reachedStability)
        {
            var currentState = initialState;
            reachedStability = false;

            for (int i = 0; i < maxGenerations; i++)
            {
                var nextState = GetNextState(currentState);

                if (AreBoardsEqual(currentState, nextState))
                {
                    reachedStability = true;
                    return nextState;
                }

                currentState = nextState;
            }

            return currentState;
        }

        private List<List<bool>> CreateEmptyBoard(int width, int height)
        {
            var board = new List<List<bool>>(width);
            for (int i = 0; i < width; i++)
            {
                var row = new List<bool>(new bool[height]);
                board.Add(row);
            }
            return board;
        }
        private bool AreBoardsEqual(List<List<bool>> board1, List<List<bool>> board2)
        {
            int width = board1.Count;
            int height = width > 0 ? board1[0].Count : 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (board1[x] [y] != board2[x][y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private int CountLiveNeighbors(List<List<bool>> grid, int x, int y, int width, int height)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int nx = x + i;
                    int ny = y + j;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        if (grid[nx][ny])
                            count++;
                    }
                }
            }
            return count;
        }
    }
}
