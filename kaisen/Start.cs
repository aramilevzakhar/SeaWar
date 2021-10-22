using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kaisen
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.CenterToScreen();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("Type your name!");
            }
            else
            {
                gameForm gameForm = new gameForm();
                gameForm.label1.Text = string.Format("{0} vs Bot", textBox1.Text); 
                gameForm.Show();
                this.Hide();
            }
        }
    }
}
