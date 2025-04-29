using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGVPrinterHelper;
namespace POSales
{
    public partial class Selling : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        DGVPrinter printer = new DGVPrinter();
        public Selling()
        {
            InitializeComponent();
        }

        private void Selling_Load(object sender, EventArgs e)
        {
            /*string connectionString = "Data Source=MSIGRATDRAGON;Initial Catalog=DBPOSale;Integrated Security=True;Encrypt=Falsess";
            SqlConnection cn = new SqlConnection(connectionString);*/
            SqlConnection cn = new SqlConnection(dbcon.myConnection());
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
        Double granTot = 0, n = 0;

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            double tot = Convert.ToDouble(txtPprice.Text) * Convert.ToDouble(txtPqnt.Text);
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(DataGridViewBill);
            row.Cells[0].Value = ++n;
            row.Cells[1].Value = txtPid.Text;
            row.Cells[2].Value = txtPname.Text;
            row.Cells[3].Value = txtPprice.Text;
            row.Cells[4].Value = txtPqnt.Text;
            row.Cells[5].Value = tot;
            DataGridViewBill.Rows.Add(row);
            granTot = tot + granTot;
            lblGrandtot.Text = granTot.ToString();

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {
            
        }

        private void DataGridViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPid.Text = DataGridViewProduct.SelectedRows[0].Cells[0].Value.ToString();
            txtPname.Text= DataGridViewProduct.SelectedRows[0].Cells[1].Value.ToString();
            txtPprice.Text= DataGridViewProduct.SelectedRows[0].Cells[2].Value.ToString();
            //txtPqnt.Text= DataGridViewProduct.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(dbcon.myConnection());
            String getProduct = "SELECT pcode, pdesc ,price FROM tblProduct WHERE pdesc LIKE '%"+txtSearch.Text+"%';";
            SqlCommand cmd = new SqlCommand(getProduct, cn);

            cn.Open();
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            dt.Fill(table);
            DataGridViewProduct.DataSource = table;
            cn.Close();
            txtSearch.Clear();

        }

       private void txtRefesh_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(dbcon.myConnection());
            String getProduct = "SELECT pcode, pdesc ,price FROM tblProduct ;";
            SqlCommand cmd = new SqlCommand(getProduct, cn);

            cn.Open();
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            dt.Fill(table);
            DataGridViewProduct.DataSource = table;
            cn.Close();
        }

        private void txtSearch_Load(object sender, EventArgs e)
        {
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                SqlConnection cn = new SqlConnection(dbcon.myConnection());
                String getProduct = "SELECT pcode, pdesc ,price FROM tblProduct WHERE pdesc LIKE '%" + txtSearch.Text + "%';";
                SqlCommand cmd = new SqlCommand(getProduct, cn);

                cn.Open();
                SqlDataAdapter dt = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                dt.Fill(table);
                DataGridViewProduct.DataSource = table;
                cn.Close();
            }
            else
            {
                SqlConnection cn = new SqlConnection(dbcon.myConnection());
                String getProduct = "SELECT pcode, pdesc ,price FROM tblProduct ;";
                SqlCommand cmd = new SqlCommand(getProduct, cn);

                cn.Open();
                SqlDataAdapter dt = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                dt.Fill(table);
                DataGridViewProduct.DataSource = table;
                cn.Close();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printer.Title = "FORZONE MOBILE";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Grand total   rs " + granTot.ToString();
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridViewBill);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Cashier cashier = new Cashier();
            cashier.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
