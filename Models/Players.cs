using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePrototype
{
    public class Players
    {

        [Table("Players")]
        public class Player
        {

            [PrimaryKey]
            public int PlayerID { get; set; }


        }
    }
}
