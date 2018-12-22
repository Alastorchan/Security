using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public enum AccessRights
    {
        FullAccess, // Полный доступ
        Owner, // Владелец
        Reading, // Чтение
        Record // Запись
    }
    class ACL
    {
        //private User user;
        //private AccessRights right;

        public ACL()
        {
            User = Program.activeUser;
            Right = AccessRights.Reading;
        }


        public User User
        {
            get; set;
        }

        public AccessRights Right
        {
            get; set;
        }

        public ACL(User user, AccessRights right)
        {
            User = new User(user);
            Right = right;
        }
    }
}
