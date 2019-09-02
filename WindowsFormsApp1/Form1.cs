using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-UKIG8U6\SQLEXPRESS;Initial Catalog=MoviesList;Integrated Security=True");
     
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'moviesListDataSet.Movies' table. You can move, or remove it, as needed.
            this.moviesTableAdapter.Fill(this.moviesListDataSet.Movies);
            // TODO: This line of code loads data into the 'movieListDataSet.Movies' table. You can move, or remove it, as needed.
            this.moviesTableAdapter.Fill(this.moviesListDataSet.Movies);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void btnAddNew_Click(object sender, EventArgs e)
        {
                
            SqlCommand cmdInsert = new SqlCommand("INSERT INTO Movies(Title, Actor, Year) VALUES ( @Title, @Actor, @Year);", conn);
            SqlCommand cmdExist = new SqlCommand("SELECT * FROM  Movies WHERE Title='" + textTitle.Text + "'", conn);
            SqlDataReader read;          
           
            if (textTitle.Text == "" || textActor.Text == "" || textYear.Text == "")
            {
                MessageBox.Show("Please enter records...");
            }
            else
            {
                cmdExist.CommandType = CommandType.Text;
                conn.Open();
                read = cmdExist.ExecuteReader();
               
                if (!read.HasRows)
                {
                    conn.Close();
                    
                    try
                    {
                        cmdInsert.CommandType = CommandType.Text;
                        cmdInsert.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = textTitle.Text;
                        cmdInsert.Parameters.Add("@Actor", SqlDbType.VarChar, 50).Value = textActor.Text;
                        cmdInsert.Parameters.Add("@Year", SqlDbType.VarChar, 50).Value = textYear.Text;
                        conn.Open();
                        int j = cmdInsert.ExecuteNonQuery();

                        conn.Close();


                        dataGrid_Load();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    conn.Close();
                    MessageBox.Show("This movie already exist");
                }
            }
            }
            public static void main(string[] args)
            {

                Application.Run(new Form1());

            }

        public void dataGrid_Load()
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movies ORDER BY Title", conn);
            DataSet ds = new DataSet();

            da.Fill(ds, "Movies");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Movies";

            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }
          
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmdUpdate = new SqlCommand("UPDATE Movies SET Title ='" + this.textTitle.Text + "', Actor='" + this.textActor.Text + "', Year='" + this.textYear.Text + "' WHERE Title = '" + textSearch.Text + "'", conn);
            SqlCommand cmdExist = new SqlCommand("SELECT * FROM  Movies WHERE Title='" + textTitle.Text + "'", conn);
            SqlDataReader read;

            if (textTitle.Text == "" || textActor.Text == "" || textYear.Text == "")
            {
                MessageBox.Show("Please enter records...");
            }
            else
            {
                try
                    {
                        conn.Open();
                        cmdUpdate.CommandType = CommandType.Text;

                        cmdUpdate.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = textTitle.Text;
                        cmdUpdate.Parameters.Add("@Actor", SqlDbType.VarChar, 50).Value = textActor.Text;
                        cmdUpdate.Parameters.Add("@Year", SqlDbType.VarChar, 50).Value = textYear.Text;

                        cmdUpdate.ExecuteNonQuery();

                        conn.Close();

                        textSearch.Clear();

                        dataGrid_Load();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                
                }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand del = conn.CreateCommand();
            del.CommandType = CommandType.Text;
            del.CommandText = "DELETE FROM Movies WHERE Title='" + textSearch.Text + "'";
            del.ExecuteNonQuery();
            
            conn.Close();

            dataGrid_Load();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {//search movie by title or actor
            SqlCommand cmd = new SqlCommand("SELECT * FROM Movies WHERE (Title = '" + textSearch.Text + "') OR (Actor = '" + textSearch.Text + "')", conn);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movies WHERE (Title = '" + textSearch.Text + "') OR (Actor = '" + textSearch.Text + "')", conn);
            DataSet ds = new DataSet();
            SqlDataReader read;
           
                conn.Open();

                read = cmd.ExecuteReader();
            if (read.HasRows)
            {
                read.Read();

                textTitle.Text = read.GetValue(0).ToString();
                textActor.Text = read.GetValue(1).ToString();
                textYear.Text = read.GetValue(2).ToString();

                conn.Close();
                da.Fill(ds, "Movies");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Movies";
            }
            else
            {
                MessageBox.Show("Unfortunately, nothing was found what ");
                read.Close();
                conn.Close();
                dataGrid_Load();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGrid_Load();
        }
    }
}
