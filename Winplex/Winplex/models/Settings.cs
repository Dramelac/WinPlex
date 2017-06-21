using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Winplex.models
{
    public class Settings
    {
        [PrimaryKey]
        public string Key { get; set; }
        
        public string Value { get; set; }
    }
}
