using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepo.GetUsers();
            var mappedUsers = _mapper.Map<IEnumerable<MemberDto>>(users);
            
            return Ok(mappedUsers);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);

            return _mapper.Map<MemberDto>(user);
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepo.GetUserByUsername(username);
            
            _mapper.Map(memberUpdateDto, user);
            
            _userRepo.Update(user);

            if (await _userRepo.SaveChanges())
            {
                return NoContent();
            }

            return BadRequest("Update failed");
        }
    }
}