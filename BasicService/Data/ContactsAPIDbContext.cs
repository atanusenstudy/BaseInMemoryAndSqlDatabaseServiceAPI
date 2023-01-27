using BasicService.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicService.Data
{
    public class ContactsAPIDbContext : DbContext
    {
        public ContactsAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<DBContact> Contacts { get; set; }
    }
}
