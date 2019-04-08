using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumValidator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                byte[] buffer1 = File.ReadAllBytes(textBox1.Text);
                string hash1 = BitConverter.ToString(cryptoProvider.ComputeHash(buffer1));

                byte[] buffer2 = File.ReadAllBytes(textBox2.Text);
                string hash2 = BitConverter.ToString(cryptoProvider.ComputeHash(buffer2));

                if (hash1 == hash2)
                {
                    MessageBox.Show("Equal!");
                }
                else
                {
                    MessageBox.Show("Not Equal!");
                }
            }


        }
    }
}
