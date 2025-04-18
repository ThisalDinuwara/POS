using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSales
{
    class DBConnect
    {
        private string con;
        public string myConnection()
        {
            con = @"Data Source=MSIGRATDRAGON;Initial Catalog=DBPOSale;Integrated Security=True;Encrypt=False";
            return con;
        }
    }
}
