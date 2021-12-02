using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepo _messageRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public MessageHub(IMessageRepo messageRepo, IUserRepo userRepo, IMapper mapper)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var messages = await _messageRepo.GetMessageThread(Context.User.GetUsername(), otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.ReceiverUsername)
            {
                throw new HubException("Can't send a message to yourself");
            }

            var sender = await _userRepo.GetUserByUsername(username);
            var receiver = await _userRepo.GetUserByUsername(createMessageDto.ReceiverUsername);

            if (receiver == null)
            {
                throw new HubException("Receiver not found");
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
                var group = GetGroupName(sender.UserName, receiver.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }
    }
}