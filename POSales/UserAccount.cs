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
    public partial class UserAccount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public UserAccount()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadGrideView();
        }

        public void Clear()
        {
            txtFullName.Clear();
            txtPassword.Clear();
            txtReTypePass.Clear();
            txtUserName.Clear();
            cmbRole.Text = "";
            txtUserName.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAccSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != txtReTypePass.Text)
                {
                    MessageBox.Show("Password did not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();
                cm = new SqlCommand("Insert into tblUser(username, password, role, name) Values (@username, @password, @role, @name)", cn);
                cm.Parameters.AddWithValue("@username", txtUserName.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                cm.Parameters.AddWithValue("@role", cmbRole.Text);
                cm.Parameters.AddWithValue("@name", txtFullName.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("New Account has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Warning");
            }
            LoadGrideView();
        }

        private void btnAccCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void LoadGrideView()
        {
            string connectionString = "Data Source=MSIGRATDRAGON;Initial Catalog=DBPOSale;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))

            {

                string query = "SELECT * FROM tblUser";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvUser.DataSource = dt;

            }

        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
          
        }

        private void btnPassCancel_Click(object sender, EventArgs e)
        {
            Clear();

        }
    }
}
