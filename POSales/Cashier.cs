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
            // Initialize labels with zero values to prevent parsing errors
            lblTotalSales.Text = "0.00";
            lblDiscount.Text = "0.00";
            lblVat.Text = "0.00";
            lblVatable.Text = "0.00";
            lblDisplayTotal.Text = "0.00";
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

            try
            {
                if (MessageBox.Show("Remove all items from cart?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblCart WHERE transno LIKE @transno AND status LIKE 'Pending'", cn);
                    cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            slide(btnChangePassword);
            // Add change password functionality here
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            // Add logout functionality here
            if (MessageBox.Show("Logout Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        #endregion button

        public void LoadCart()
        {
            try
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

                    // Safe conversion for total
                    double itemTotal = 0;
                    if (dr["total"] != DBNull.Value && double.TryParse(dr["total"].ToString(), out itemTotal))
                    {
                        total += itemTotal;
                    }

                    // Safe conversion for discount
                    double itemDiscount = 0;
                    if (dr["disc"] != DBNull.Value && double.TryParse(dr["disc"].ToString(), out itemDiscount))
                    {
                        discount += itemDiscount;
                    }

                    // Format values safely for display
                    string formattedTotal = itemTotal.ToString("#,##0.00");
                    string id = dr["id"]?.ToString() ?? "";
                    string pcode = dr["pcode"]?.ToString() ?? "";
                    string pdesc = dr["pdesc"]?.ToString() ?? "";
                    string price = dr["price"]?.ToString() ?? "0";
                    string qty = dr["qty"]?.ToString() ?? "0";
                    string disc = dr["disc"]?.ToString() ?? "0";

                    dgvCash.Rows.Add(i, id, pcode, pdesc, price, qty, disc, formattedTotal);
                }
                dr.Close();
                cn.Close();

                // Update summary labels
                lblTotalSales.Text = total.ToString("#,##0.00");
                lblDiscount.Text = discount.ToString("#,##0.00");
                GetCartTotal();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show("Error loading cart: " + ex.Message, stitle);
            }
        }

        public void GetCartTotal()
        {
            try
            {
                // Safe parsing with defaults
                double discount = 0;
                if (!double.TryParse(lblDiscount.Text, out discount))
                {
                    discount = 0;
                }

                double sales = 0;
                if (!double.TryParse(lblTotalSales.Text, out sales))
                {
                    sales = 0;
                }

                double finalSales = sales - discount;
                double vat = finalSales * 0.12; // 12% VAT
                double vatable = finalSales - vat;

                lblVat.Text = vat.ToString("#,##0.00");
                lblVatable.Text = vatable.ToString("#,##0.00");
                lblDisplayTotal.Text = finalSales.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating totals: " + ex.Message, stitle);
            }
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

                    // Validate transaction format
                    if (transaction.Length >= 12 && int.TryParse(transaction.Substring(8, 4), out count))
                    {
                        lblTransno.Text = sdate + (count + 1).ToString("0000");
                    }
                    else
                    {
                        // Default if format is invalid
                        lblTransno.Text = sdate + "1001";
                    }
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
                // Use a default transaction number if there's an error
                lblTransno.Text = DateTime.Now.ToString("yyyyMMdd") + "1001";
                MessageBox.Show("Error getting transaction: " + ex.Message, stitle);
            }
        }

        // Method to add item to cart from Qty form
        public void AddToCart(string pcode, double price, int qty)
        {
            try
            {
                if (string.IsNullOrEmpty(pcode) || qty <= 0)
                {
                    MessageBox.Show("Invalid product code or quantity", stitle);
                    return;
                }

                // Calculate total
                double total = price * qty;

                // Check if product already exists in cart
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(*) FROM tblCart WHERE pcode = @pcode AND transno = @transno AND status LIKE 'Pending'", cn);
                cm.Parameters.AddWithValue("@pcode", pcode);
                cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                cn.Close();

                if (count > 0)
                {
                    // Update existing item
                    cn.Open();
                    cm = new SqlCommand("UPDATE tblCart SET qty = qty + @qty, total = total + @total WHERE pcode = @pcode AND transno = @transno AND status LIKE 'Pending'", cn);
                    cm.Parameters.AddWithValue("@qty", qty);
                    cm.Parameters.AddWithValue("@total", total);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
                else
                {
                    // Get a default username if lblUsername is empty
                    string cashier = "Unknown";
                    if (lblUsername != null && !string.IsNullOrEmpty(lblUsername.Text))
                    {
                        cashier = lblUsername.Text;
                    }

                    // Add new item
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblCart(transno, pcode, price, qty, disc, total, sdate, cashier, status) VALUES (@transno, @pcode, @price, @qty, @disc, @total, @sdate, @cashier, @status)", cn);
                    cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@qty", qty);
                    cm.Parameters.AddWithValue("@disc", 0);
                    cm.Parameters.AddWithValue("@total", total);
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", cashier);
                    cm.Parameters.AddWithValue("@status", "Pending");
                    cm.ExecuteNonQuery();
                    cn.Close();
                }

                LoadCart(); // Refresh cart display
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                MessageBox.Show("Error adding to cart: " + ex.Message, stitle);
            }
        }

        // Method to remove item from cart
        public void RemoveFromCart(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Invalid item ID", stitle);
                    return;
                }

                if (MessageBox.Show("Remove this item from cart?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblCart WHERE id = @id", cn);
                    cm.Parameters.AddWithValue("@id", id);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart(); // Refresh cart display
                }
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                MessageBox.Show("Error removing item: " + ex.Message, stitle);
            }
        }

        // Method to update product quantity
        public void UpdateInventory()
        {
            try
            {
                cn.Open();
                for (int i = 0; i < dgvCash.Rows.Count; i++)
                {
                    string pcode = dgvCash.Rows[i].Cells[2].Value?.ToString();

                    if (string.IsNullOrEmpty(pcode))
                        continue;

                    int qty = 0;
                    if (dgvCash.Rows[i].Cells[5].Value != null &&
                        int.TryParse(dgvCash.Rows[i].Cells[5].Value.ToString(), out qty) &&
                        qty > 0)
                    {
                        cm = new SqlCommand("UPDATE tblProduct SET qty = qty - @qty WHERE pcode = @pcode", cn);
                        cm.Parameters.AddWithValue("@qty", qty);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                MessageBox.Show("Error updating inventory: " + ex.Message, stitle);
            }
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                string colName = dgvCash.Columns[e.ColumnIndex].Name;
                if (colName == "Delete") // Assuming you have a Delete column
                {
                    string id = dgvCash.Rows[e.RowIndex].Cells[1].Value?.ToString();
                    if (!string.IsNullOrEmpty(id))
                    {
                        RemoveFromCart(id);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, stitle);
            }
        }
    }
}
