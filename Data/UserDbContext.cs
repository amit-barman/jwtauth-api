using Microsoft.EntityFrameworkCore;
using userauthentication.Models;

namespace userauthentication.Data;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    	
    }
}