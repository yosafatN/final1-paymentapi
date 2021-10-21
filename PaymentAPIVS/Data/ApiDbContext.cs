using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PaymentAPI.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<PaymentDAO> PaymentData { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}