using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using photogram.Models;
using Microsoft.Extensions.Logging;

namespace photogram.DAL
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(PostDbContext db, ILogger<PostRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Get all the post from teh database, retrives null if errors.
        public async Task<IEnumerable<Post>?> GetAll()
        {
            try
            {
                return await _db.Posts.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetAll failed: {ErrorMessage}", e.Message);
                return null;
            }
        }

        //  Collects singel post by id,  
        public async Task<Post?> GetPostById(int id)
        {
            try
            {
                return await _db.Posts.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetPostById failed for PostId {PostId}: {ErrorMessage}", id, e.Message);
                return null;
            }
        }

        // get all post for the user, spesific user. if not null error messagee
        public async Task<IEnumerable<Post>?> GetPostsByUserName(string userName)
        {
            try
            {
                return await _db.Posts
                    .Where(post => post.UserName == userName)
                    .OrderByDescending(post => post.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetPostsByUserName failed for user {UserName}: {ErrorMessage}", userName, e.Message);
                return null;
            }
        }

        // Counts the total of post in the database.
        public async Task<int> GetPostsCount()
        {
            try
            {
                return await _db.Posts.CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetPostsCount failed: {ErrorMessage}", e.Message);
                return 0;
            }
        }

        // collects paginated list of post, order by the most recent
        public async Task<IEnumerable<Post>?> GetAllPostsPaged(int? pageNr, int pageSize)
        {
            try
            {
                int skip = ((pageNr ?? 1) - 1) * pageSize;
                return await _db.Posts
                    .OrderByDescending(post => post.CreatedAt)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetAllPostsPaged failed: {ErrorMessage}", e.Message);
                return null;
            }
        }

        // Create a new post in database, if sucsessful return trun if not false. 
        public async Task<bool> Create(Post post)
        {
            try
            {
                await _db.Posts.AddAsync(post);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] Create failed for Post {@Post}: {ErrorMessage}", post, e.Message);
                return false;
            }
        }

        // Updates a existing post in the db, return true if right and false other wise. 
        public async Task<bool> Update(Post post)
        {
            try
            {
                _db.Posts.Update(post);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] Update failed for Post {@Post}: {ErrorMessage}", post, e.Message);
                return false;
            }
        }

        // Deletes the post by its Id, returns true if successful, otherwise false. 
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _db.Posts.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("[PostRepository] Delete failed: PostId {PostId} not found", id);
                    return false;
                }

                _db.Posts.Remove(item);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] Delete failed for PostId {PostId}: {ErrorMessage}", id, e.Message);
                return false;
            }
        }

        // Add friend connection in the database, returns true if successful, otherwise false.
        public async Task<bool> AddFriend(Friend friend)
        {
            try
            {
                await _db.Friends.AddAsync(friend);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] AddFriend failed for Friend {@Friend}: {ErrorMessage}", friend, e.Message);
                return false;
            }
        }

        // Removes a friend from database by its Id, returns true if successful, otherwise false.
        public async Task<bool> RemoveFriend(int id)
        {
            try
            {
                var friend = await _db.Friends.FindAsync(id);
                if (friend == null)
                {
                    _logger.LogWarning("[PostRepository] RemoveFriend failed: FriendId {FriendId} not found", id);
                    return false;
                }

                _db.Friends.Remove(friend);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] RemoveFriend failed for FriendId {FriendId}: {ErrorMessage}", id, e.Message);
                return false;
            }
        }

        // Gets all friends for a user, returns empty list if no friend, or error
        public async Task<IEnumerable<Friend>> GetFriends(string userEmail)
        {
            try
            {
                return await _db.Friends
                    .Where(f => f.RequesterEmail == userEmail)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetFriends failed for UserEmail {UserEmail}: {ErrorMessage}", userEmail, e.Message);
                return Enumerable.Empty<Friend>();
            }
        }

        // gets all users, except the user logged in. , return empty list if error happens
        public async Task<IEnumerable<User>> GetAllUsersExcept(string userEmail)
        {
            try
            {
                return await _db.Users
                    .Where(u => u.Email != userEmail)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository] GetAllUsersExcept failed for UserEmail {UserEmail}: {ErrorMessage}", userEmail, e.Message);
                return Enumerable.Empty<User>();
            }
        }


        
    }
}
