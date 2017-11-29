using RestServerApi.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RestServerApi.InMemoryDataStores
{
    public static class UserStore
    {
        public static List<User> UsersList = new List<User>();

        static UserStore()
        {
            var userId = Guid.NewGuid();
            UsersList.Add(new User
                                {
                                    Id= userId,
                                    Email = "user01@m.com",
                                    Password = "1!2@3#",
                                    Name = "user01"
                                });
        }

        public static User AddUser(string name, string email, string password)
        {
            var userId = Guid.NewGuid();
            User newUser = new User { Id = userId, Email = email, Password=password, Name = name };
            UsersList.Add(newUser);
            return newUser;
        }

        public static bool FindUser(string userName, string password)
        {
            return UsersList.Exists(u => u.Name == userName && u.Password == password);
        }
    }
}