using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using QuoteQuizApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteQuizApi.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {

        }

        private DataContext UserContext => Context as DataContext;

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await GetByUserName(username);

            // check if username exists
            if (user == null)
                return null;

            //check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        //public new IEnumerable<User> GetAll()
        //{
        //    var users = UserContext.Users.ToList();
        //    return users;
        //}


        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("password is required");

            if (await GetByUserName(user.UserName) != null)
                throw new Exception(@$"user ""{user.UserName}"" already exsists");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            UserContext.Users.Add(user);
            await UserContext.SaveChangesAsync();

            return user;
        }

        //public void Delete(int id)
        //{
        //    var user = UserContext.Users.Find(id);
        //    if (user != null)
        //    {
        //        UserContext.Users.Remove(user);
        //        UserContext.SaveChanges();
        //    }
        //}

        //public User GetById(int id)
        //{
        //    var user = UserContext.Users.FirstOrDefault(u => u.Id == id);
        //    return user;
        //}

        public async Task<User> GetByUserName(string username)
        {
            var res = await UserContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            return res;
        }

        public async Task Update(User userParam, string password = null)
        {

            User user = UserContext.Users.Find(userParam.Id);
            if (user == null)
                throw new Exception("მომხმარებელი ვერ მოიძებნა");

            if (userParam.UserName != user.UserName)
            {
                // username has changed so check if the new username is already taken
                if (UserContext.Users.Any(x => x.UserName == userParam.UserName))
                    throw new Exception("მომხმარებელი " + userParam.UserName + " უკვე არსებობს");
            }

            // update user properties
            user.UserName = userParam.UserName;
            

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            var res = UserContext.Users.Update(user).Entity;
            await UserContext.SaveChangesAsync();

        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }





    }
}
