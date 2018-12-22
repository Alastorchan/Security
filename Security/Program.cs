using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using Security.Map;

namespace Security
{
    class Program
    {
        static string dirName = "";
        static string path = "";
        static string subpath = "";
        static List<User> users = new List<User>();
        public static User activeUser;
        public static AccessRights activeAccessRights;
        static string currentPath = @"C:\OS";
        static Map<string, List<ACL>> access = new Map<string, List<ACL>>();

        public static void Load(string filename)
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(Map<string, List<ACL>>));
            access = (Map<string, List<ACL>>)serializer.Deserialize(new FileStream(filename, FileMode.Open));
        }

        public static void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map<string, List<ACL>>));
            serializer.Serialize(new FileStream(filename, FileMode.Create), access);
        }

        static void CreateUser()
        {
            Console.Clear();
            User user2 = new User("Nikita", "asdfg", Role.User);
            User user3 = new User("Sasha", "zxcvb", Role.User);
            String str = "";
            Console.WriteLine("Войти как гость? Y\\N");
            str = Console.ReadLine();
            if (str.ToLower() == "y")
            {
                if (guest == false)
                {
                    name = "Guest";
                    password = "";
                    role = Role.Guest;
                    User guest = new User(name, password, role);
                    users.Add(guest);
                }
                else {
                    Console.WriteLine("Гость уже есть в системе!!!");
                }
            }
            else
            {
                Console.WriteLine("Имя пользователя: ");
                name = Console.ReadLine();
                Console.WriteLine("Пароль: ");
                password = Console.ReadLine();
                role = Role.User;
                User user = new User(name, password, role);
                users.Add(user);
                Console.WriteLine("Пользователь добавлен");
            }
        }

        static void ChangeUser()
        {
            Console.Clear();
            string username;

            for (int i = 0; i < users.Count; i++)
                Console.WriteLine(i + 1 + ": " + " Имя: " + users[i].GetName() + "Статус: " + users[i].GetRole());

            do
            {
                Console.WriteLine("Имя пользователя: ");
                username = Console.ReadLine();
            } while (users.Where(item => item.GetName() == username).Count() == 0);

            User curUser = users.Where(item => item.GetName() == username).First();

            do
            {
                Console.WriteLine("Введите пароль пользователя: ");
                string password = Console.ReadLine();
            } while (password != curUser.GetPassword());

            activeUser = new User(curUser);
        }

        static void MenuPrint()
        {
            // структура файла в NTFS
            // Есть активный пользователь
            while (true)
            {
                Console.Clear();
                dirName = "";
                path = @"C:\OS";
                subpath = "";
                Console.Clear();
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Получение списка файлов и подкатологов");
                Console.WriteLine("2. Создание каталога");
                Console.WriteLine("3. Получение информации о каталоге");
                Console.WriteLine("4. Удаление каталога");
                Console.WriteLine("5. Exit");
                Console.Write("\n" + "Введите команду: ");


                    // https://www.white-windows.ru/sekrety-ntfs-prava-razresheniya-i-ih-nasledovanie/
                    // https://habr.com/post/121806/
                    char ch = char.Parse(Console.ReadLine());

                    switch (ch)
                    {
                        case '1':
                            {
                                // Получение списка файлов и подкатологов
                                Console.WriteLine(@"Формат ввода пути к каталогу: C:\OS\primer");
                                Console.WriteLine("Путь к каталогу: ");
                                dirName = Console.ReadLine();

                                Console.ReadKey();
                                break;
                            }
                        case '2':
                            {
                                //Создание каталога
                                Console.WriteLine(@"Создание в папке C:\OS");
                                Console.WriteLine(@"Формат ввода пути к каталогу: primer\avalon");
                                Console.WriteLine("Путь к каталогу: ");
                                
                                
                                Console.ReadKey();
                                break;
                            }
                        case '3':
                            {
                                //Получение информации о каталоге
                                Console.WriteLine(@"Формат ввода пути к каталогу: C:\OS\primer \n");
                                Console.WriteLine("Путь к каталогу: ");

                                Console.ReadKey();
                                break;
                            }
                        case '4':
                            {
                                //Удаление каталога
                                Console.WriteLine(@"Формат ввода пути к каталогу: C:\OS\primer \n");
                                Console.WriteLine("Путь к каталогу: ");
                                
                                break;
                            }
                        case '5':
                            {
                                return;
                            }
                    }
            }
        }

        static void Main(string[] args)
        {
            // хочу создать пользователей
            // менять пользователей
            // получать доступ к созданным файлам и директориям
            // учитывать какой пользователь создал данный файл
            // другим пользователям дать только просмотр
            // 
            name = "Admin";
            password = "Admin";
            role = Role.Admin;
            User admin = new User(name, password, role);
            users.Add(admin);
            activeUser = admin;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Создать пользователя");
                Console.WriteLine("2. Сменить пользователя");
                Console.WriteLine("3. Действия");
                Console.WriteLine("Exit - Выход");
                var ch = Console.ReadKey();

                switch (ch.Key)
                {
                    case ConsoleKey.D1:
                        {
                            CreateUser();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            ChangeUser();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            MenuPrint();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Save("D:\\test.xml");
                            Environment.Exit(0);
                            break;
                        }
                }
            }
        }
    }
}
