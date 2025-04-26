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
    public partial class Cashier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Point Of Sales";

        public Cashier()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            GetTransaction();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        public void slide(Button button)
        {
            panelSlide.BackColor = Color.White;
            panelSlide.Height = button.Height;
            panelSlide.Top = button.Top;
        }

        #region button

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            slide(btnTransaction);
            GetTransaction();
        }

        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            slide(btnAddDiscount);
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            slide(btnSearchProduct);
            LookUpProduct lookUp = new LookUpProduct(this);
            lookUp.LoadProduct();
            lookUp.ShowDialog();
        }

        private void btnSettlePayment_Click(object sender, EventArgs e)
        {
            slide(btnSettlePayment);
        }

        private void btnClearcart_Click(object sender, EventArgs e)
        {
            slide(btnClearcart);
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            slide(btnChangePassword);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
        }

        #endregion button

        public void LoadCart()
        {
            int i = 0;
            double total = 0;
            double discount = 0;
            dgvCash.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT c.id, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblCart AS c INNER JOIN tblProduct AS p ON c.pcode=p.pcode WHERE c.transno LIKE @transno and c.status LIKE 'Pending'", cn);
            cm.Parameters.AddWithValue("@transno", lblTransno.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                total += Convert.ToDouble(dr["total"].ToString());
                discount += Convert.ToDouble(dr["disc"].ToString());
                dgvCash.Rows.Add(i, dr["id"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            cn.Close();
            lblTotalSales.Text = total.ToString("#,##0.00");
            lblDiscount.Text = discount.ToString("#,##0.00");
            GetCartTotal();
        }
        public void GetCartTotal()
        {
            double discount = double.Parse(lblDiscount.Text);
            double sales = double.Parse(lblTotalSales.Text) - discount;
            double vat = sales * 0.12;
            double vatable = sales - vat;

            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
            lblDisplayTotal.Text = sales.ToString("#,##0.00"); 
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        public void GetTransaction()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transaction;
                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 transno FROM tblCart WHERE transno LIKE '" + sdate + "%' ORDER BY id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transaction = dr[0].ToString();
                    count = int.Parse(transaction.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transaction = sdate + "1001";
                    lblTransno.Text = transaction;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle);
            }
            
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
