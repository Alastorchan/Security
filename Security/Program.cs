using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace Security
{
    class Program
    {
      
        static List<User> users = new List<User>();
        static User activeUser;
        static string currentPath = @"C:\OS";
        static Dictionary<string, List<ACL>> access = new Dictionary<string, List<ACL>>();

        public static void Load(string filename, out Dictionary<string, List<ACL>> loadedAccess)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, List<ACL>>));
            loadedAccess = (Dictionary<string, List<ACL>>)serializer.Deserialize(new FileStream(filename, FileMode.Open));
        }

        public static void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, List<ACL>>));
            serializer.Serialize(new FileStream(filename, FileMode.Create), access);
        }

        static void CreateUser()
        {
            string name = "";
            string password = "";
            Role role;
            Console.Clear();
            Console.WriteLine("Создание пользователя: ");
            Console.WriteLine("Логин: ");
            name = Console.ReadLine();
            Console.WriteLine("Пароль: ");
            password = Console.ReadLine();
            role = Role.User;
            User user = new User(name, password, role);
            users.Add(user);
            Console.WriteLine("Пользователь добавлен");
        }

        static void ChangeUser()
        {
            Console.Clear();
            string username;

            for (int i = 0; i < users.Count; i++)
                Console.WriteLine(i + 1 + ": " + " Логин: " + users[i].GetName() + "    " + "Статус: " + users[i].GetRole());

            do
            {
                Console.WriteLine("Имя пользователя: ");
                username = Console.ReadLine();
            } while (users.Where(item => item.GetName() == username).Count() == 0);
            
            User curUser = users.Where(item => item.GetName() == username).First();

            if (curUser == users[0])
            {
                activeUser = curUser;
                return;
            }

            string password;

            do
            {
                Console.WriteLine("Введите пароль пользователя: ");
                password = Console.ReadLine();
            } while (password != curUser.GetPassword());

            activeUser = new User(curUser);
        }

        public static void MenuPrint()
        {
            // структура файла в NTFS
            // Есть активный пользователь            


            while (true)
            {
                Console.Clear();
                Console.Write("{0}> ", currentPath);
                string command = Console.ReadLine();

                if (command == "") continue;
                else
                {
                    string[] attributes = command.Split(); // разделили на команду и путь

                    switch (attributes[0])
                    {
                        case "help":
                            {
                                Console.Clear();
                                Console.WriteLine("Файловые операции: ");
                                Console.WriteLine("cd — переход по директориям.");
                                Console.WriteLine("open — открытие файла.");
                                Console.WriteLine("about — информация о файле.");
                                Console.WriteLine("ls — выводит список файлов и каталогов.");
                                Console.WriteLine("editrights — редактировать права доступа.");
                                Console.WriteLine("rm — удаление.");
                                Console.WriteLine("mv — перемещение.");
                                Console.WriteLine("cp — копирование.");
                                Console.WriteLine("mkdir — создание директории.");
                                Console.WriteLine("touch — создание пустого файла.");
                                Console.WriteLine("clear — очищает экран терминала.");
                                Console.ReadKey();
                                break;
                            }

                        case "cd":
                            {
                                // все пользователи
                                if (attributes.Length == 1)
                                {
                                    Console.WriteLine("Использование: cd <путь>");
                                }
                                else
                                {
                                    currentPath = attributes[1];
                                }
                                Console.WriteLine(currentPath);
                                break;
                            }

                        case "open":
                            {
                                //OpenRead()  Создает доступный только для чтения объект FileStream
                                //OpenText()  Создает объект StreamReader и читает из существующего текстового файла
                                //OpenWrite() Создает доступный только для записи объект FileStream
                                //открыть могут все
                                //как предусмотреть изменение?
                                //ACL alc; 
                                //if (activeUser.GetRole == Role.Guest)
                                //{

                                //}
                                FileInfo file = new FileInfo(attributes[1]);
                                file.Attributes = FileAttributes.ReadOnly;

                                if (File.Exists(currentPath + "\\" + attributes[1]))
                                {
                                    Process.Start(currentPath + "\\" + attributes[1]);
                                }
                                else if (File.Exists(attributes[1]))
                                {
                                    Process.Start(attributes[1]);
                                    file.OpenRead();
                                }
                                else
                                    Console.WriteLine("Файл \"{0}\" не существует", attributes[1]);
                                break;
                            }

                        case "about":
                            {
                                // вывести пользователя, создавшего файл
                                // таблицу ACL
                                Console.ReadKey();
                                break;
                            }

                        case "ls":
                            {
                                // могут все пользователи
                                string path = currentPath;
                                string mask = "*.*";
                                string maskDir = "*.*";
                               
                                string[] dirs = Directory.GetDirectories(path, maskDir);
                                foreach (string d in dirs)
                                    FileSearch(d, mask, maskDir);
                                Console.ReadKey();
                                break;
                            }

                        case "editrights":
                            {
                                // редактировать может админ или владелец
                                // вывести всех пользователей, каждому переназначить права
                                Console.ReadKey();
                                break;
                            }

                        case "rm":
                            {
                                // удалить может только владелец или админ
                                if (attributes[1].Contains('.'))
                                {
                                    // Delete a file by using File class static method...
                                    if (File.Exists(attributes[1]))
                                    {
                                        // Use a try block to catch IOExceptions, to
                                        // handle the case of the file already being
                                        // opened by another process.
                                        try
                                        {
                                            File.Delete(attributes[1]);
                                        }
                                        catch (IOException e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    deleteFolder(attributes[1]); //Вызываем наш рекурсивный метод
                                                                 //После вызова метода deleteFolder() папка, путь которой указан в deletePath,
                                                                 //должна быть пуста. Остаётся просто удалить её.
                                                                 //Делаем это, вызвав метод Directory.Delete().  
                                    try
                                    {
                                        Directory.Delete(attributes[1]);
                                        Console.WriteLine("Папка {0} успешно удалена", attributes[1]);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("При удалении папки возникли ошибки");
                                    }
                                }
                                Console.ReadKey();
                                break;
                            }

                        case "mv":
                            {
                                // сохраняет свою таблицу ACL
                                // перемещать может владелец и админ
                                if (attributes.Length == 1)
                                {
                                    Console.WriteLine("Использование: mv <название папки> <название папки>");
                                }
                                else
                                {
                                    if (attributes[1].Contains('.') && attributes[2].Contains('.'))
                                    {
                                        string sourceFile = attributes[1];
                                        string destinationFile = attributes[2];

                                        // To move a file or folder to a new location:
                                        File.Move(sourceFile, destinationFile);
                                    }
                                    else
                                    {
                                        // To move an entire directory. To programmatically modify or combine
                                        // path strings, use the System.IO.Path class.
                                        Directory.Move(attributes[1], attributes[2]);
                                    }
                                }
                                Console.ReadKey();
                                break;
                            }

                        case "cp":
                            {
                                // https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/file-system/how-to-copy-delete-and-move-files-and-folders
                                // копировать могут все
                                // права изменяются на те что, сейчас у родительской папки
                                if (attributes.Length == 1)
                                {
                                    Console.WriteLine("Использование: cp <название папки> <название папки>");
                                }
                                else
                                {
                                    List<string> directories = attributes[1].Split('\\').ToList();
                                    string fileName = directories.Last();
                                    string sourcePath = attributes[1];
                                    string targetPath = attributes[2];
                                    string destFile = "";
                                    string sourceFile = "";
                                    
                                    if (File.Exists(attributes[1]))
                                    {
                                        sourcePath = "";
                                        targetPath = "";
                                        directories.Remove(directories.Last());
                                        foreach (string dir in directories)
                                        {
                                            if (dir == directories.Last())
                                                sourcePath += dir;
                                            else
                                                sourcePath += dir + "\\";
                                        }
                                        targetPath = attributes[2]; // путь к папке
                                        // Use Path class to manipulate file and directory paths.
                                        sourceFile = Path.Combine(sourcePath, fileName); // откуда
                                        destFile = Path.Combine(targetPath, fileName); // куда

                                        if (!Directory.Exists(targetPath))
                                        {
                                            Directory.CreateDirectory(targetPath);
                                        }

                                        File.Copy(sourceFile, destFile, true);
                                    }
                                    else
                                    {
                                        if (Directory.Exists(sourcePath))
                                        {
                                            string[] files = Directory.GetFiles(sourcePath);
                                            // Copy the files and overwrite destination files if they already exist.
                                            foreach (string s in files)
                                            {
                                                // Use static Path methods to extract only the file name from the path.
                                                fileName = Path.GetFileName(s);
                                                destFile = Path.Combine(targetPath, fileName);
                                                File.Copy(s, destFile, true);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Source path does not exist!");
                                        }
                                    }
                                    Console.WriteLine("Press any key to exit.");
                                }
                                Console.ReadKey();
                                break;
                            }

                        case "mkdir":
                            {
                                // создать могут все
                                if (attributes.Length == 1)
                                {
                                    Console.WriteLine("Использование: mkdir <название папки>");
                                }
                                else
                                {
                                    // mkdir /home/weazy/kek
                                    List<ACL> userACL = new List<ACL>();

                                    for (int i = 0; i < users.Count; i++)
                                    {
                                        if (users[i] == activeUser)
                                        {
                                            if (users[i].GetRole() == Role.Guest)
                                                userACL.Add(new ACL(users[i], AccessRights.Reading));
                                            else
                                                userACL.Add(new ACL(users[i], AccessRights.Owner));
                                        }
                                        else if (users[i].GetRole() == Role.Admin)
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.FullAccess));
                                        }
                                        else if (users[i].GetRole() == Role.Guest)
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.Reading));
                                        }
                                        else
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.Record));
                                        }
                                    }
                                    access.Add(attributes[1], userACL);

                                    string pathString = attributes[1];

                                    Directory.CreateDirectory(pathString);
                                    
                                    Console.WriteLine("Папка создана.");
                                }
                                Console.ReadKey();
                                break;
                            }

                        case "touch":
                            {
                                // создать могут все
                                if (attributes.Length == 1)
                                {
                                    Console.WriteLine("Использование: touch <название файла>");
                                }
                                else
                                {
                                    List<ACL> userACL = new List<ACL>();

                                    for (int i = 0; i < users.Count; i++)
                                    {
                                        if (users[i] == activeUser)
                                        {
                                            if (users[i].GetRole() == Role.Guest)
                                                userACL.Add(new ACL(users[i], AccessRights.Reading));
                                            else
                                                userACL.Add(new ACL(users[i], AccessRights.Owner));
                                        }
                                        else if (users[i].GetRole() == Role.Admin)
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.FullAccess));
                                        }
                                        else if (users[i].GetRole() == Role.Guest)
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.Reading));
                                        }
                                        else
                                        {
                                            userACL.Add(new ACL(users[i], AccessRights.Record));
                                        }
                                    }
                                    access.Add(attributes[1], userACL);

                                    string pathString = attributes[1];
                                    List<string> directories = attributes[1].Split('\\').ToList();
                                    string fileName = directories.Last();
                                    if (!File.Exists(pathString))
                                    {
                                        FileStream fs = File.Create(pathString);
                                    }
                                    else
                                    {
                                        Console.WriteLine("File \"{0}\" already exists.", fileName);
                                    }
                                }
                                Console.ReadKey();
                                break;
                            }

                        case "clear":
                            {
                                //все
                                Console.Clear();
                                break;
                            }
                        case "exit":
                            return;

                        default: continue;
                    }
                }
            }
        }

        private static void deleteFolder(string folder)
        {
            try
            {
                //Класс DirectoryInfo как раз позволяет работать с папками. Создаём объект этого
                //класса, в качестве параметра передав путь до папки.
                DirectoryInfo di = new DirectoryInfo(folder);
                //Создаём массив дочерних вложенных директорий директории di
                DirectoryInfo[] diA = di.GetDirectories();
                //Создаём массив дочерних файлов директории di
                FileInfo[] fi = di.GetFiles();
                //В цикле пробегаемся по всем файлам директории di и удаляем их
                foreach (FileInfo f in fi)
                {
                    f.Delete();
                }
                //В цикле пробегаемся по всем вложенным директориям директории di 
                foreach (DirectoryInfo df in diA)
                {
                    //Как раз пошла рекурсия
                    deleteFolder(df.FullName);
                    //Если в папке нет больше вложенных папок и файлов - удаляем её,
                    if (df.GetDirectories().Length == 0 && df.GetFiles().Length == 0) df.Delete();
                }
            }
            //Начинаем перехватывать ошибки
            //DirectoryNotFoundException - директория не найдена
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Директория не найдена. Ошибка: " + ex.Message);
            }
            //UnauthorizedAccessException - отсутствует доступ к файлу или папке
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
            }
            //Во всех остальных случаях
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка. Обратитесь к администратору. Ошибка: " + ex.Message);
            }

        }

        static void FileSearch(string d, string mask, string maskDir)
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

        static void Main(string[] args)
        {
            // хочу создать пользователей
            // менять пользователей
            // получать доступ к созданным файлам и директориям
            // учитывать какой пользователь создал данный файл
            // другим пользователям дать только просмотр
            // 
            
            User guest = new User("Guest", "", Role.Guest);
            users.Add(guest);

            User admin = new User("Admin", "Admin", Role.Admin);
            users.Add(admin);

            activeUser = guest;

            User user1 = new User("sasha", "sasha", Role.User);
            User user2 = new User("name2", "name2", Role.User);
            User user3 = new User("name3", "name3", Role.User);
            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

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
                            Save("C:\test.xml");
                            Environment.Exit(0);
                            break;
                        }
                }
            }
        }
    }
}
