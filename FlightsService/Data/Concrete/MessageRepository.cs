using FlightsService.Data.Abstract;
using FlightsService.Models;
using System.Collections.Generic;
using System.Linq;

namespace FlightsService.Data.Concrete
{
    public sealed class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
        }

        IEnumerable<Message> IMessageRepository.Messages => GetDbSet().AsEnumerable();
    }
}
