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
    public partial class FrmDevolucao : Form
    {
        public List<Livro> ListaLivros { get; set; }
        public List<Usuarios> ListaUsuarios { get; set; }
        public List<Retirada> ListaRetirados { get; set; }
        public List<Reservas> ListaReservas { get; set; }
        public Usuarios Usuario { get; set; }

        public FrmDevolucao(Usuarios usuario)
        {
            InitializeComponent();

            this.Usuario = usuario;
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();

            CalcularMultaUsuario(ListaRetirados);

            if (this.Usuario.Tipo == "Cliente")
            {
                lblId.Visible = false;
                txtId.Visible = false;
                lblNomeUsuario.Visible = false;
                txtNomeUsuario.Visible = false;
                label2.Visible = false;
                panel2.Visible = false;
                dtGridUsuarios.Visible = false;
                panel1.Dock = DockStyle.Fill;

                PreencherDataGridViewLivro(Usuario);

                Usuario.MultaTotal = ListaRetirados
                    .Where(r => r.UsuarioID == Usuario.ID).Sum(r => r.Multa);
                if (Usuario.MultaTotal > 0)
                {
                    btnDevolver.Enabled = false;
                    MessageBox.Show($"Usuario {Usuario.Usuario}, Esta devendo uma Multa no valor de {Usuario.MultaTotal}\n" +
                        $"Pague para devolver os livros");
                }
                else
                    btnPagarMulta.Enabled = false;
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();

            Usuarios usuario = obterUsuario();

            if (usuario != null)
            {
                usuario.MultaTotal = ListaRetirados
                    .Where(r => r.UsuarioID == usuario.ID).Sum(r => r.Multa);
                if (usuario.MultaTotal > 0)
                {
                    btnDevolver.Enabled = false;
                    MessageBox.Show($"Usuario {usuario.Usuario}, Esta devendo uma Multa no valor de {usuario.MultaTotal}\n" +
                        $"Pague para devolver os livros");
                }
                txtNomeUsuario.Text = usuario.Usuario;
                PreencherDataGridViewLivro(usuario);
            }
            else
                txtNomeUsuario.Text = string.Empty;
        }
        private Usuarios obterUsuario()
        {
            int idUsuario;
            try
            {
                idUsuario = Convert.ToInt32(txtId.Text);

            }
            catch (FormatException)
            {
                idUsuario = 0;
            }
            var usuario = ListaUsuarios.FirstOrDefault(u => u.ID == idUsuario);

            return usuario;
        }

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            this.ListaUsuarios = Usuarios.GetUsuarios();
            if (txtNomeUsuario.Text.Length >= 3)
            {
                PreencherDataGridViewUsuario();
            }
            else if (txtNomeUsuario.Text.Length == 0)
            {
                LimparDadosUsuario();
            }
            else
            {
                dtGridLivros.DataSource = null;
            }
        }

        private void PreencherDataGridViewUsuario()
        {
            // Realiza a busca
            var listaUsuario = this.ListaUsuarios
                .Where(u => u.Usuario.ToLower().StartsWith(txtNomeUsuario.Text.ToLower()))
                .Select(u => new { u.ID, u.Usuario })
                .ToList();
            // Define a lista filtrada como a fonte de dados do DataGridView
            dtGridUsuarios.DataSource = listaUsuario;

            // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
            dtGridUsuarios.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dtGridUsuarios.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // Ajusta o AutoSizeMode da última coluna para Fill
            dtGridUsuarios.Columns[dtGridUsuarios.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void PreencherDataGridViewLivro(Usuarios usuario)
        {
            var retirados = ListaRetirados
                .Where(r => r.UsuarioID == usuario.ID)
                .Select(r => r.LivroID)
                .ToList();

            if (retirados.Any())
            {
                var listaLivros = ListaLivros.Where(l => retirados.Contains(l.ID))
                    .Select(l => new { l.ID, l.Nome, l.Autor, l.Status })
                    .ToList();

                dtGridLivros.DataSource = listaLivros;

                // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
                dtGridLivros.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dtGridLivros.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dtGridLivros.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dtGridLivros.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                // Ajusta o AutoSizeMode da última coluna para Fill
                dtGridLivros.Columns[dtGridLivros.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
                dtGridLivros.DataSource = null;
        }

        private void CalcularMultaUsuario(List<Retirada> retirados)
        {
            foreach (var retirado in retirados)
            {
                if (retirado.DataDevolucao < DateTime.Now)
                {
                    TimeSpan atraso = DateTime.Now - retirado.DataDevolucao;
                    int diasAtraso = (int)atraso.TotalDays;
                    double multa = 5 * Math.Pow(1 + 0.01, diasAtraso);
                    Retirada novaRetirada = new Retirada
                    {
                        ID = retirado.ID,
                        FuncionarioID = retirado.FuncionarioID,
                        UsuarioID = retirado.UsuarioID,
                        LivroID = retirado.LivroID,
                        DataRetirada = retirado.DataRetirada,
                        DataDevolucao = retirado.DataDevolucao,
                        Multa = retirado.Multa + multa
                    };
                    Retirada.UpdateRetirada(novaRetirada);
                }
            }
            this.ListaRetirados = Retirada.GetRetirados();
        }

        private void LimparDadosUsuario()
        {
            txtId.Text = string.Empty;
            txtNomeUsuario.Text = string.Empty;
            dtGridUsuarios.DataSource = null;
        }

        private void dtGridLivros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dtGridLivros.Rows[e.RowIndex];

                txtNomeLivro.Text = row.Cells["Nome"].Value.ToString();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(Usuario);
            frmPricipal.Show();
            this.Close();
        }

        private void dtGridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dtGridUsuarios.Rows[e.RowIndex];

                // Desabilitar o evento TextChanged
                txtId.TextChanged -= txtId_TextChanged;
                txtNomeUsuario.TextChanged -= txtNomeUsuario_TextChanged;

                txtId.Text = row.Cells["ID"].Value.ToString();
                txtNomeUsuario.Text = row.Cells["Usuario"].Value.ToString();

                // Reabilitar o evento TextChanged
                txtId.TextChanged += txtId_TextChanged;
                txtNomeUsuario.TextChanged += txtNomeUsuario_TextChanged;
            }
        }

        private void btnPagarMulta_Click(object sender, EventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();

            if (Usuario.Tipo == "Cliente")
            {
                Usuario.MultaTotal = 0;
                Usuarios.UpdateUsuario(Usuario);
                MostrarMensagem("Pagamento efetuado com sucesso.", "Multa", MessageBoxIcon.Information);
                return;
            }

            Usuarios usuario = obterUsuario();

            if (usuario != null)
            {
                usuario.MultaTotal = ListaRetirados
                    .Where(r => r.UsuarioID == usuario.ID).Sum(r => r.Multa);
                if (usuario.MultaTotal > 0)
                {
                    usuario.MultaTotal = 0;
                    Usuarios.UpdateUsuario(usuario);
                    var retirado = ListaRetirados
                    .Where(r => r.UsuarioID == usuario.ID);

                    retirado.First().Multa = 0;
                    Retirada.UpdateRetirada(retirado.First());
                    MostrarMensagem("Pagamento efetuado com sucesso.", "Multa", MessageBoxIcon.Information);
                    return;
                }
            }
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();

            btnDevolver.Enabled = true;

        }

        private void MostrarMensagem(string mensagem, string titulo, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, icone);
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();
            
            Usuarios usuario;
            var livro = ListaLivros.FirstOrDefault(l => l.Nome.ToLower().Contains(txtNomeLivro.Text.ToLower()));
            if (Usuario.Tipo != "Cliente")
                usuario = obterUsuario();
            else
            {
                usuario = Usuario;
            }

            if (usuario == null)
            {
                MostrarMensagem("Selecione um Usuario valido!", "Devolvar Livro", MessageBoxIcon.Warning);
                return;
            }

            if (livro == null)
            {
                MostrarMensagem("Selecione um livro valido!", "Devolvar Livro", MessageBoxIcon.Warning);
                return;
            }

            var retirado = ListaRetirados
                .Where(l => l.LivroID == livro.ID && l.UsuarioID == usuario.ID);
            if (retirado.Any())
            {
                var reserva = ListaReservas
                    .Where(r => r.LivroID == livro.ID).ToArray();
                usuario.LivrosRetirados -= 1;
                livro.Status = (reserva.Any()) ? "Reservado" : "Disponível";
                Usuarios.UpdateUsuario(usuario);
                Livro.UpdateLivro(livro);
                Retirada.DeleteRetirada(retirado.First());

                usuario = ListaUsuarios.FirstOrDefault(u => u.ID == reserva.First().UsuarioID);
                usuario.LivrosRetirados += 1;
                livro.Status = "Retirado";
                int id= UltimoIdRetirado() + 1;
                Retirada cadRetirada = new Retirada
                {
                    ID = id,
                    DataRetirada = DateTime.Now,
                    DataDevolucao = DateTime.Now.AddDays(7),
                    FuncionarioID = usuario.ID,
                    LivroID = livro.ID,
                    Multa = 0,
                    UsuarioID = usuario.ID
                };
                Usuarios.UpdateUsuario(usuario);
                Livro.UpdateLivro(livro);
                Retirada.AddRetirada(cadRetirada);

                MessageBox.Show("Devolucao efetuada com sucesso!");
            }

        }

        private int UltimoIdRetirado()
        {
            this.ListaRetirados = Retirada.GetRetirados();
            return (ListaRetirados.Count > 0) ? ListaRetirados.Max(u => u.ID) : 0;
        }

    }
}
