using FlightsService.Models;
using System.Collections.Generic;

namespace FlightsService.Data.Abstract
{
    interface IMessageRepository : IBaseRepository<Message>
    {
        IEnumerable<Message> Messages { get; }
    }
}
