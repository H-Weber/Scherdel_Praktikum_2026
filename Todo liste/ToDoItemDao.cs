using System.Data;
using System.Data.Common;
using System.Numerics;
using MySqlConnector;

namespace  todo;

public class ToDoItemDao
{
    private readonly string _connectionString;

    public ToDoItemDao(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<ToDoItem> LoadData()
    {
        List<ToDoItem> toDoItems = new List<ToDoItem>();
        string query = "SELECT * FROM todo";

        foreach (DataRow row in Select(query).Rows)
        {
            toDoItems.Add(GetDtoFromDataRow(row));
        }

        return toDoItems;
    }

    public void SaveData(List<ToDoItem> toDoItems)
    {   
        DeleteData(toDoItems);
        foreach (ToDoItem todoItem in toDoItems)
        {
            if (todoItem.Id == 0)
            {
                string query = string.Format("INSERT INTO todo VALUES(0, '{0}', {1})", todoItem.Description, todoItem.IsDone);
                Insert(query);
            }
        }
    }
    private ToDoItem GetDtoFromDataRow(DataRow row)
    {
        return new ToDoItem {
            Description = Convert.ToString(row["Description"]),
            IsDone = Convert.ToBoolean(row["IsDone"]),
            Id = Convert.ToInt16(row["Id"])
        };
    }

    private DataTable Select(string query)
    {
        DataTable dataTable = new DataTable();
        using MySqlConnection connection = new MySqlConnection(_connectionString);
        try
        {
            connection.Open();

            using MySqlCommand cmd = new MySqlCommand(query, connection);
            using MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            _ = adapter.Fill(dataTable);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        connection.Close();

        return dataTable;
    }

    private void Insert(string query)
    {
        using MySqlConnection connection = new MySqlConnection(_connectionString);
        try
        {
            connection.Open();

            using MySqlCommand cmd = new MySqlCommand(query, connection);
            _ = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        connection.Close();
    }
    private void DeleteData(List<ToDoItem> toDoItems)
    {
        foreach (DataRow row in Select("SELECT * FROM todo").Rows)
        {   
            int id = GetDtoFromDataRow(row).Id;
            if (!toDoItems.Any(t => t.Id == id) & id != 0)
            {
                    Insert($"DELETE FROM todo WHERE Id={id}");
            }
        }       
    }
}
