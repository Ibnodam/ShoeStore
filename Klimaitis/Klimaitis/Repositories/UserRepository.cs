using Klimaitis.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShoeStore.Repositories
{
    public class UserRepository
    {
        private readonly ShoeStoreDbContext _context;

        public UserRepository()
        {
            _context = new ShoeStoreDbContext();
        }

        public User? GetUserByLogin(string login)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login);
        }
    }
}








//using Klimaitis.Models;
//using Microsoft.EntityFrameworkCore;

//namespace ShoeStore.Repositories
//{
//    public class UserRepository
//    {
//        private readonly ShoeStoreDbContext _context;

//        public UserRepository()
//        {
//            _context = new ShoeStoreDbContext();
//        }

//        public User? GetUserByLogin(string login)
//        {
//            return _context.Users
//                .Include(u => u.Role)
//                .FirstOrDefault(u => u.Login == login);
//        }

//        public bool ValidateUser(string login, string password, out User? user)
//        {
//            user = GetUserByLogin(login);

//            if (user == null)
//                return false;

//            return user.Password == password;
//        }

//        public List<User> GetAllUsers()
//        {
//            return _context.Users
//                .Include(u => u.Role)
//                .ToList();
//        }

//        public User? GetUserById(int id)
//        {
//            return _context.Users
//                .Include(u => u.Role)
//                .FirstOrDefault(u => u.UserId == id);
//        }
//    }
//}




////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace Klimaitis.Repositories
////{
////    internal class UserRepository
////    {
////    }
////}
