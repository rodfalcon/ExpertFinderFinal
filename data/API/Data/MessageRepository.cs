using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.Include(u => u.Sender).Include(u => u.Recipient ).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(msg =>
            msg.Recipient.username == currentUsername && msg.RecipientDeleted == false
            && msg.Sender.username == recipientUsername
            || msg.Recipient.username == recipientUsername
            && msg.Sender.username == currentUsername && msg.SenderDeleted == false
            )
            .OrderBy(x => x.MessageSent)
            .ToListAsync();

            var unreadmessages = messages.Where(m => m.DateRead == null && m.Recipient.username == currentUsername).ToList();

            if(unreadmessages.Any())
            {
                foreach (var msg in unreadmessages)
                {
                    msg.DateRead = DateTime.Now;

                } 
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<PagedList<MessageDto>> GetsMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.username == messageParams.username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.Sender.username == messageParams.username && u.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.username == messageParams.username && u.RecipientDeleted == false && u.DateRead == null)
            };
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}