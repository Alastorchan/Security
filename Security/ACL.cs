﻿using System;
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

    public class ACL
    {
        //private User user;
        //private AccessRights right;

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
