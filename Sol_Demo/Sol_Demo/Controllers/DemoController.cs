using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Controllers
{
    [Route("api/redis")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IDistributedCache distributedCache = null;

        public DemoController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        [HttpGet("demo")]
        public async Task<IActionResult> Demo()
        {
            try
            {
                var currentDateTimeObject = DateTime.Now.ToString();

                var cacheDateTime = await this.distributedCache.GetStringAsync("CacheDateTime");
                if (cacheDateTime == null)
                {
                    var distributeCacheEntryOption = new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                    };
                    await this.distributedCache.SetStringAsync("CacheDateTime", currentDateTimeObject, distributeCacheEntryOption);
                    cacheDateTime = await this.distributedCache.GetStringAsync("CacheDateTime");
                }

                return base.Ok(currentDateTimeObject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}