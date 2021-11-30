using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepo userRepo, IMapper mapper, IPhotoService photoService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepo.GetUsers();
            var mappedUsers = _mapper.Map<IEnumerable<MemberDto>>(users);
            
            return Ok(mappedUsers);
        }

        [HttpGet("{username}", Name = "GetUser")]
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();
            var user = await _userRepo.GetUserByUsername(username);

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsProfilePic = true;
            }
            
            user.Photos.Add(photo);

            if (await _userRepo.SaveChanges())
            {
                return CreatedAtRoute("GetUser", new {username = user.UserName},_mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Error during photo upload");
        }
        
        [HttpPut("set-profile-picture/{photoId}")]
        public async Task<ActionResult> SetProfilePicture(int photoId)
        {
            var username = User.GetUsername();
            var user = await _userRepo.GetUserByUsername(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsProfilePic)
            {
                return BadRequest("Same profile picture");
            }
            var currentProfilePic = user.Photos.FirstOrDefault(p => p.IsProfilePic);

            if (currentProfilePic != null)
            {
                currentProfilePic.IsProfilePic = false;
            }

            photo.IsProfilePic = true;

            if (await _userRepo.SaveChanges())
            {
                return NoContent();
            }
            return BadRequest("Error during profile picture change");
        }
        
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();
            var user = await _userRepo.GetUserByUsername(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null)
            {
                return NotFound();
            }
            if (photo.IsProfilePic)
            {
                return BadRequest("Can't delete profile picture");
            }
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhoto(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }
            user.Photos.Remove(photo);
            if (await _userRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Could not delete photo");
        }
    }
}