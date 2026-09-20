using System.Runtime.CompilerServices;
using System.Text.Json;

namespace todo;

class Program
{
    // TODO: Einträge abhaken (als erledigt markieren)
    // TODO: Einträge in Datenbank verwalten
    static void Main(string[] args)
    {
        bool exit = false;
        string file = "todod.json";
        Console.Write("enter password fuer DATABASE");
        string password = Console.ReadLine();
        Console.WriteLine("enter USER ID fuer DATABASE");
        string Id = Console.ReadLine();
        List<ToDoItem> todo = [];
        string connection = $"Server=localhost;port=3306;user={Id};password={password};database=todo";

        ToDoItemDao dao = new ToDoItemDao(connection);
        todo = dao.LoadData();

        // if (File.Exists(file))
        // {
        //     try
        //     {
        //         todo = JsonSerializer.Deserialize<List<ToDoItem>>(File.ReadAllText(file));
        //     } catch
        //     {
        //         LogError("Gespeicherte ToDos konnten nicht geladen werden!");
        //     }
        // }
        //string? text = "hello";

        //File.AppendAllText(file,$"{text}\n");
        while (!exit)
        {
            Console.WriteLine("Menü\n [P] todos ausgeben \n [E] todo bearbeiten \n [A] todo hinzufügen \n [D] todo löschen \n \n [S] Speichern \n [X] Verlasssen");
            string? input = Console.ReadLine();//appendtext
            switch (input?.ToLower())
            {
                case "p":
                    Viewtodo(todo);
                    break;
                case "a":
                    Addtodo(todo);
                    break;
                case "e":
                    completetodo(todo);
                    break;
                case "d":
                    Removetodo(todo);
                    break;
                case "x":
                    exit = true;
                    break;
                case "s":
                    safetodo(todo, file);
                    break;
                default:
                    break;
            }
        }
    }

    static void Removetodo(List<ToDoItem> todo)
    {
        Viewtodo(todo);

        Console.WriteLine("enter todo ID to remove");
        int? index = ReadIntegerFromConsole();

        if (index == null)
        {
            return;
        }

        if (index <= 0 || index > todo.Count)
        {
            LogError("Der zu löschende Eintrag existiert nicht!");
            return;
        }

        todo.RemoveAt(index.Value - 1);
    }

    static int? ReadIntegerFromConsole()
    {
        string? input = Console.ReadLine();

        if (input == null)
        {
            LogError("Eingabe erwartet!");
            return null;
        }

        bool isValidInt = int.TryParse(input, out int number);
        if (!isValidInt)
        {
            LogError("Keine gültige Eingabe. Zahl erwartet!");
            return null;
        }

        return number;
    }

    static void LogError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {error}");
        Console.ResetColor();
    }

    static void Viewtodo(List<ToDoItem> todo)
    {
        for (int i = 0; i < todo.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {todo[i]}");
        }

        Console.WriteLine($"Max. {todo.Count}");
    }

    static void Addtodo(List<ToDoItem> todo)
    {
        Console.WriteLine("enter todo:");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            LogError("Eingabe erwartet!");
            return;
        }

        todo.Add(new ToDoItem { Description = input, IsDone = false });
    }
    static void safetodo(List<ToDoItem> todo, string file)
    {
        string json = JsonSerializer.Serialize(todo);
        File.WriteAllText(file, json);
    }
    // static string addcheckbox(string input, bool complete = false)
    // {
    //     //eins [ ]
    //     // string m = todo[idx]
    //     // m.replace("[ ]", "[X]")
    //     input.IndexOf('[');
    //     input.Replace("[ ]", "[X]");


    //     if (complete)
    //     {
    //         return $"{input} [X]";
    //     }
    //     return $"{input} [ ]";
    // }
    static void completetodo(List<ToDoItem> todo)
    {
        Viewtodo(todo);

        Console.WriteLine("enter todo ID to remove");
        int? i = ReadIntegerFromConsole();
        if (i == null)
        {
            return;
        }

        else if (i <= 0 || i > todo.Count)
        {
            LogError("Der zu löschende Eintrag existiert nicht!");
            return;
        }
        int idx = i ?? 0;
        // string target = todo[idx-1];
        // target = target.Replace("[ ]", "[X]");
        todo[idx - 1].MarkAsDone();
    }
}

//lst[1] elemet [-1] = X
