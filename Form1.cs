using MySql.Data.MySqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace IT_ELECTIVES
{
    public partial class Form1 : Form
    {
        private string connStr;
        MySqlConnection conn;

        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connStr);
            LoadEmployees();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void txtemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtfir.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtfir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtlas.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtlast_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtmid.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtmid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtgen.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtgen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtpos.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtpos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtdep.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtdep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtcon.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtcon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtadd.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void txtadd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveEmployee(); // ✅ call save function
                e.SuppressKeyPress = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connStr = "server=localhost;user=root;database=employee_db;password=;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"UPDATE employees 
               SET first_name=@fir, last_name=@las, middle_name=@mid, 
                   gender=@gen, position=@pos, department=@dep, 
                   contact=@con, address=@add 
               WHERE emp_no=@emp";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    // ✅ Make sure emp_no is INT (if table is INT)
                    cmd.Parameters.AddWithValue("@emp", Convert.ToInt32(txtedit.Text));

                    cmd.Parameters.AddWithValue("@fir", txtfir.Text);
                    cmd.Parameters.AddWithValue("@las", txtlas.Text);
                    cmd.Parameters.AddWithValue("@mid", txtmid.Text);
                    cmd.Parameters.AddWithValue("@gen", txtgen.Text);
                    cmd.Parameters.AddWithValue("@pos", txtpos.Text);
                    cmd.Parameters.AddWithValue("@dep", txtdep.Text);
                    cmd.Parameters.AddWithValue("@con", txtcon.Text);
                    cmd.Parameters.AddWithValue("@add", txtadd.Text);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployees(); // refresh DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No employee updated. Check the emp_no in Edit textbox.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating employee: " + ex.Message);
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    txtemp.Text = row.Cells["emp_no"].Value.ToString();
                    txtfir.Text = row.Cells["first_name"].Value.ToString();
                    txtlas.Text = row.Cells["last_name"].Value.ToString();
                    txtmid.Text = row.Cells["middle_name"].Value.ToString();
                    txtgen.Text = row.Cells["gender"].Value.ToString();
                    txtpos.Text = row.Cells["position"].Value.ToString();
                    txtdep.Text = row.Cells["department"].Value.ToString();
                    txtcon.Text = row.Cells["contact"].Value.ToString();
                    txtadd.Text = row.Cells["address"].Value.ToString();

                  
                }
            }


        }
        // ✅ Load Employees into DataGridView
        private void LoadEmployees(string search = "")
        {
            string connStr = "server=localhost;user=root;database=employee_db;password=;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM employees";

                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE emp_no LIKE @search OR first_name LIKE @search OR last_name LIKE @search";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        }

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employees: " + ex.Message);
                }
            }
        }

        // ✅ Clear all textboxes
        private void ClearBoxes()
        {
           
        }
        private void SaveEmployee()
        {
            {
                string connStr = "server=localhost;user=root;database=employee_db;password=;";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        string sql = "INSERT INTO employees (emp_no, first_name, last_name, middle_name, gender, position, department, contact, address) " +
                                     "VALUES (@emp, @fir, @las, @mid, @gen, @pos, @dep, @con, @add)";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@emp", txtemp.Text);
                        cmd.Parameters.AddWithValue("@fir", txtfir.Text);
                        cmd.Parameters.AddWithValue("@las", txtlas.Text);
                        cmd.Parameters.AddWithValue("@mid", txtmid.Text);
                        cmd.Parameters.AddWithValue("@gen", txtgen.Text);
                        cmd.Parameters.AddWithValue("@pos", txtpos.Text);
                        cmd.Parameters.AddWithValue("@dep", txtdep.Text);
                        cmd.Parameters.AddWithValue("@con", txtcon.Text);
                        cmd.Parameters.AddWithValue("@add", txtadd.Text);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Employee saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployees(); // refresh DataGridView
                            
                        }
                        else
                        {
                            MessageBox.Show("Failed to save employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving employee: " + ex.Message);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connStr = "server=localhost;user=root;database=employee_db;password=;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM employees WHERE emp_no LIKE @search OR first_name LIKE @search OR last_name LIKE @search";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching: " + ex.Message);
                }
            }
        }

        private void txtedit_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    
