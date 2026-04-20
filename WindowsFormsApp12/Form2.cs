using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateDriver_Click(object sender, EventArgs e)
        {
            FormCreateDriver formCreateDriver = new FormCreateDriver();
            formCreateDriver.Show();

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnCards_Click(object sender, EventArgs e)
        {
            FormCardsDriver formCardsDriver = new FormCardsDriver();
            formCardsDriver.Show();
        }

        private void btnCreateVehicle_Click(object sender, EventArgs e)
        {
            FormCreateVehicle formCreateVehicle = new FormCreateVehicle();
            formCreateVehicle.Show();
        }

        private void btnFine_Click(object sender, EventArgs e)
        {
            FinesForm formFine = new FinesForm();
            formFine.Show();
        }
    }
}
