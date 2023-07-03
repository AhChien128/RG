using Microsoft.EntityFrameworkCore;

namespace RGContext.Context
{
    public class RGContext : DbContext
    {
        public RGContext(DbContextOptions<RGContext> options, IHttpContextAccessor _httpContextAccessor) : base(options)
        {

        }
    }
}
