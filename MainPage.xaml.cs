using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePrototype
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _dbService;
        private readonly GoogleDriveService _driveService;

        public MainPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _driveService = new GoogleDriveService();
            LoadItems();
        }

        private async void LoadItems()
        {
            var items = await _dbService.GetItems();
            ItemsCollection.ItemsSource = items;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var item = new DataItem
            {
                Name = NameEntry.Text,
                LastUpdated = DateTime.Now,
            };

            await _dbService.SaveItem(item);
            LoadItems();

            NameEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            try
            {
                await _driveService.UploadDatabase();
                await DisplayAlert("Success", "Database uploaded to Drive", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
