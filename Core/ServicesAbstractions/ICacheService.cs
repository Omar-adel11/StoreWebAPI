using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string Key);
        Task SetCacheValueAsync(string Key, object Value, TimeSpan? duration);
    }
}
