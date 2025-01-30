using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task Initialize()
        {
            if (_database != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "localdb.db");
            _database = new SQLiteAsyncConnection(dbPath);


            await _database.CreateTableAsync<DatabasePrototype.Models.DataItem>();
        }

        public async Task<List<DatabasePrototype.Models.DataItem>> GetItemsAsync()
        {
            await Initialize();

            return await _database.Table<DatabasePrototype.Models.DataItem>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(DatabasePrototype.Models.DataItem item)
        {
            await Initialize();
            item.LastUpdated = DateTime.Now;
            return await _database.InsertAsync(item);
        }
    }
}