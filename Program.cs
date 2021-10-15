using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Authentication
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> dataUser = new List<User>();
            InitiateUser(dataUser);
            int menu;
            bool exit = false;
            do
            {
                Console.Clear();
                DisplayMenu();
                SelectMenuMain(out menu);
                switch (menu)
                {
                    case 1: MenuCreateUser(dataUser); break;
                    //case 2: ShowUser(dataUser); break;
                    //case 3: SearchUser(dataUser); break;
                    case 2: MenuLogin(dataUser); break;
                    case 3: exit = true; break;
                }

            } while (exit == false);
        }

        public static void InitiateUser(List<User> users)
        {
            users.Add(new User("Nelson", "Mandela", "nelma", "Mandela01!"));
            users.Add(new User("John", "Cena", "jocen", "cena01!"));
            users.Add(new Admin("admin", " ", "admin", "admin"));
            users.Add(new User("user", "user", "user", "user"));

        }
        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("************************");
            Console.WriteLine("* Basic Authentication *");
            Console.WriteLine("************************");
            Console.WriteLine("");
            Console.WriteLine("1. Register");
            //Console.WriteLine("2. Show User");
            //Console.WriteLine("3. Search User");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select menu.... ");
        }

        public static void SelectMenuMain(out int menu)
        {
            menu = 0;
            try
            {
                menu = Convert.ToInt32(Console.ReadLine());
                if (menu > 5)
                {
                    throw new FormatException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Input tidak sesuai!!!");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void MenuCreateUser(List<User> dataUser)
        {
            Console.Clear();
            bool isSuccess = false;
            do
            {
                Console.Clear();
                Console.WriteLine("==========Create User===========");
                Console.Write("First Name   : ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name    : ");
                string lastName = Console.ReadLine();
                Console.Write("Password     : ");
                string password = Console.ReadLine();
                string username = firstName.Substring(0, 2) + lastName.Substring(0, 3);

                ///menghindari username yang sama
                bool isExist = false;
                do
                {
                    if (dataUser.Exists(element => element.username == username))
                    {
                        isExist = true;
                        int id = dataUser.FindIndex(element => element.username == username);
                        var rand = new Random();
                        username = username + rand.Next(100);
                    }
                    else
                    { isExist = false; }

                } while (isExist == true);

                try
                {
                    CheckPasswordCriteria(password);
                    Console.WriteLine("Akun berhasil dibuat");
                    dataUser.Add(new User(firstName, lastName, username, password));
                    isSuccess = true;
                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Silahkan ulangi prosedur");
                    Console.ReadKey();
                }

                Console.WriteLine($"Username anda adalah   : {username}");
                Console.ReadKey();
            } while (isSuccess == false);
        }
        public static void ShowUser(List<User> user)
        {
            Console.Clear();
            int i = 1;
            foreach (var item in user)
            {
                Console.WriteLine("==================");
                Console.WriteLine(" User no." + i);
                Console.WriteLine("==================");
                Console.WriteLine($"Name     : {item.firstName} {item.lastName}");
                Console.WriteLine($"Username : {item.username}");
                Console.WriteLine($"Password : {item.Password}");
                Console.WriteLine("");
                i += 1;
            }

            if (user.Count == 0)
            {
                Console.WriteLine("Tidak ditemukan data pengguna");
            }
            Console.ReadKey();
        }

        public static void SearchUser(List<User> dataUser)
        {
            Console.Clear();
            Console.WriteLine("=====Search User======");
            Console.Write("Insert username  : ");
            string uname = Console.ReadLine();
            bool exist = dataUser.Exists(element => element.username == uname);
            Console.WriteLine("is exist = " + exist);

            if (exist == true)
            {
                Console.Clear();
                int findIndex = dataUser.FindIndex(element => element.username == uname);
                Console.WriteLine("==================");
                Console.WriteLine("      Result ");
                Console.WriteLine("==================");
                Console.WriteLine($"Username       : {dataUser[findIndex].username}");
                Console.WriteLine($"First Name     : {dataUser[findIndex].firstName}");
                Console.WriteLine($"Last Name      : {dataUser[findIndex].lastName}");
                Console.WriteLine($"Password       : {dataUser[findIndex].Password}");
                Console.WriteLine("");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Data tidak ditemukan");
            }

            Console.ReadKey();
        }

        public static void MenuLogin(List<User> dataUser)
        {
            Console.Clear();
            Console.WriteLine("Please input your login info...");
            Console.Write("username  : ");
            string username = Console.ReadLine();
            Console.Write("Password  : ");
            string password = Console.ReadLine();
            int id = 0;
            foreach (var item in dataUser)
            {
                if (username == item.username)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, item.Password))
                    {
                        if (item.isAdmin == true)
                        {
                            MenuAdmin(dataUser, id);
                        }
                        else
                        { 
                            MenuEmployee(dataUser, id);
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Password tidak sesuai!!");
                        Console.ReadKey();
                        break;
                    }
                }
                else
                {
                    if (id == dataUser.Count - 1)
                    {
                        Console.WriteLine("Data pengguna tidak ditemukan!!");
                        Console.ReadKey();
                    }
                }
                id += 1;
            }
        }

        public static void MenuEmployee(List<User> dataUser, int id)
        {
            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("===================");
                Console.WriteLine("Employee Menu");
                Console.WriteLine("===================");
                Console.WriteLine(" 1. Edit Password");
                Console.WriteLine(" 2. Delete Account");
                Console.WriteLine(" 3. Exit");
                int adminMenu = 0;

                try
                {
                    adminMenu = Convert.ToInt32(Console.ReadLine());
                    if (adminMenu > 2) { throw new FormatException(); }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input tidak sesuai!!!");
                    Console.ReadLine();
                    Console.Clear();
                }

                switch (adminMenu)
                {
                    case 1:
                        EditPassword(dataUser, id);
                        break;
                    case 2:
                        DeleteAccount(dataUser, id);
                        if (Console.ReadLine() == dataUser[id].Password)
                        {
                            dataUser.RemoveAt(id);
                            Console.Clear();
                            Console.WriteLine("Account removed succesfully");
                            Console.ReadKey();
                            exit = true;
                        }
                        else
                        {
                            Console.WriteLine("Wrong Password!");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        exit = true;
                        break;
                }
            } while (exit == false);
        }

        public static void MenuAdmin(List<User> dataUser, int id)
        {
            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("===================");
                Console.WriteLine("Administration Menu");
                Console.WriteLine("===================");
                Console.WriteLine(" 1. Edit Admin Password");
                Console.WriteLine(" 2. Delete Account");
                Console.WriteLine(" 3. Show User");
                Console.WriteLine(" 4. Search User");
                Console.WriteLine(" 5. Exit");
                Console.WriteLine(" Input menu number to proceed...");
                int adminMenu = 0;

                try
                {
                    adminMenu = Convert.ToInt32(Console.ReadLine());
                    if (adminMenu > 5) { throw new FormatException(); }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input tidak sesuai!!!");
                    Console.ReadLine();
                    Console.Clear();
                }

                switch (adminMenu)
                {
                    case 1:
                        EditPassword(dataUser, id);
                        break;
                    case 2:
                        
                        DeleteAccount(dataUser);
                        int deleteUserId = Convert.ToInt32(Console.ReadLine()) - 1;
                        Console.WriteLine("Please insert password to confirm");
                        if (BCrypt.Net.BCrypt.Verify(Console.ReadLine(), dataUser[id].Password))
                        {
                            dataUser.RemoveAt(deleteUserId);
                            Console.Clear();
                            Console.WriteLine("Account removed succesfully");
                            Console.ReadKey();
                            id = id - 1;
                            //exit = true;
                        }
                        else
                        {
                            Console.WriteLine("Wrong Password!");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        ShowUser(dataUser);
                        break;
                    case 4:
                        SearchUser(dataUser);
                        break;
                    case 5:
                        exit = true;
                        break;
                }
            } while (exit == false);
        }
        
        public static void EditPassword(List<User> dataUser, int id)
        {
            bool isChanged = false;
            do
            {
                Console.Clear();
                Console.WriteLine("=======EDIT PASSWORD=======");
                Console.Write("Please input new password  :");
                string newPassword = Console.ReadLine();
                try
                {
                    CheckPasswordCriteria(newPassword);
                    Console.Write("Repeat your new password   :");
                    if (newPassword != Console.ReadLine())
                    {
                        Console.WriteLine("Password tidak sama");
                    }
                    else
                    {
                        Console.WriteLine("mantap");
                        dataUser[id].Password = newPassword;
                    }
                    Console.ReadKey();
                    isChanged = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Silahkan ulangi proses");
                    Console.ReadKey();
                }

            } while (isChanged == false);

        }
        public static void DeleteAccount(List<User> dataUser, int id)
        {
            Console.Clear();
            Console.WriteLine("========DELETE ACCOUNT========");
            Console.WriteLine("Confirm password to delete your account...");
            Console.Write("password   : ");
        }
        public static void DeleteAccount(List<User> dataUser)
        {
            Console.Clear();
            int i = 1;
            foreach (var item in dataUser)
            {
                Console.WriteLine("==================");
                Console.WriteLine(" User no." + i);
                Console.WriteLine("==================");
                Console.WriteLine($"Username : {item.username}");

                Console.WriteLine("");
                i += 1;
            }
            if (dataUser.Count == 0)
            {
                Console.WriteLine("Tidak ditemukan data pengguna");
            }
            Console.WriteLine("========DELETE ACCOUNT========");
            Console.WriteLine("Choose id that you want to delete...");
            Console.Write("user id   : ");
        }

        public static void CheckPasswordCriteria(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            // var hasSymbols = new Regex(@"!@#$%^&*()_");
            bool hasSymbols = false;
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            //untuk ngecek simbol
            foreach (var item in specialChar)
            {
                if (password.Contains(item))
                { hasSymbols = true; }
            }

            if (!hasLowerChar.IsMatch(password))
            {
                Console.WriteLine("Kata Sandi Wajib diisi");
                throw new FormatException();

            }
            else if (!hasUpperChar.IsMatch(password))
            {
                Console.WriteLine("Kata Sandi Wajib Menggunakan Huruf Besar");
                throw new FormatException();
            }
            else if (!hasMiniMaxChars.IsMatch(password))
            {
                Console.WriteLine("Kata sandi tidak boleh kurang dari atau lebih dari 12 karakter");
                throw new FormatException();
            }
            else if (!hasNumber.IsMatch(password))
            {
                Console.WriteLine("Kata Sandi Wajib diisi Menggunakan Angka");
                throw new FormatException();
            }
            //else if (!hasSymbols.IsMatch(password))
            //{
            //    Console.WriteLine("Kata Sandi Wajib diisi Menggunakan Simbol");
            //    throw new FormatException();
            //}
            else if (hasSymbols == false)
            {
                Console.WriteLine("Kata Sandi Wajib diisi Menggunakan Simbol");
                throw new FormatException();
            }
            else if (password.Length < 2)
            {
                Console.WriteLine("Kata sandi harus mengandung setidaknya satu karakter huruf besar-kecil");
            }
        }
    }
}

