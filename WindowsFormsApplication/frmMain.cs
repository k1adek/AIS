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
    public partial class frmMain : Form
    {
        string idworker, ruleworker, fioworker;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bdDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.bdDataSet.Сотрудник);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.сотрудникTableAdapter.Update(this.bdDataSet.Сотрудник);

            bindingSource1.Filter = "Логин='" + textBox1.Text + "' and Пароль='" + textBox2.Text + "'";

            textBox1.Clear();
            textBox2.Clear();

            if (bindingSource1.Count == 0)
            {
                MessageBox.Show("Введена не верная пара логин/пароль");
                return;
            }
/*
            textBox1.Hide();
            textBox2.Hide();
            button1.Hide();
            */
            DataRowView drw = bindingSource1.Current as DataRowView;
            idworker = drw["СотрудникНомер"].ToString();
            ruleworker = drw["ПраваДоступа"].ToString();
            fioworker = drw["Сотрудник"].ToString();

            if (ruleworker == "Администратор")
            {
                frmAdminMode newForm = new frmAdminMode();
                newForm.Text = fioworker + ". Режим администратора";
                newForm.ShowDialog();               
                return;
            }

            if (ruleworker == "Исполнитель")
            {
                frmWorkerMode newForm = new frmWorkerMode();
                newForm.Text = fioworker + ". Режим исполнителя";
                newForm.idworker = idworker;
                newForm.ShowDialog();
                return;
            }

            if (ruleworker == "Секретарь")
            {
                frmManagerMode newForm = new frmManagerMode();
                newForm.Text = fioworker + ". Режим секретаря";
                newForm.idworker = idworker;
                newForm.ShowDialog();
                return;
            }

            if (ruleworker == "Начальник")
            {
                frmChiefMode newForm = new frmChiefMode();
                newForm.Text = fioworker + ". Режим начальника";
                newForm.idworker = idworker;
                newForm.ShowDialog();
                return;
            }

        }

    }
}
