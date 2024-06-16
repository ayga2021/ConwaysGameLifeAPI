using ConwaysGameLifeAPI.Repositories;
using ConwaysGameLifeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConwaysGameLifeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IBoardRepository _repository;
        private readonly GameManager _gameManager;

        public GameController(IBoardRepository repository, GameManager gameManager)
        {
            _repository = repository;
            _gameManager = gameManager;
        }

        [HttpPost("upload")]
        public ActionResult<Guid> UploadBoard([FromBody] List<List<bool>> state)
        {
            return Ok(_repository.CreateBoard(state));
        }

        [HttpGet("{id}/next")]
        public ActionResult<List<List<bool>>> GetNextState(Guid id)
        {
            var board = _repository.GetBoard(id);
            if (board == null) return NotFound();

            var nextState = _gameManager.GetNextState(board.State);
            board.State = nextState;
            _repository.UpdateBoard(board);

            return Ok(nextState);
        }

        [HttpGet("{id}/stateAfter/{generations}")]
        public ActionResult<List<List<bool>>> GetStateAfterGenerations(Guid id, int generations)
        {
            var board = _repository.GetBoard(id);
            if (board == null) return NotFound();

            var stateAfterGenerations = _gameManager.GetStateAfterGenerations(board.State, generations);
            board.State = stateAfterGenerations;
            _repository.UpdateBoard(board);

            return Ok(stateAfterGenerations);
        }

        [HttpGet("{id}/finalState/{maxGenerations}")]
        public ActionResult<List<List<bool>>> GetFinalState(Guid id, int maxGenerations)
        {
            var board = _repository.GetBoard(id);
            if (board == null) return NotFound();

            var finalState = _gameManager.GetFinalState(board.State, maxGenerations, out bool reachedStability);
            if (!reachedStability)
            {
                return StatusCode(500, "Board did not reach a stable state within the given number of generations.");
            }

            board.State = finalState;
            _repository.UpdateBoard(board);

            return Ok(finalState);
        }
    }
}
