using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallange.Common.MessagingService
{
    public interface IMessagingService
    {
        Task<bool> SendMessage<T>(string queueName, T message);
    }
}
