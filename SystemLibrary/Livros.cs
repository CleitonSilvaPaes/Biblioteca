using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary
{
    public class Livro
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public string Sinopse { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Status { get; set; }
        public int Reservas { get; set; }

        public Livro(){ }


        public static List<Livro> GetLivros()
        {
            List<Livro> listaLivros = new List<Livro>();
            using (var conexao = Banco.ConexaoDB())
            {
                conexao.Open();
                using (var comando = new SQLiteCommand("SELECT * FROM Livros", conexao))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Livro livro = new Livro
                            {
                                ID = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Autor = reader.GetString(2),
                                Sinopse = reader.GetString(3),
                                DataLancamento = reader.GetDateTime(4),
                                Status = reader.GetString(5),
                                Reservas = reader.GetInt32(6)
                            };
                            listaLivros.Add(livro);
                        }
                    }
                }
            }
            return listaLivros;
        }

        public static bool AddLivro(Livro livro)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var comando = new SQLiteCommand(conexao))
                    {
                        comando.CommandText = "INSERT INTO Livros (ID, Nome, Autor, Sinopse, DataLancamento, Status, Reservas) VALUES (@ID, @Nome, @Autor, @Sinopse, @DataLancamento, @Status, @Reservas)";
                        comando.Parameters.AddWithValue("@ID", livro.ID);
                        comando.Parameters.AddWithValue("@Nome", livro.Nome);
                        comando.Parameters.AddWithValue("@Autor", livro.Autor);
                        comando.Parameters.AddWithValue("@Sinopse", livro.Sinopse);
                        comando.Parameters.AddWithValue("@DataLancamento", livro.DataLancamento);
                        comando.Parameters.AddWithValue("@Status", livro.Status);
                        comando.Parameters.AddWithValue("@Reservas", livro.Reservas);
                        comando.ExecuteNonQuery();
                    }
                }
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }

}
