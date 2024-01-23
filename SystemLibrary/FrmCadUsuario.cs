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
    public partial class FrmCadUsuario : Form
    {
        public List<Usuarios> ListaUsuarios { get; set; }
        public Usuarios Usuario { get; set; }

        public FrmCadUsuario(Usuarios usuario)
        {
            InitializeComponent();

            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.Usuario = usuario;
            int ultimoId = ListaUsuarios.Max(u => u.ID);

            lblNumId.Text = (ultimoId + 1).ToString();

            cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;

            cbTipo.SelectedIndex = 0;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(this.ListaUsuarios, this.Usuario);
            frmPricipal.Visible = true;
            this.Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            int idUsu = Convert.ToInt32(lblNumId.Text);
            string txtUsu = txtUsuario.Text;
            string txtSen = txtSenha.Text;
            string txtTipo = cbTipo.Text;
            Usuarios usuarioCad = new Usuarios
            {
                ID = idUsu,
                Usuario = txtUsu,
                Senha = txtSen,
                Tipo = txtTipo
            };
            if (Usuarios.AddUsuario(usuarioCad.ID,
                usuarioCad.Usuario,
                usuarioCad.Senha,
                usuarioCad.Tipo))
            {
                txtUsuario.Text = string.Empty;
                txtSenha.Text = string.Empty;

                this.ListaUsuarios = Usuarios.GetUsuarios();
                int ultimoId = ListaUsuarios.Max(u => u.ID);

                lblNumId.Text = (ultimoId + 1).ToString();

                MessageBox.Show("Realizado com Sucesso",
                    "Cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Falha ao Realizar o Cadastro, Verifique se o nome do usuario ja não foi usado!!",
                    "Cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

        }
    }
}
