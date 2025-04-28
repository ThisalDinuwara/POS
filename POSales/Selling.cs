using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class Selling : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;

        public Selling()
        {
            InitializeComponent();
        }

        private void Selling_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MSIGRATDRAGON;Initial Catalog=DBPOSale;Integrated Security=True;Encrypt=Falsess";
            SqlConnection cn = new SqlConnection(connectionString);

            String getProduct = "SELECT pcode, pdesc ,price FROM tblProduct ;";
            SqlCommand cmd = new SqlCommand(getProduct, cn);

            cn.Open();
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            dt.Fill(table);
            DataGridViewProduct.DataSource = table;
            cn.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
