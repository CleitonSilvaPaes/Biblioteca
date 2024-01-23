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
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            if (Usuario.Tipo == "Funcionario")
            {
                cbTipo.Items.Add("Funcionario");
                cbTipo.Items.Add("Cliente");
                comboBox1.Items.Add("Funcionario");
                comboBox1.Items.Add("Cliente");
            }
            else if (Usuario.Tipo == "Admin")
            {
                cbTipo.Items.Add("Admin");
                cbTipo.Items.Add("Funcionario");
                cbTipo.Items.Add("Cliente");
                comboBox1.Items.Add("Admin");
                comboBox1.Items.Add("Funcionario");
                comboBox1.Items.Add("Cliente");
            }

            cbTipo.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(this.ListaUsuarios, this.Usuario);
            frmPricipal.Visible = true;
            this.Close();
        }

        private bool validaCad(string nomeUsuario, string senha)
        {
            if (nomeUsuario.Length < 8 && senha.Length < 8)
            {
                MessageBox.Show("Usuario e Senha tem que ter no minimo 8 caracter",
                    "Cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            int idUsu = Convert.ToInt32(lblNumId.Text);
            string txtUsu = txtUsuario.Text;
            string txtSen = txtSenha.Text;
            string txtTipo = cbTipo.Text;

            if (validaCad(txtUsu, txtSen))
            {
                if (Usuarios.AddUsuario(idUsu,
                    txtUsu,
                    txtSen,
                    txtTipo))
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

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Realiza a busca
                PreencherCamposFormulario(int.Parse(textBox3.Text));
            }
        }

        public void PreencherCamposFormulario(int id)
        {
            // Pesquisa o usuário na lista de usuários
            var usuario = ListaUsuarios.FirstOrDefault(u => u.ID == id);

            // Se o usuário for encontrado, preenche os campos do formulário
            if (usuario != null)
            {
                textBox3.Text = usuario.ID.ToString();
                textBox1.Text = usuario.Usuario;
                textBox2.Text = usuario.Senha;
                comboBox1.Text = usuario.Tipo;
            }
            else
            {
                // Limpa os campos do formulário se o usuário não for encontrado
                textBox3.Clear();
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = 0;
            }
        }

        public void PreencherDataGridView()
        {
            // Cria uma nova lista com apenas as propriedades que você deseja exibir
            var usuariosParaExibir = ListaUsuarios.Select(u => new { u.ID, u.Usuario }).ToList();

            // Define a nova lista como a fonte de dados do DataGridView
            dataGridView1.DataSource = usuariosParaExibir;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 3)
            {
                // Realiza a busca
                var usuariosFiltrados = ListaUsuarios
                    .Where(u => u.Usuario.StartsWith(textBox1.Text))
                    .Select(u => new { u.ID, u.Usuario })
                    .ToList();

                // Define a lista filtrada como a fonte de dados do DataGridView
                dataGridView1.DataSource = usuariosFiltrados;
            }
            else
                dataGridView1.DataSource = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textBox3.Text = row.Cells["ID"].Value.ToString();
                textBox1.Text = row.Cells["Usuario"].Value.ToString();

                var usuario = ListaUsuarios.FirstOrDefault(u => u.ID == int.Parse(textBox3.Text));
                if (usuario != null)
                {
                    textBox2.Text = usuario.Senha;
                    comboBox1.Text = usuario.Tipo;
                }
            }
        }
    }
}
