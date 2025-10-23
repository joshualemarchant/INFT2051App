using SQLite;

public class UserQuestion
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string? Prompt { get; set; } // Customer User question
    public string? Answer { get; set; } // Answer to question
    public DateTime CreatedAt { get; set; } // Date of creation (Current date)

    public bool IsDue { get; set; } // False until until "prompting notification" has been fired
    public bool IsAnswered { get; set; } // Status of whether question has been successfully answered 
}