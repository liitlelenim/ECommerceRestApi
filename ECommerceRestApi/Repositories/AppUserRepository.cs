using ECommerceRestApi.DAL;
using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _dbContext;

    public AppUserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppUser> GetByIdAsync(int id)
    {
        AppUser? user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new ArgumentException("User with given id does not exist.");
        }

        return user;
    }

    public async Task<AppUser> GetByUsernameAsync(string username)
    {
        AppUser? user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
        {
            throw new ArgumentException("User with given username does not exist.");
        }

        return user;
    }

    public async Task<bool> ExistByUsernameAsync(string username)
    {
        return await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username) is not null;
    }

    public async Task InsertAsync(AppUser user)
    {
        await _dbContext.AppUsers.AddAsync(user);
    }

    public void RemoveAsync(AppUser user)
    {
        _dbContext.AppUsers.Remove(user);
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}

public interface IAppUserRepository
{
    public Task<AppUser> GetByIdAsync(int id);
    public Task<AppUser> GetByUsernameAsync(string username);
    public Task<Boolean> ExistByUsernameAsync(string username);
    public Task InsertAsync(AppUser user);
    public void RemoveAsync(AppUser user);
    public Task SaveAsync();
}