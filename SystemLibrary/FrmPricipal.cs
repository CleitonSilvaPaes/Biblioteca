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
        public Usuarios Usuario { get; set; }

        public FrmPricipal(Usuarios usuario)
        {
            InitializeComponent();
            this.Usuario = usuario;

            if (this.Usuario.Tipo == "Cliente")
            {
                cadastroToolStripMenuItem.Visible = false;
            }

            toolStripStatusLabel4.Text = $"Logado como {usuario.Tipo}";
            toolStripStatusLabel1.Text = $"Bem vindo (Sr/Sra){usuario.Usuario.ToUpper()} !";
        }

        public FrmPricipal()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToShortDateString();
            toolStripStatusLabel3.Text = DateTime.Now.ToShortTimeString();
        }

        private void FrmPricipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LivroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmCadLivros frmCadLivros = new FrmCadLivros(Usuario);
            frmCadLivros.Show();
        }

        private void UsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmCadUsuario frmCadUsuario = new FrmCadUsuario(Usuario);
            frmCadUsuario.Show();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.Show();
            this.Visible = false;
        }

        private void locacaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocacao frmLocacao = new FrmLocacao(Usuario);
            frmLocacao.Show();
            this.Visible = false;
        }

        private void devolucaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDevolucao frmDevolucao = new FrmDevolucao(Usuario);
            frmDevolucao.Show();
            this.Visible = false;
        }
    }
}
