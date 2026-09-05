using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD.DataAccess
{
    public static class DBConnection
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["DB_DVLD"]
                    .ConnectionString;
            }
        }
    }
}
