using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepo : IMessageRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessages(string currentUsername)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .Where(m => m.Receiver.UserName == currentUsername || m.Sender.UserName == currentUsername)
                .OrderBy(m => m.DateSent).ToListAsync();
            
            return _mapper.Map<IEnumerable<MessageDto>>(messages); 
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string receiverUsername)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .Where(m => m.Receiver.UserName == currentUsername && m.Sender.UserName == receiverUsername ||
                            m.Receiver.UserName == receiverUsername && m.Sender.UserName == currentUsername)
                .OrderBy(m => m.DateSent).ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.Receiver.UserName == currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (var unreadMessage in unreadMessages)
                {
                    unreadMessage.DateRead = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}