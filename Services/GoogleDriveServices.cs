using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DatabasePrototype
{
    public class GoogleDriveService
{
    private DriveService _driveService;
    public static GoogleDriveService Instance { get; } = new GoogleDriveService();
    private const string DATABASE_FILE_NAME = "mydb.db";

    private GoogleDriveService()
    {
        // Initialize Google Drive service here
    }

    public async Task UploadDatabase(string localDbPath)
    {
        try
        {
            // Check if file exists in Drive
            var fileId = await GetDatabaseFileId();

            if (fileId != null)
            {
                // Update existing file
                var file = new Google.Apis.Drive.v3.Data.File();
                using (var fsSource = new FileStream(localDbPath, FileMode.Open, FileAccess.Read))
                {
                    var updateRequest = _driveService.Files.Update(file, fileId, fsSource, "application/x-sqlite3");
                    await updateRequest.UploadAsync();
                }
            }
            else
            {
                // Upload new file
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = DATABASE_FILE_NAME,
                    MimeType = "application/x-sqlite3"
                };

                using (var fsSource = new FileStream(localDbPath, FileMode.Open, FileAccess.Read))
                {
                    var request = _driveService.Files.Create(fileMetadata, fsSource, "application/x-sqlite3");
                    await request.UploadAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle or log error
            Debug.WriteLine($"Error uploading to Drive: {ex.Message}");
        }
    }

    private async Task<string> GetDatabaseFileId()
    {
        try
        {
            var listRequest = _driveService.Files.List();
            listRequest.Q = $"name = '{DATABASE_FILE_NAME}'";
            var files = await listRequest.ExecuteAsync();
            return files.Files.FirstOrDefault()?.Id;
        }
        catch
        {
            return null;
        }
    }

    public async Task DownloadLatestDatabase(string localDbPath)
    {
        try
        {
            var fileId = await GetDatabaseFileId();
            if (fileId != null)
            {
                var request = _driveService.Files.Get(fileId);
                using (var stream = new FileStream(localDbPath, FileMode.Create))
                {
                    await request.DownloadAsync(stream);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error downloading from Drive: {ex.Message}");
        }
    }
}

}
