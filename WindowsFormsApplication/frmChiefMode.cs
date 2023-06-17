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
    public partial class frmChiefMode : Form
    {
        public string idworker;
        bdDataSet.ДокументRow currentDoc = null;
        string defaultFilter = "СостояниеНомер = СостояниеНомер ";

        public frmChiefMode()
        {
            InitializeComponent();
        }



        private void frmChiefMode_Load(object sender, EventArgs e)
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
                comboBox2.Text = "";
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

            документBindingSource.Filter = "СостояниеНомер = СостояниеНомер ";
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


                if (currentDoc.СостояниеНомер == 2)
                {
                    toolStripButton4.Visible = true;
                }
                else
                {
                    toolStripButton4.Visible = false;
                }

                if (currentDoc.СостояниеНомер == 2)
                {
                    toolStripButton6.Visible = true;
                }
                else
                {
                    toolStripButton6.Visible = false;
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
            frmGoWork newForm = new frmGoWork();
            newForm.Text = currentDoc.Документ;
            newForm.iddocument = currentDoc.ДокументНомер.ToString();

            newForm.ShowDialog();

            this.документTableAdapter.Fill(this.bdDataSet.Документ);
            this.операцияTableAdapter.Fill(this.bdDataSet.Операция);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            frmGoAbort newForm = new frmGoAbort();
            newForm.Text = currentDoc.Документ;
            newForm.iddocument = currentDoc.ДокументНомер.ToString();

            newForm.ShowDialog();

            this.документTableAdapter.Fill(this.bdDataSet.Документ);
            this.операцияTableAdapter.Fill(this.bdDataSet.Операция);
        }
    }
}
