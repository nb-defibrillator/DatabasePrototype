using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabasePrototype
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task Initialize()
        {
            if (_database != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "localdb.db");
            _database = new SQLiteAsyncConnection(dbPath);


            await _database.CreateTableAsync<DatabasePrototype.DataItem>();
        }

        public async Task<List<DatabasePrototype.DataItem>> GetItemsAsync()
        {
            await Initialize();

            return await _database.Table<DatabasePrototype.DataItem>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(DatabasePrototype.DataItem item)
        {
            await Initialize();
            item.LastUpdated = DateTime.Now;
            return await _database.InsertAsync(item);
        }
    }
}