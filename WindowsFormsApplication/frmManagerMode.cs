using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;


namespace WindowsFormsApplication
{
    public partial class frmManagerMode : Form
    {
        public string idworker;
        bdDataSet.ДокументRow currentDoc = null;
        string defaultFilter = "СостояниеНомер = СостояниеНомер ";
        public frmManagerMode()
        {
            InitializeComponent();
        }

        private void документBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.документBindingSource.EndEdit();
            this.документTableAdapter.Update(this.bdDataSet.Документ);

        }

        private void frmManagerMode_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Операция". При необходимости она может быть перемещена или удалена.
            this.операцияTableAdapter.Fill(this.bdDataSet.Операция);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Состояние". При необходимости она может быть перемещена или удалена.
            this.состояниеTableAdapter.Fill(this.bdDataSet.Состояние);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.bdDataSet.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Документ". При необходимости она может быть перемещена или удалена.
            this.документTableAdapter.Fill(this.bdDataSet.Документ);

            button6_Click(sender, e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            документBindingSource.AddNew();
            документDataGridView.Rows[документDataGridView.CurrentRow.Index].Cells[1].Value = DateTime.Today;
            документDataGridView.Rows[документDataGridView.CurrentRow.Index].Cells[2].Value = DateTime.Today;
            документDataGridView.Rows[документDataGridView.CurrentRow.Index].Cells[5].Value = idworker;
            документDataGridView.Rows[документDataGridView.CurrentRow.Index].Cells[6].Value = 1; 
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Index <= 4)
            {
                button6_Click(sender, e);
                документBindingSource.Filter = "СостояниеНомер = " + ((int)e.Node.Index + (int)1);
                defaultFilter = документBindingSource.Filter;
                comboBox2.SelectedValue = 0;
                comboBox2.SelectedValue = e.Node.Index + 1;
            }
            else
            {
                документBindingSource.Filter = "СостояниеНомер = СостояниеНомер ";
                comboBox2.Text="";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";  
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            textBox1.Clear();
            textBox2.Clear();

            документBindingSource.Filter  = "СостояниеНомер = СостояниеНомер ";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (документDataGridView.IsCurrentCellInEditMode == false)
            {
                //tabControl1.SelectedIndex.ToString();

                    документBindingSource.Filter = defaultFilter;

                if (checkBox1.Checked)
                    документBindingSource.Filter = документBindingSource.Filter + " and ДатаСоздания>='" + dateTimePicker1.Value.ToString("dd-MM-yyyy") + "'";
                if (checkBox2.Checked)
                    документBindingSource.Filter = документBindingSource.Filter + " and ДатаСоздания<='" + dateTimePicker2.Value.ToString("dd-MM-yyyy") + "'";
                if (comboBox1.Text.Length != 0)
                    документBindingSource.Filter = документBindingSource.Filter + " and СоздательНомер=" + Convert.ToString(comboBox1.SelectedValue);
                if (comboBox2.Text.Length != 0)
                    документBindingSource.Filter = документBindingSource.Filter + " and СостояниеНомер=" + Convert.ToString(comboBox2.SelectedValue);
                
                if (textBox1.Text.Length != 0)
                    документBindingSource.Filter = документBindingSource.Filter + " and Документ LIKE '*" + textBox1.Text + "*'";
                if (textBox2.Text.Length != 0)
                    документBindingSource.Filter = документBindingSource.Filter + " and Тема LIKE '*" + textBox2.Text + "*'";
                
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

                    if (currentDoc == null)
                       {
                           MessageBox.Show("Выберите или создайте спецификацию заказа", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           return;

                       }

            string CD;
            CD = Directory.GetCurrentDirectory();

            openFileDialog1.Filter = "*.doc|*.doc";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Directory.SetCurrentDirectory(CD);

                object temp;
                temp = File.ReadAllBytes(openFileDialog1.FileName);

                string name = Path.ChangeExtension(Path.GetTempFileName(), "doc");
                currentDoc.Файл = (byte[])temp;
            }
        }

        private void документDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (документDataGridView.Rows.Count > 0 && документDataGridView.CurrentCell != null && документDataGridView.CurrentCell.RowIndex >= 0 && документDataGridView.Rows[документDataGridView.CurrentCell.RowIndex] != null && документDataGridView.Rows[документDataGridView.CurrentCell.RowIndex].DataBoundItem != null)
            {


                currentDoc = (bdDataSet.ДокументRow)((DataRowView)документDataGridView.Rows[документDataGridView.CurrentCell.RowIndex].DataBoundItem).Row;
               
                if (currentDoc.IsФайлNull())
                {
                    toolStripButton3.Visible = false;
                }
                else
                {
                    toolStripButton3.Visible = true;
                }


                if (currentDoc.СостояниеНомер==1)
                {
                    toolStripButton4.Visible = true;
                }
                else
                {
                    toolStripButton4.Visible = false;
                }


                if (currentDoc.СостояниеНомер != 5)
                {
                    toolStripButton5.Visible = true;
                }
                else
                {
                    toolStripButton5.Visible = false;
                }   
                
                if (!currentDoc.IsNull(0))
                    {
                        операцияBindingSource.Filter = "ДокументНомер=" + currentDoc.ДокументНомер.ToString();
                    }
                    

                    
            }
            else
            {
                currentDoc = null;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (currentDoc == null)
            {
                MessageBox.Show("Выберите или создайте документ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            if (!currentDoc.IsФайлNull())
            {
                string name = Path.ChangeExtension(Path.GetTempFileName(), "doc");

                File.WriteAllBytes(name, currentDoc.Файл);
                Process.Start(name);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmGoConfirm newForm = new frmGoConfirm();
            newForm.Text = currentDoc.Документ;
            newForm.iddocument = currentDoc.ДокументНомер.ToString();

            newForm.ShowDialog();   
            
            this.документTableAdapter.Fill(this.bdDataSet.Документ);
            this.операцияTableAdapter.Fill(this.bdDataSet.Операция);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
            using (OleDbConnection con = new OleDbConnection(WindowsFormsApplication.Properties.Settings.Default.bdConnectionString))
            {
                using (OleDbCommand com = new OleDbCommand())
                {
                    com.Connection = con;
                    con.Open();
                    

                    com.CommandText = "update Документ set СостояниеНомер=@СостояниеНомер where ДокументНомер=@ДокументНомер";
                    //com.CommandText = "update Документ set СостояниеНомер=@СостояниеНомер where ДокументНомер=@ДокументНомер";
                    com.Parameters.AddWithValue("@СостояниеНомер", 5);
                    com.Parameters.AddWithValue("@ДокументНомер", currentDoc.ДокументНомер);                    
                    
                    
                    com.ExecuteNonQuery();
                    con.Close();

                }
                using (OleDbCommand com = new OleDbCommand())
                {

                    com.Connection = con;
                    con.Open();
                    com.CommandText = "insert into Операция  (ДокументНомер, ДатаОперации, ПолучательНомер, СостояниеНомер) VALUES (@ДокументНомер, @ДатаОперации, @ПолучательНомер, @СостояниеНомер)";

                    com.Parameters.AddWithValue("@ДокументНомер", currentDoc.ДокументНомер);
                    com.Parameters.AddWithValue("@ДатаОперации", DateTime.Today);
                    com.Parameters.AddWithValue("@ПолучательНомер", idworker);
                    com.Parameters.AddWithValue("@СостояниеНомер", 5);
                    com.ExecuteNonQuery();
                    
                    con.Close();


                }

                this.документTableAdapter.Fill(this.bdDataSet.Документ);
                this.операцияTableAdapter.Fill(this.bdDataSet.Операция);
            
            }
        }    
         
        
     
    }
}
