using System;
using SQLite;
using Google.Apis.Drive;
using 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePrototype.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        private string status = "Ready";

        [ObservableProperty]
        private ObservableCollection<DataItem> items;

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            items = new ObservableCollection<DataItem>();
        }

        [RelayCommand]
        private async Task LoadData()
        {
            try
            {
                var loadedItems = await _dbService.GetItemsAsync();
                Items = new ObservableCollection<DataItem>(loadedItems);
                Status = $"Loaded {Items.Count} items";
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task AddTestItem()
        {
            try
            {
                var item = new DataItem
                {
                    Name = $"Test Item {DateTime.Now.Ticks}"
                };
                await _dbService.SaveItemAsync(item);
                await LoadData();
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
            }
        }
    }
}
