using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace WindowsFormsApplication
{
    public partial class frmGoExec : Form
    {
        public string iddocument;
        public string idworker;
        public frmGoExec()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OleDbConnection con = new OleDbConnection(WindowsFormsApplication.Properties.Settings.Default.bdConnectionString))
            {
                using (OleDbCommand com = new OleDbCommand())
                {
                    com.Connection = con;
                    con.Open();


                    com.CommandText = "update Документ set СостояниеНомер=@СостояниеНомер where ДокументНомер=@ДокументНомер";
                    //com.CommandText = "update Документ set СостояниеНомер=@СостояниеНомер where ДокументНомер=@ДокументНомер";
                    com.Parameters.AddWithValue("@СостояниеНомер", 4);
                    com.Parameters.AddWithValue("@ДокументНомер", iddocument);


                    com.ExecuteNonQuery();
                    con.Close();

                }
                using (OleDbCommand com = new OleDbCommand())
                {

                    com.Connection = con;
                    con.Open();
                    com.CommandText = "insert into Операция  (ДокументНомер, ДатаОперации, ПолучательНомер, СостояниеНомер, Резолюция) VALUES (@ДокументНомер, @ДатаОперации, @ПолучательНомер, @СостояниеНомер, @Резолюция)";

                    com.Parameters.AddWithValue("@ДокументНомер", iddocument);
                    com.Parameters.AddWithValue("@ДатаОперации", DateTime.Today);
                    com.Parameters.AddWithValue("@ПолучательНомер", idworker);
                    com.Parameters.AddWithValue("@СостояниеНомер", 4);
                    com.Parameters.AddWithValue("@Резолюция", textBox1.Text);
                    com.ExecuteNonQuery();

                    con.Close();


                }


            }
            Close();
        }
    }
}
