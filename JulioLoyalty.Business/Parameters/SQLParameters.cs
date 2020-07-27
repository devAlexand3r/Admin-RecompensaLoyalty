using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class SQLParameters
    {
        public string tableName { get; set; }
        public List<Column> Columns { get; set; }
        public string userName { get; set; }
    }
    public class Column
    {
        public string Name { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
}

