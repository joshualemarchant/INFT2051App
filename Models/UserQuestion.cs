using SQLite;

public class UserQuestion
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string? Prompt { get; set; }
    public string? Answer { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsAnswered { get; set; }
}