using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public enum Role
    {
        User,
        Admin,
        Guest
    }
    class User
    {
        private string name;
        private string password;
        private Role role;

        public Role GetRole()
        {
            return this.role;
        }

        public string GetPassword()
        {
            return this.password;
        }

        public string GetName()
        {
            return this.name;
        }

        public User()
        {
            name = "Guest";
            password = "";
            role = Role.Guest;
        }

        public User(string name, string password, Role role)
        {
            this.name = name;
            this.password = password;
            this.role = role;
        }

        // Конструктор копирования
        public User(User user)
        {
            this.name = user.name;
            this.password = user.password;
            this.role = user.role;
        }
    }
}
