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
    public partial class Login : Form
    {
        //SqlConnection cn = new SqlConnection();
        //SqlCommand cm = new SqlCommand();
        //DBConnect dbcon = new DBConnect();
        //SqlDataReader dr;

        //public string _pass = "";
        //public bool _isactive;
        private const string ValidUsername = "admin";
        private const string ValidPassword = "123";
        public Login()
        {
            InitializeComponent();
            //cn = new SqlConnection(dbcon.myConnection());

        }

        public void Clear()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { 
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
        private bool AuthenticateUser(string username, string password)
        {
            return username == ValidUsername && password == ValidPassword;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure", "Error",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            Clear();
        }
    }
}
