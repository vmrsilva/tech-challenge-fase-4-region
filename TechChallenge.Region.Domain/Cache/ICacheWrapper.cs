﻿using Microsoft.Extensions.Caching.Distributed;

namespace TechChallange.Region.Domain.Cache
{
    public interface ICacheWrapper
    {
        Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default);
        Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default);
    }
}
