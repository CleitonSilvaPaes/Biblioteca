using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemLibrary
{
    public partial class FrmPricipal : Form
    {
        public FrmPricipal()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToShortDateString();
            toolStripStatusLabel3.Text = DateTime.Now.ToShortTimeString();
        }

        private void FrmPricipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void livroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmCadLivros frmCadLivros = new FrmCadLivros();
            frmCadLivros.Show();
        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmCadUsuario frmCadUsuario = new FrmCadUsuario();
            frmCadUsuario.ShowDialog();
        }
    }
}
