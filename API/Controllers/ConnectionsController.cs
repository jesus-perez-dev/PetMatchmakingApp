using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ConnectionsController : BaseController
    {
        private readonly IUserRepo _userRepo;
        private readonly IConnectionsRepo _connectionsRepo;

        public ConnectionsController(IUserRepo userRepo, IConnectionsRepo connectionsRepo)
        {
            _userRepo = userRepo;
            _connectionsRepo = connectionsRepo;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUser = User.GetUsername();
            var sourceUserId = (await _userRepo.GetUserByUsername(sourceUser)).Id;
            var likedUser = await _userRepo.GetUserByUsername(username);
            var connectionsOfUser = await _connectionsRepo.GetConnectionsOfUser(sourceUserId);

            if (likedUser == null)
            {
                return NotFound();
            }
            if (connectionsOfUser.UserName == username)
            {
                return BadRequest("Can't like yourself");
            }

            var connection = await _connectionsRepo.GetConnection(sourceUserId, likedUser.Id);
            if (connection != null)
            {
                return BadRequest("User already liked");
            }

            connection = new Connection
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            
            connectionsOfUser.LikedUsers.Add(connection);

            if (await _userRepo.SaveChanges())
            {
                return Ok();
            }

            return BadRequest("Could not like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectionDto>>> GetConnections(string connectionType)
        {
            var username = User.GetUsername();
            var userId = (await _userRepo.GetUserByUsername(username)).Id;
            
            var connections = await _connectionsRepo.GetConnections(connectionType, userId);
            return Ok(connections);
        }
    }
}