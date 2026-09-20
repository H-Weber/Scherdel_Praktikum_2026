namespace todo;

public class ToDoItem
{
    public string Description { get; set; }
    public bool IsDone { get; set; }

    public void MarkAsDone()
    {
        IsDone = true;
    }

    public override string ToString()
    {
        return $"{Description} [{(IsDone ? "X" : " ")}]";
    }
}