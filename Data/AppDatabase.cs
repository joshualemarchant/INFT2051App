using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppDatabase
{
    private SQLiteAsyncConnection database;

    public AppDatabase()
    {
    }

    // Initialize the database for a given model type
    public async Task Init<T>() where T : new()
    {
        if (database == null)
        {
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        await database.CreateTableAsync<T>();
    }

    // Save an item (insert if new, update if existing)
    public async Task<int> SaveItemAsync<T>(T item) where T : new()
    {
        await Init<T>();

        // Check if the item has an ID property (commonly used as primary key)
        var propertyInfo = typeof(T).GetProperty("ID");
        if (propertyInfo != null)
        {
            var idValue = (int)(propertyInfo.GetValue(item) ?? 0);

            if (idValue != 0)
                return await database.UpdateAsync(item); // update existing
        }

        return await database.InsertAsync(item); // insert new
    }

    // Get a single item by ID
    public async Task<T?> GetItemAsync<T>(int id) where T : new()
    {
        await Init<T>();

        var propertyInfo = typeof(T).GetProperty("ID");
        if (propertyInfo == null)
            throw new InvalidOperationException($"{typeof(T).Name} does not have an ID property.");

        return await database.FindAsync<T>(id);
    }

    // Get all items of type T
    public async Task<List<T>> GetItemsAsync<T>() where T : new()
    {
        await Init<T>();
        return await database.Table<T>().ToListAsync();
    }

    // Delete an item
    public async Task<int> DeleteItemAsync<T>(T item) where T : new()
    {
        await Init<T>();
        return await database.DeleteAsync(item);
    }
}
