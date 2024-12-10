using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;
using PlayerManagementService.Model;
using System.Net.NetworkInformation;


namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly PlayerContext context;
        private readonly HttpClient _httpClient;

        // contructor for dependencies
        public PlayerController(PlayerContext context, HttpClient httpClient)
        {
            this.context = context;
            _httpClient = httpClient;
        }
        //list all players drafted or released
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await context.Players.ToListAsync();
            return Ok(players);
        }
        //draft a player to a team
        [HttpPost("draft")]
        public async Task<IActionResult> DraftPlayer([FromBody] DraftRequest request)
        {
            // verify player exists
            var player = await context.Players.FindAsync(request.PlayerId);
            if (player == null)
            {
                return NotFound($"Player with ID {request.PlayerId} not found.");
            }

            // check if the player is already drafted
            if (player.IsDrafted)
            {
                return BadRequest($"Player with ID {request.PlayerId} is already drafted.");
            }

            // verify team to add to exists
            var teamValidationUrl = $"https://localhost:7200/api/team/{request.TeamId}";
            var teamResponse = await _httpClient.GetAsync(teamValidationUrl);
            if (!teamResponse.IsSuccessStatusCode)
            {
                return BadRequest($"Team with ID {request.TeamId} does not exist.");
            }

            // update the player's team field and drafted status
            player.TeamId = request.TeamId;
            player.IsDrafted = true;

            // make call to team API to put the player in the list
            var teamManagementUrl = "https://localhost:7200/api/team/addPlayer";
            var addPlayerRequest = new { TeamId = int.Parse(request.TeamId), PlayerId = request.PlayerId };

            var response = await _httpClient.PostAsJsonAsync(teamManagementUrl, addPlayerRequest);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to update team management service.");
            }

            // save changes
            await context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Player with ID {request.PlayerId} successfully added to team {request.TeamId}.",
                Player = player
            });
        }
        //releast player from a team
        [HttpPost("undraft")]
        public async Task<IActionResult> UndraftPlayer([FromBody] DraftRequest request)
        {
            // verify player exists
            var player = await context.Players.FindAsync(request.PlayerId);
            if (player == null)
            {
                return NotFound($"Player with ID {request.PlayerId} not found.");
            }

            // verify if player is drafted or has a team id already
            if (string.IsNullOrEmpty(player.TeamId) || !player.IsDrafted)
            {
                return BadRequest($"Player with ID {request.PlayerId} is not drafted to any team.");
            }

            //make call to Team API to remove player from team list
            var teamManagementUrl = "https://localhost:7200/api/team/removePlayer";
            var removePlayerRequest = new { TeamId = int.Parse(player.TeamId), PlayerId = request.PlayerId };

            var response = await _httpClient.PostAsJsonAsync(teamManagementUrl, removePlayerRequest);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to update team management service.");
            }

           //update fields 
            player.TeamId = ""; 
            player.IsDrafted = false;
           

            //save changes
            await context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Player with ID {request.PlayerId} successfully removed from team {player.TeamId}.",
                Player = player
            });
        }
        //add player to backend
        [HttpPost("add")]
        public async Task<IActionResult> AddPlayer([FromBody] Player request)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(request.PlayerName))
                {
                    return BadRequest("Player name is required.");
                }

                //create player
                var newPlayer = new Player
                {
                    PlayerName = request.PlayerName,
                    TeamId = request.TeamId,
                    IsDrafted = request.IsDrafted,
                    Details = request.Details
                };

                //add to database
                await context.Players.AddAsync(newPlayer);
                await context.SaveChangesAsync();

                //success feedback
                return Ok(new
                {
                    Message = "Player added successfully.",
                    Player = newPlayer
                });
            }
            catch (Exception ex)
            {
               //error if something doesn't work
                return StatusCode(500, new { Error = "An error occurred while adding the player.", Details = ex.Message });
            }
        }
        //delete player from general database by id
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                //find player by id
                var player = await context.Players.FindAsync(id);

                // make sure player exist
                if (player == null)
                {
                    return NotFound($"Player with ID {id} not found.");
                }

                //remove from database
                context.Players.Remove(player);
                await context.SaveChangesAsync();

                // success feedback
                return Ok(new
                {
                    Message = $"Player with ID {id} successfully deleted."
                });
            }
            catch (Exception ex)
            {
                // return error
                return StatusCode(500, new { Error = "An error occurred while deleting the player.", Details = ex.Message });
            }
        }
        public class DraftRequest
        {
            public int PlayerId { get; set; }
            public string TeamId { get; set; }
        }
    }
}