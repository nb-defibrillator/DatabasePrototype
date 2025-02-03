using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.App.LauncherActivity;


namespace DatabasePrototype
{
    public class DatabaseService

    {
        private SQLiteAsyncConnection database;
        private string dbPath;
        public async Task Initialize()
        {
            if (database != null) return;

            dbPath = Path.Combine(FileSystem.AppDataDirectory, "localdb.db");
            database = new SQLiteAsyncConnection(dbPath);


            await database.CreateTableAsync<DatabasePrototype.DataItem>();
        }

        public async Task<List<DatabasePrototype.DataItem>> GetItemsAsync()
        {
            await Initialize();

            return await database.Table<DatabasePrototype.DataItem>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(DatabasePrototype.DataItem item)
        {
            int update;
            await Initialize();
            if (item.Id == 0)
            {
               update = await database.InsertAsync(item);
            }
            else
            {
                update = await database.UpdateAsync(item);
            }
            // After saving locally, sync with Google Drive
            await SyncWithGoogleDrive();
            item.LastUpdated = DateTime.Now;
            return update;
        }
        public async Task<List<DataItem>> GetItems()
        {
            // First sync with Google Drive to get latest
            await SyncWithGoogleDrive();
            return await database.Table<DataItem>().ToListAsync();
        }

        private async Task SyncWithGoogleDrive()
        {
            // This will be handled by GoogleDriveService
            await GoogleDriveService.Instance.UploadDatabase(dbPath);
        }

    }
}