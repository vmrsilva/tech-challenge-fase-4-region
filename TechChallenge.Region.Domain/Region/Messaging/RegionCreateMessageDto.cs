using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallange.Region.Domain.Region.Messaging
{
    public class RegionCreateMessageDto
    {
        public required string Name { get; init; }
        public required string Ddd { get; init; }
    }
}
