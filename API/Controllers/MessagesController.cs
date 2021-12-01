using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IUserRepo _userRepo;
        private readonly IMessageRepo _messageRepo;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepo userRepo, IMessageRepo messageRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _messageRepo = messageRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.ReceiverUsername)
            {
                return BadRequest("Can't send a message to yourself");
            }

            var sender = await _userRepo.GetUserByUsername(username);
            var receiver = await _userRepo.GetUserByUsername(createMessageDto.ReceiverUsername);

            if (receiver == null)
            {
                return BadRequest("Receiver not found");
            }

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUsername = receiver.UserName,
                ReceiverUsername = receiver.UserName,
                Content = createMessageDto.Content
            };
            
            _messageRepo.AddMessage(message);

            if (await _messageRepo.SaveChanges())
            {
                return Ok(_mapper.Map<MessageDto>(message));
            }

            return BadRequest("Sending message failed");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages()
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messageRepo.GetMessages(currentUsername));
        }
        
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messageRepo.GetMessageThread(currentUsername, username));
        }
    }
}