using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace work
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            timer1.Start();
        }



        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(6);

            if (progressBar1.Value == progressBar1.Maximum)
            {
                timer1.Stop();
                progressBar1.Visible = false;
                button1.Visible = true;
                buttonClose.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainfor = new MainForm();
            mainfor.ShowDialog();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
