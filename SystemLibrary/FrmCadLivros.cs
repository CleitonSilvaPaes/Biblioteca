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

            if (!verificaSeListaLivroEstaVazia(this.ListaLivros)){
                int ultimoId = ListaLivros.Max(u => u.ID);
                lblNumId.Text = (ultimoId + 1).ToString();
            }
            else
                lblNumId.Text = "1";
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal();
            frmPricipal.Visible = true;
            this.Close();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal();
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
            Livro.AddLivro(livro);
            this.ListaLivros = Livro.GetLivros();
        }

        private void MostrarMensagem(string mensagem, string titulo, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, icone);
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
            int idLivro = Convert.ToInt32(txtId.Text);
            var livro = ListaLivros.FirstOrDefault(u => u.ID == idLivro);

            if (livro != null)
            {
                txtNome1.Text = livro.Nome;
                txtAutor1.Text = livro.Autor;
                txtSinopse.Text = livro.Sinopse;
                dtLancamento1.Value = livro.DataLancamento;
            }
        }
    }
}
