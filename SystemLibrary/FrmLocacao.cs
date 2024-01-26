using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemLibrary
{
    public partial class FrmLocacao : Form
    {
        public List<Livro> ListaLivros { get; set; }
        public List<Usuarios> ListaUsuarios { get; set; }
        public List<Retirada> ListaRetirados { get; set; }
        public List<Reservas> ListaReservas { get; set; }
        public Usuarios Usuario { get; set; }

        public FrmLocacao(Usuarios usuario)
        {
            InitializeComponent();

            this.Usuario = usuario;
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            this.ListaReservas = Reservas.GetReservas();
            PreencherDataGridViewLivro(true);

            if (this.Usuario.Tipo == "Cliente")
            {
                lblId.Visible = false;
                txtId.Visible = false;
                lblNomeUsuario.Visible = false;
                txtNomeUsuario.Visible = false;
                label2.Visible = false;
                panel2.Visible = false;
                panel1.Dock = DockStyle.Fill;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal(Usuario);
            frmPricipal.Show();
            this.Close();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            this.ListaUsuarios = Usuarios.GetUsuarios();

            Usuarios usuario = obterUsuario();

            if (usuario != null)
            {
                txtNomeUsuario.Text = usuario.Usuario;
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

        private void txtNomeLivro_TextChanged(object sender, EventArgs e)
        {
            this.ListaLivros = Livro.GetLivros();
            if (txtNomeLivro.Text.Length >= 3)
            {
                PreencherDataGridViewLivro();
            }
            else
                PreencherDataGridViewLivro(true);

            var livro = ListaLivros.FirstOrDefault(l => l.Nome.ToLower() == txtNomeLivro.Text.ToLower());

            if (livro != null)
            {
                btnRetirada.Enabled = (livro.Status == "Disponível") ? true : false;
                btnReserva.Enabled = (livro.Status == "Disponível") ? false : true;
            }
            else
            {
                btnRetirada.Enabled = true;
                btnReserva.Enabled = true; 
            }
        }

        private void PreencherDataGridViewLivro(bool todosLivros = false) 
        {
            var listaLivros = this.ListaLivros
                    .Select(l => new { l.ID, l.Nome, l.Autor, l.Status })
                    .ToList();
            if (!todosLivros)
            {
                listaLivros = this.ListaLivros
                    .Where(l => l.Nome.ToLower().StartsWith(txtNomeLivro.Text.ToLower()))
                    .Select(l => new { l.ID, l.Nome, l.Autor, l.Status })
                    .ToList();
            }

                

            dtGridLivros.DataSource = listaLivros;

            // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
            dtGridLivros.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dtGridLivros.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dtGridLivros.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dtGridLivros.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // Ajusta o AutoSizeMode da última coluna para Fill
            dtGridLivros.Columns[dtGridLivros.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void LimparDados()
        {
            txtId.Text = string.Empty;
            txtNomeUsuario.Text = string.Empty;
            txtNomeLivro.Text = string.Empty;
            PreencherDataGridViewLivro(true);
            dtGridUsuarios.DataSource = null;
        }

        private void PreencherDataGridViewUsuario()
        {
            // Realiza a busca
            var listaUsuario = this.ListaUsuarios
                .Where(u => u.Usuario.ToLower().StartsWith(txtNomeUsuario.Text.ToLower()))
                .Select(u => new { u.ID, u.Usuario})
                .ToList();
            // Define a lista filtrada como a fonte de dados do DataGridView
            dtGridUsuarios.DataSource = listaUsuario;

            // Ajusta o AutoSizeMode das primeiras colunas para DisplayedCells
            dtGridUsuarios.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dtGridUsuarios.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // Ajusta o AutoSizeMode da última coluna para Fill
            dtGridUsuarios.Columns[dtGridUsuarios.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LimparDadosUsuario()
        {
            txtId.Text = string.Empty;
            txtNomeUsuario.Text = string.Empty;
            dtGridUsuarios.DataSource = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dtGridLivros.Rows[e.RowIndex];

                txtNomeLivro.Text = row.Cells["Nome"].Value.ToString();
            }
        }

        private bool verificaSeLivroRetirado(Usuarios usuario)
        {
            var retirados = ListaRetirados
                .Where(r => r.UsuarioID == usuario.ID)
                .Select(r => new {r.ID, r.FuncionarioID, r.LivroID,
                    r.UsuarioID, r.DataRetirada, r.DataDevolucao, r.Multa }).ToList();

            if (retirados !=  null)
            {
                int numRetirados = retirados.Count;
                if (numRetirados < 3)
                    return true;
                return false;
            }
            return true;
        }

        private double verificaSeUsuarioMulta(Usuarios usuario)
        {
            var retirados = ListaRetirados
                .Where(r => r.UsuarioID == usuario.ID)
                .Select(r => new {
                    r.ID,
                    r.FuncionarioID,
                    r.LivroID,
                    r.UsuarioID,
                    r.DataRetirada,
                    r.DataDevolucao,
                    r.Multa
                }).ToList();
            if (retirados != null)
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
                usuario.MultaTotal = ListaRetirados.Where(r => r.UsuarioID == usuario.ID).Sum(r => r.Multa);
            }
            return usuario.MultaTotal;
        }


        private void MostrarMensagem(string mensagem, string titulo, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, icone);
        }

        private void btnRetirada_Click(object sender, EventArgs e)
        {
            ListaLivros = Livro.GetLivros();
            ListaUsuarios = Usuarios.GetUsuarios();
            ListaRetirados = Retirada.GetRetirados();

            Usuarios usuario;
            var livro = ListaLivros.FirstOrDefault(l => l.Nome.ToLower() == txtNomeLivro.Text.ToLower());
            if (Usuario.Tipo != "Cliente")
                usuario = obterUsuario();
            else
            {
                usuario = Usuario;
            }

            if (usuario == null)
            {
                MostrarMensagem("Selecione um Usuario valido!", "Emprestimo Livro", MessageBoxIcon.Warning);
                return;
            }

            if (livro == null)
            {
                MostrarMensagem("Selecione um livro valido!", "Emprestimo Livro", MessageBoxIcon.Warning);
                return;
            }

            if (!verificaSeLivroRetirado(usuario))
            {
                MostrarMensagem("Ja foi atingido sua cota de livros", "Emprestimo Livro", MessageBoxIcon.Warning);
                return;
            }
            var retirado = ListaRetirados
                .Where(r => r.LivroID == livro.ID)
                .Where(r => r.UsuarioID == usuario.ID)
                .ToList();
            if (retirado.Any())
            {
                MostrarMensagem("O livro ja esta em posse !", "Emprestimo Livro", MessageBoxIcon.Warning);
                return;
            }


            double multa = verificaSeUsuarioMulta(usuario);
            if (multa > 0)
            {
                MostrarMensagem($"Usuario Possui Multa de {multa}, Devolva os livros e pague a multa",
                    "Emprestimo Livro", MessageBoxIcon.Warning);
                Usuarios.UpdateUsuario(usuario);
                return;
            }
            int ultimoId;
            try
            {
                ultimoId = ListaRetirados.Max(u => u.ID);
                ultimoId += 1;
            }
            catch { ultimoId = 1; }
            Retirada retirada = new Retirada
            {
                ID = ultimoId,
                DataRetirada = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(7),
                FuncionarioID = Usuario.ID,
                UsuarioID = usuario.ID,
                LivroID = livro.ID,
                Multa = 0
            };
            if (Retirada.AddRetirada(retirada))
            {
                usuario.LivrosRetirados += 1;
                livro.Status = "Retirado";
                Livro.UpdateLivro(livro);
                Usuarios.UpdateUsuario(usuario);
                MostrarMensagem($"Usuario {usuario.Usuario}, Retirou Livro com Sucesso.",
                    "Emprestimo Livro", MessageBoxIcon.Information);
            }
            else
            {
                MostrarMensagem($"Usuario {usuario.Usuario}, Nao foi possivel retirar.",
                    "Emprestimo Livro", MessageBoxIcon.Error);
            }
            this.ListaLivros = Livro.GetLivros();
            this.ListaUsuarios = Usuarios.GetUsuarios();
            this.ListaRetirados = Retirada.GetRetirados();
            LimparDados();
        }

        private void btnReserva_Click(object sender, EventArgs e)
        {
            ListaLivros = Livro.GetLivros();
            ListaUsuarios = Usuarios.GetUsuarios();
            ListaRetirados = Retirada.GetRetirados();
            ListaReservas = Reservas.GetReservas();

            Usuarios usuario;
            var livro = ListaLivros.FirstOrDefault(l => l.Nome.ToLower() == txtNomeLivro.Text.ToLower());
            if (Usuario.Tipo != "Cliente")
                usuario = obterUsuario();
            else
            {
                usuario = Usuario;
            }


            if (usuario == null)
            {
                MostrarMensagem("Selecione um Usuario valido!", "Reserva Livro", MessageBoxIcon.Warning);
                return;
            }

            if (livro == null)
            {
                MostrarMensagem("Selecione um livro valido!", "Reserva Livro", MessageBoxIcon.Warning);
                return;
            }

            var retirados = ListaRetirados
                .Where(r => r.LivroID == livro.ID && r.UsuarioID == usuario.ID).ToList();
            var resevados = ListaReservas
                .Where(r => r.LivroID == livro.ID && r.UsuarioID == usuario.ID).ToList();
            if (retirados.Any())
            {
                MostrarMensagem("Livro ja esta em posse!", "Reserva Livro", MessageBoxIcon.Warning);
                return;
            }
            if (resevados.Any())
            {
                MostrarMensagem($"Usuario {usuario.Usuario}, ja tem reserva!", "Reserva Livro", MessageBoxIcon.Warning);
                return;
            }

            livro.Status = "Reservado";
            int idReservado = UltimoIdReserva() + 1;
            Reservas reservas = new Reservas
            {
                ID = idReservado,
                FuncionarioID = Usuario.ID,
                UsuarioID = usuario.ID,
                LivroID = livro.ID
            };
            Reservas.InsertReserva( reservas );
            Livro.UpdateLivro( livro );
            MostrarMensagem($"Usuario {usuario.Usuario}, reservou com sucesso!", "Reserva Livro", MessageBoxIcon.Warning);

        }

        private int UltimoIdReserva()
        {
            return (ListaReservas.Count > 0) ? ListaReservas.Max(r => r.ID) : 0;
        }
    }
}
