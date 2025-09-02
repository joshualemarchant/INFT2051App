using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppDatabase
{
    private SQLiteAsyncConnection database;

    public AppDatabase()
    {
    }

    // Initialize the database (create table if it doesn't exist)
    public async Task Init()
    {
        if (database != null)
            return;

        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await database.CreateTableAsync<UserQuestion>();
    }

    // Save a question (insert if new, update if existing)
    public async Task<int> SaveItemAsync(UserQuestion item)
    {
        await Init();

        if (item.ID != 0)
            return await database.UpdateAsync(item); // update existing
        else
            return await database.InsertAsync(item); // insert new
    }

    // Get all questions
    public async Task<List<UserQuestion>> GetItemsAsync()
    {
        await Init();
        return await database.Table<UserQuestion>()
                             .OrderByDescending(q => q.CreatedAt)
                             .ToListAsync();
    }

    // Delete a question
    public async Task<int> DeleteItemAsync(UserQuestion item)
    {
        await Init();
        return await database.DeleteAsync(item);
    }
}
