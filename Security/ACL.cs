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
        private User user;
        private AccessRights rights;
        private bool flag;
    }
}
