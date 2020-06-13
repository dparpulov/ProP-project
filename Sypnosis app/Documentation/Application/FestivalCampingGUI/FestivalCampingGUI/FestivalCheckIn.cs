using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FestivalCampingGUI
{
    public partial class FestivalCheckIn : Form
    {
        public FestivalCheckIn()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form1 checkOut = new Form1();
            checkOut.ShowDialog();
            this.Close();
        }
    }
}
