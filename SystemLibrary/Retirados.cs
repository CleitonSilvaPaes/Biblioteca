using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary
{
    public class Retirada
    {
        public int ID { get; set; }
        public int FuncionarioID { get; set; }
        public int UsuarioID { get; set; }
        public int LivroID { get; set; }
        public DateTime DataRetirada { get; set; }
        public DateTime DataDevolucao { get; set; }
        public double Multa { get; set; }

        public static List<Retirada> GetRetirados()
        {
            var listaRetiradas = new List<Retirada>();

            using (var conexao = Banco.ConexaoDB())
            {
                conexao.Open();
                using (var comando = new SQLiteCommand("SELECT * FROM Retiradas", conexao))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var retirada = new Retirada
                            {
                                ID = reader.GetInt32(0),
                                FuncionarioID = reader.GetInt32(1),
                                UsuarioID = reader.GetInt32(2),
                                LivroID = reader.GetInt32(3),
                                DataRetirada = reader.GetDateTime(4),
                                DataDevolucao = reader.GetDateTime(5),
                                Multa = reader.GetDouble(6)
                            };
                            listaRetiradas.Add(retirada);
                        }
                    }

                }
            }
            return listaRetiradas;
        }

        public static bool AddRetirada(Retirada retirada)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var comando = new SQLiteCommand(conexao))
                    {
                        comando.CommandText = "INSERT INTO Retiradas (FuncionarioID, UsuarioID, LivroID, DataRetirada, DataDevolucao, Multa) VALUES (@FuncionarioID, @UsuarioID, @LivroID, @DataRetirada, @DataDevolucao, @Multa)";
                        comando.Parameters.AddWithValue("@FuncionarioID", retirada.FuncionarioID);
                        comando.Parameters.AddWithValue("@UsuarioID", retirada.UsuarioID);
                        comando.Parameters.AddWithValue("@LivroID", retirada.LivroID);
                        comando.Parameters.AddWithValue("@DataRetirada", retirada.DataRetirada);
                        comando.Parameters.AddWithValue("@DataDevolucao", retirada.DataDevolucao);
                        comando.Parameters.AddWithValue("@Multa", retirada.Multa);
                        comando.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public static bool UpdateRetirada(Retirada retirada)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var comando = new SQLiteCommand(conexao))
                    {
                        comando.CommandText = "UPDATE Retiradas SET FuncionarioID = @FuncionarioID, UsuarioID = @UsuarioID, LivroID = @LivroID, DataRetirada = @DataRetirada, DataDevolucao = @DataDevolucao, Multa = @Multa WHERE ID = @ID";
                        comando.Parameters.AddWithValue("@ID", retirada.ID);
                        comando.Parameters.AddWithValue("@FuncionarioID", retirada.FuncionarioID);
                        comando.Parameters.AddWithValue("@UsuarioID", retirada.UsuarioID);
                        comando.Parameters.AddWithValue("@LivroID", retirada.LivroID);
                        comando.Parameters.AddWithValue("@DataRetirada", retirada.DataRetirada);
                        comando.Parameters.AddWithValue("@DataDevolucao", retirada.DataDevolucao);
                        comando.Parameters.AddWithValue("@Multa", retirada.Multa);
                        comando.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool DeleteRetirada(Retirada retirada)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var comando = new SQLiteCommand(conexao))
                    {
                        comando.CommandText = "DELETE FROM Retiradas WHERE ID = @ID";
                        comando.Parameters.AddWithValue("@ID", retirada.ID);
                        comando.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


    }

}
