using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SystemLibrary
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (vericaSeEstaPreenchido())
            {
                List<Usuarios> listaUsuario = Usuarios.GetUsuarios();
                var usuario = listaUsuario.FirstOrDefault(u => u.Usuario == txtUsuario.Text.ToLower() && u.Senha == txtSenha.Text.ToLower());
                if (verificaSeListaUsuarioEstaVazia(listaUsuario))
                {
                    Usuarios usuarioCad = new Usuarios
                    {
                        ID = 1,
                        Usuario = txtUsuario.Text.ToLower(),
                        Senha = txtSenha.Text.ToLower(),
                        LivrosRetirados = 0,
                        MultaTotal = 0,
                        Reservas = 0,
                        Tipo = "Admin"
                    };
                    Usuarios.AddUsuario(usuarioCad);
                    listaUsuario.Add(usuarioCad);
                    FrmPricipal principal = new FrmPricipal(usuarioCad);
                    principal.Show();
                    this.Visible = false;
                    this.Close();
                }
                else if (usuario != null)
                {
                    FrmPricipal principal = new FrmPricipal(usuario);
                    principal.Show();
                    this.Visible = false;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuarío ou Senha Inválidos",
                                    "Ocorreu um Erro ao Autenticar",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private bool verificaSeListaUsuarioEstaVazia(List<Usuarios> usuarios)
        {
            if (usuarios.Count == 0)
            {
                return true;
            }
            return false;
        }

        private bool vericaSeEstaPreenchido()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Usuário ou senha não podem estar vazios",
                        "Erro de autenticação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
