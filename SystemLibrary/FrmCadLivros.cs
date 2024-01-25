using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SystemLibrary
{
    public partial class FrmCadLivros : Form
    {

        public List<Livro> ListaLivros { get; set; }

        public Usuarios Usuario { get; set; }

        public FrmCadLivros(Usuarios usuario)
        {
            this.Usuario = usuario;
            this.ListaLivros = Livro.GetLivros();

            InitializeComponent();

            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Disponível");
            cbStatus.SelectedIndex = 0;

            cbStatus1.Enabled = false;

            if (!verificaSeListaLivroEstaVazia(this.ListaLivros)){
                int ultimoId = ListaLivros.Max(u => u.ID);
                lblNumId.Text = (ultimoId + 1).ToString();
            }
            else
                lblNumId.Text = "1";
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(this.Usuario);
            frmPricipal.Visible = true;
            this.Close();

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            int idLivro = Convert.ToInt32(lblNumId.Text);
            string nomeLivro = txtNome.Text;
            string nomeAutor = txtAutor.Text;
            DateTime dtLancamentoLivro = dtLancamento.Value;
            string statusLivro = cbStatus.Text;
            string sinopseLivro = txtSinopse.Text;

            if (!validaCampo(nomeLivro, 8, 60))
            {
                MostrarMensagem("O nome do Livro tem que no minimo 8 caracter e no maximo 60", "Cadastro", MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (!validaCampo(nomeAutor, 8, 60))
            {
                MostrarMensagem("O nome do Autor tem que no minimo 8 caracter e no maximo 60", "Cadastro", MessageBoxIcon.Warning);
                txtAutor.Focus();
                return;
            }

            if (!validaCampo(sinopseLivro, 10, 1500))
            {
                MostrarMensagem("A sinopse do Livro tem que no minimo 10 caracter e no maximo 1500", "Cadastro", MessageBoxIcon.Warning);
                txtSinopse.Focus();
                return;
            }
            if (dtLancamentoLivro > DateTime.Now)
            {
                MostrarMensagem("A data de Lancamento nao pode ser maior que a Data atual", "Cadastro", MessageBoxIcon.Warning);
                return;
            }

            Livro livro = new Livro
            {
                ID = idLivro,
                Nome = nomeLivro,
                Autor = nomeAutor,
                DataLancamento = dtLancamentoLivro,
                Reservas = 0,
                Sinopse = sinopseLivro,
                Status = statusLivro
            };
            if (Livro.AddLivro(livro))
            {
                MostrarMensagem("Livro castrado com sucesso", "Cadastro", MessageBoxIcon.Information);
                txtNome.Text = string.Empty;
                txtAutor.Text = string.Empty;
                txtSinopse.Text = string.Empty;
            }
            else
                MostrarMensagem("Falha ao castrado o livro", "Cadastro", MessageBoxIcon.Error);

            this.ListaLivros = Livro.GetLivros();
            int ultimoId = ListaLivros.Max(u => u.ID);
            lblNumId.Text = (ultimoId + 1).ToString();
        }

        private void MostrarMensagem(string mensagem, string titulo, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, icone);
        }

        public void PreencherDataGridView()
        {
            // Realiza a busca
            var livrosFiltrados = this.ListaLivros
                .Where(l => l.Nome.ToLower().StartsWith(txtNome1.Text.ToLower()))
                .Select(l => new { l.ID, l.Nome, l.Autor })
                .ToList();
            // Define a lista filtrada como a fonte de dados do DataGridView
            dataGridView1.DataSource = livrosFiltrados;

            // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // Ajusta o AutoSizeMode da última coluna para Fill
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private bool validaCampo(string campo, int min, int max)
        {
            if (campo.Length >= min && campo.Length <= max)
                return true;
            return false;
        }

        private bool verificaSeListaLivroEstaVazia(List<Livro> livro)
        {
            if (livro.Count == 0)
            {
                return true;
            }
            return false;
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo aceita somente números");
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            
            Livro livro = obterLivro();

            if (livro != null)
            {
                txtNome1.Text = livro.Nome;
                txtAutor1.Text = livro.Autor;
                txtSinopse1.Text = livro.Sinopse;
                dtLancamento1.Value = livro.DataLancamento;
                cbStatus1.Text = livro.Status;
            }
            else
            {
                txtNome1.Text = string.Empty;
                txtAutor1.Text = string.Empty;
                txtSinopse1.Text = string.Empty;
                dtLancamento1.Value = DateTime.Now;
                cbStatus1.Text = string.Empty;
            }
        }

        private void btnCancelar1_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(this.Usuario);
            frmPricipal.Visible = true;
            this.Close();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Livro livro = obterLivro();

            if (livro == null)
            {
                MostrarMensagem("Selecione um Livro valido", "Atualizacao de Cadastro", MessageBoxIcon.Information);
                return;
            }
            else
            {
                // Atualizar Livro
                AtualizarLivro(livro);
            }
            this.ListaLivros = Livro.GetLivros();
            LimparDadosTela();
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            Livro livro = obterLivro();

            if (livro == null)
            {
                MostrarMensagem("Selecione um Livro valido", "Remover de Cadastro", MessageBoxIcon.Information);
                return;
            }
            else
            {
                // Atualizar Livro
                RemoverLivro(livro);
            }
            this.ListaLivros = Livro.GetLivros();
            LimparDadosTela();
        }

        private void txtAutor1_TextChanged(object sender, EventArgs e)
        {
            if (txtAutor1.Text.Length >= 3)
            {
                PreencherDataGridView();
            } else if (txtAutor1.Text.Length == 0)
            {
                LimparDadosTela();
            }
            else
            {
                dataGridView1.DataSource = null;
            }

            this.ListaLivros = Livro.GetLivros();
        }

        private void txtNome1_TextChanged(object sender, EventArgs e)
        {
            if (txtNome1.Text.Length >= 3)
            {
                PreencherDataGridView();
            }
            else if (txtNome1.Text.Length == 0)
            {
                LimparDadosTela();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            if (e.RowIndex >= 0)
            {
                // Mova a criação da lista de nomes de colunas para aqui
                List<string> columnNames = new List<string>();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    columnNames.Add(column.Name);
                }

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Desabilitar o evento TextChanged
                txtNome1.TextChanged -= txtNome1_TextChanged;
                txtId.TextChanged -= txtId_TextChanged;

                txtId.Text = row.Cells["ID"].Value.ToString();
                txtNome1.Text = row.Cells["Nome"].Value.ToString();
                txtAutor1.Text = row.Cells["Autor"].Value.ToString();

                // Reabilitar o evento TextChanged
                txtNome1.TextChanged += txtNome1_TextChanged;
                txtId.TextChanged += txtId_TextChanged;

                var livro = ListaLivros.FirstOrDefault(l => l.ID == int.Parse(txtId.Text));
                if (livro != null)
                {
                    dtLancamento1.Value = livro.DataLancamento;
                    cbStatus1.Text = livro.Status;
                    txtSinopse1.Text = livro.Sinopse;
                }
            }
        }
        
        private void LimparDadosTela()
        {
            txtId.Text = string.Empty;
            txtNome1.Text = string.Empty;
            txtAutor1.Text = string.Empty;
            txtSinopse1.Text = string.Empty;
            cbStatus.Text = string.Empty;
            dtLancamento1.Value = DateTime.Now;
            dataGridView1.DataSource = null;

            txtNome.Text = string.Empty;
            txtAutor.Text = string.Empty;
            txtSinopse.Text = string.Empty;
            dtLancamento1.Value = DateTime.Now;
            dataGridView1.DataSource = null;

            this.ListaLivros = Livro.GetLivros();
            if (!verificaSeListaLivroEstaVazia(this.ListaLivros))
            {
                int ultimoId = ListaLivros.Max(u => u.ID);
                lblNumId.Text = (ultimoId + 1).ToString();
            }
            else
                lblNumId.Text = "1";
        }
    
        private void AtualizarLivro(Livro livro)
        {
            livro.ID = int.Parse(txtId.Text);
            livro.Nome = txtNome1.Text;
            livro.Autor = txtAutor1.Text;
            livro.DataLancamento = dtLancamento1.Value;
            livro.Status = cbStatus1.Text;
            livro.Sinopse = txtSinopse1.Text;

            if (Livro.UpdateLivro(livro))
                MostrarMensagem("Atualizado com sucesso", "Atualizacao de Cadastro", MessageBoxIcon.Information);
            else
                MostrarMensagem("Falha ao atualizar livro", "Atualizacao de Cadastro", MessageBoxIcon.Error);
        }

        private void RemoverLivro(Livro livro)
        {
            if (livro.Status == "Disponível")
            {
                if (Livro.RemoveLivro(livro))
                    MostrarMensagem("Removido com sucesso", "Remoção de Cadastro", MessageBoxIcon.Information);
                else
                    MostrarMensagem("Falha ao remover livro", "Remoção de Cadastro", MessageBoxIcon.Error);
            }
            else
                MostrarMensagem("Livro so pode ser removido quando o Status tiver Disponível", "Remoção de Cadastro", MessageBoxIcon.Error);
        }

        private Livro obterLivro()
        {
            int idLivro;
            try
            {
                idLivro = Convert.ToInt32(txtId.Text);

            }
            catch (FormatException)
            {
                idLivro = 0;
            }
            var livro = ListaLivros.FirstOrDefault(l => l.ID == idLivro);

            return livro;
        }
    }
}
