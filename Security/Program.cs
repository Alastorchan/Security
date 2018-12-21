using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    class Program
    {
        static string dirName = "";
        static string path = "";
        static string subpath = "";
        static List<User> users = new List<User>();
        static DirectoryInfo dirInfo;
        static String name = "";
        static String password = "";
        static Role role;
        static bool guest = false;
        static User activeUser;

        private static void FileSearch(string d, string mask, string maskDir)
        {
            if (d != null)
            {
                if (maskDir == mask)
                    Console.WriteLine(d);

                //Возвращает имена файлов(включая пути) из указанного каталога,
                //отвечающие условиям заданного шаблона поиска.
                string[] files = Directory.GetFiles(d, mask);
                foreach (string f in files)
                    Console.WriteLine(f);
                //Возвращает имена подкаталогов(включая их пути),
                string[] dirs = Directory.GetDirectories(d, maskDir);
                foreach (string d2 in dirs)
                    FileSearch(d2, mask, maskDir);
            }
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
                            Environment.Exit(0);
                            break;
                        }
                }
            }
        }
    }
}
