using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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

                    MostrarMensagem("Realizado com Sucesso", "Cadastro", MessageBoxIcon.Information);
                }
                else
                {
                    MostrarMensagem(
                        "Falha ao Realizar o Cadastro, Verifique se o nome do usuario ja não foi usado!!",
                        "Cadastro", 
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

                // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                // Ajusta o AutoSizeMode da última coluna para Fill
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
                
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

        private void button1_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(this.ListaUsuarios, this.Usuario);
            frmPricipal.Visible = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox3.Text);
            var usuario = ListaUsuarios.FirstOrDefault(u => u.ID == id);

            if (usuario == null)
            {
                MostrarMensagem("Selecione um Usuario valido", "Atualizacao de Cadastro", MessageBoxIcon.Information);
                return;
            }

            bool podeEditar = (this.Usuario.Tipo == "Funcionario" && (usuario.Tipo == "Funcionario" || usuario.Tipo == "Cliente"))
                              || this.Usuario.Tipo == "Admin";

            if (podeEditar)
            {
                AtualizarUsuario(usuario);
            }
            else
            {
                MostrarMensagem("Você Nao tem perimicao para alterar o registro", "Atualizacao de Cadastro", MessageBoxIcon.Error);
            }

            this.ListaUsuarios = Usuarios.GetUsuarios();
            LimparCampos();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox3.Text);
            var usuario = ListaUsuarios.FirstOrDefault(u => u.ID == id);

            if (usuario == null)
            {
                MostrarMensagem("Selecione um Usuario valido", "Remover Cadastro", MessageBoxIcon.Information);
                return;
            }

            bool podeRemover = (this.Usuario.Tipo == "Funcionario" && (usuario.Tipo == "Funcionario" || usuario.Tipo == "Cliente"))
                              || this.Usuario.Tipo == "Admin";

            if (podeRemover)
            {
                RemoverUsuario(usuario);
            }
            else
            {
                MostrarMensagem("Você Nao tem perimicao para remover o registro", "Remover Cadastro", MessageBoxIcon.Error);
            }

            this.ListaUsuarios = Usuarios.GetUsuarios();
            LimparCampos();
        }

        private void MostrarMensagem(string mensagem, string titulo, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, icone);
        }

        private void RemoverUsuario(Usuarios usuario)
        {
            if (Usuarios.RemoveUsuario(usuario))
            {
                MostrarMensagem("Removido com sucesso", "Remover Cadastro", MessageBoxIcon.Information);
            }
            else
            {
                MostrarMensagem("Falha ao remover usuario", "Remover Cadastro", MessageBoxIcon.Error);
            }
        }

        private void AtualizarUsuario(Usuarios usuario)
        {
            usuario.Usuario = textBox1.Text;
            usuario.Senha = textBox2.Text;
            usuario.Tipo = comboBox1.Text;

            if (Usuarios.UpdateUsuario(usuario))
            {
                MostrarMensagem("Atualizado com sucesso", "Atualizacao de Cadastro", MessageBoxIcon.Information);
            }
            else
            {
                MostrarMensagem("Falha ao atualizar usuario", "Atualizacao de Cadastro", MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            textBox3.Clear();
            textBox1.Clear();
            textBox2.Clear();
        }

    }
}
