using TeamManagementServiceV2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamManagementServiceV2.Data;

namespace TeamManagementServiceV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamContext context;
        public TeamController(TeamContext context)
        {
            this.context = context;
        }

        // get function to retreive all teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
        {
            return await context.Teams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await context.Teams.FindAsync(id);

            // validation
            if(team == null)
            {
                return NotFound();
            }
            return team;
        }

        // Create a new team
        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] CreateTeamRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TeamName))
            {
                return BadRequest("Team name is required.");
            }

            var newTeam = new Team
            {
                TeamName = request.TeamName,
                PlayerIds = new List<int>()
            };

            context.Teams.Add(newTeam);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = newTeam.TeamId }, newTeam);
        }

        [HttpPost("addPlayer")]
        public async Task<IActionResult> AddPlayerToTeam([FromBody] AddPlayerRequest request)
        {
            var team = await context.Teams.FindAsync(request.TeamId);
            if (team == null)
            {
                return NotFound($"Team with ID {request.TeamId} not found.");
            }

            if (!team.PlayerIds.Contains(request.PlayerId))
            {
                team.PlayerIds.Add(request.PlayerId);
                await context.SaveChangesAsync();
            }

            return Ok(team);
        }

        [HttpPost("removePlayer")]
        public async Task<IActionResult> RemovePlayer([FromBody] RemovePlayerRequest request)
        {
           
            var team = await context.Teams.FindAsync(request.TeamId);
            if (team == null)
            {
                return NotFound($"Team with ID {request.TeamId} not found.");
            }

            
            if (!team.PlayerIds.Contains(request.PlayerId))
            {
                return BadRequest($"Player with ID {request.PlayerId} is not in the team.");
            }

            
            team.PlayerIds.Remove(request.PlayerId);

            
            await context.SaveChangesAsync();

            return Ok(new { Message = $"Player with ID {request.PlayerId} successfully removed from team {team.TeamId}." });
        }

        public class RemovePlayerRequest
        {
            public int TeamId { get; set; }
            public int PlayerId { get; set; }
        }
        public class AddPlayerRequest
        {
            public int TeamId { get; set; }
            public int PlayerId { get; set; }
        }

        public class CreateTeamRequest
        {
            public string TeamName { get; set; } = string.Empty;
        }



    }
}
