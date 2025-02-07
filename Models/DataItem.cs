using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePrototype
{
    public class DataItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
