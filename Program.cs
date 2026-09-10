using System;
using System.Collections.Generic;

internal class Program
{
    static Dictionary<string, string> userCredentials = new Dictionary<string, string>();

    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        bool exit = false;
        bool isAuthenticated = false;

        while (!exit)
        {
            Console.WriteLine("\nМеню:");
            if (!isAuthenticated)
            {
                Console.WriteLine("1. Авторизоваться");
                Console.WriteLine("2. Зарегистрироваться");
                Console.WriteLine("3. Выйти из программы");
            }
            else
            {
                Console.WriteLine("1. Добавить пользователя");      // 12
                Console.WriteLine("2. Удалить пользователя");
                Console.WriteLine("3. Найти пользователя по имени"); // 11
                Console.WriteLine("4. Вывести всех пользователей");
                Console.WriteLine("5. Выйти из учетной записи");
                Console.WriteLine("6. Выйти из программы");
            }

            Console.Write("Выберите опцию: ");

            string choice = Console.ReadLine();

            if (!isAuthenticated)
            {
                switch (choice)
                {
                    case "1":
                        isAuthenticated = Authorize();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            else
            {
                switch (choice)
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        RemoveUser();
                        break;
                    case "3":
                        FindUser();
                        break;
                    case "4":
                        DisplayUsers();
                        break;
                    case "5":
                        isAuthenticated = false;
                        Console.WriteLine("Вы вышли из учетной записи.");
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }


    static bool Authorize()
    {
        Console.WriteLine("Введите имя пользователя:");
        string username = Console.ReadLine();

        Console.WriteLine("Введите пароль:");
        string password = Console.ReadLine();

        if (userCredentials.TryGetValue(username, out string storedPassword) && storedPassword == password) // 4
        {
            Console.WriteLine("Успешная авторизация!");
            return true;
        }
        else
        {
            Console.WriteLine("Неверное имя пользователя или пароль.");
            return false;
        }
    }

    static void Register()
    {
        Console.WriteLine("Введите имя пользователя для регистрации:");
        string username = Console.ReadLine();

        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Имя пользователя не может быть пустым.");
            return; // 7
        }

        if (userCredentials.ContainsKey(username))
        {
            Console.WriteLine("Пользователь с таким именем уже существует.");
            return; // 8
        }

        Console.WriteLine("Введите пароль:");
        string password = Console.ReadLine();

        if (password.Length < 8)
        {
            Console.WriteLine("Пароль слишком короткий. Минимум 8 символов.");
            return; // 9
        }

        string hashedPassword = HashPassword(password); // 13

        userCredentials.Add(username, hashedPassword);
        Console.WriteLine("Пользователь успешно зарегистрирован.");
    }

    static string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    static void AddUser()
    {
        Console.WriteLine("Введите имя пользователя:");
        string name = Console.ReadLine();

        Console.WriteLine("Введите возраст пользователя:");

        if (!int.TryParse(Console.ReadLine(), out int age)) // 6
        {
            Console.WriteLine("Возраст должен быть числом.");
            return;
        }

        if (age < 0)
        {
            Console.WriteLine("Возраст не может быть отрицательным.");
            return; // 10
        }

        users.Add(new User(name, age));

        Console.WriteLine("Пользователь добавлен.");
    }

    static void RemoveUser()
    {
        Console.WriteLine("Введите имя пользователя для удаления:");
        string name = Console.ReadLine();

        User userToRemove = users.Find(u => u.Name == name);

        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            Console.WriteLine("Пользователь удален.");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    static void FindUser()
    {
        Console.WriteLine("Введите имя пользователя для поиска:");
        string name = Console.ReadLine();

        User userFound = users.Find(u => u.Name == name);

        if (userFound != null)
        {
            Console.WriteLine($"Найден пользователь: {userFound.Name}, возраст {userFound.Age}"); // 3
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    static void DisplayUsers()
    {
        if (users.Count == 0)
        {
            Console.WriteLine("Список пользователей пуст.");
            return; // 2
        }

        for (int i = 0; i < users.Count; i++) // 5
        {
            Console.WriteLine($"Имя: {users[i].Name}, Возраст: {users[i].Age}");
        }
    }
}

class User
{
    public string Name;
    public int Age;

    public User(string name, int age)
    {
        Name = name;
        Age = age;
    }
} // 1
