using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class frmAdminMode : Form
    {
        public frmAdminMode()
        {
            InitializeComponent();
        }

        private void состояниеBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.состояниеBindingSource.EndEdit();
            this.состояниеTableAdapter.Update(this.bdDataSet.Состояние);

        }

        private void frmAdminMode_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.bdDataSet.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Должность". При необходимости она может быть перемещена или удалена.
            this.должностьTableAdapter.Fill(this.bdDataSet.Должность);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Подразделение". При необходимости она может быть перемещена или удалена.
            this.подразделениеTableAdapter.Fill(this.bdDataSet.Подразделение);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Состояние". При необходимости она может быть перемещена или удалена.
            this.состояниеTableAdapter.Fill(this.bdDataSet.Состояние);

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            tabControl1.SelectedIndex = e.Node.Index;

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.подразделениеBindingSource.EndEdit();
            this.подразделениеTableAdapter.Update(this.bdDataSet.Подразделение);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.должностьBindingSource.EndEdit();
            this.должностьTableAdapter.Update(this.bdDataSet.Должность);
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.сотрудникBindingSource.EndEdit();
            this.сотрудникTableAdapter.Update(this.bdDataSet.Сотрудник);
        }
    }
}
