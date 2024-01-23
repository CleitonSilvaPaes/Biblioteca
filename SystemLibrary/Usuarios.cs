using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary
{
    public class Usuarios
    {
        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Reservas { get; set; }
        public int LivrosRetirados { get; set; }
        public double MultaTotal { get; set; }
        public string Tipo { get; set; }

        public static List<Usuarios> GetUsuarios()
        {
            List<Usuarios> listaUsuario = new List<Usuarios>();
            using (var conexao = Banco.ConexaoDB())
            {
                conexao.Open();
                using (var comando = new SQLiteCommand("SELECT * FROM Usuarios", conexao))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuarios usuario = new Usuarios
                            {
                                ID = reader.GetInt32(0),
                                Usuario = reader.GetString(1),
                                Senha = reader.GetString(2),
                                Reservas = reader.GetInt32(3),
                                LivrosRetirados = reader.GetInt32(4),
                                MultaTotal = reader.GetDouble(5),
                                Tipo = reader.GetString(6)
                            };
                            listaUsuario.Add(usuario);
                        }
                    }
                }
            }
                return listaUsuario;
        }

        public static void AddUsuario(Usuarios usuario)
        {
            using (var conexao = Banco.ConexaoDB())
            {
                conexao.Open();
                using (var comando = new SQLiteCommand(conexao))
                {
                    comando.CommandText = "INSERT INTO Usuarios (ID, Usuario, Senha, Reservas, LivrosRetirados, MultaTotal, Tipo) VALUES (@ID, @Usuario, @Senha, @Reservas, @LivrosRetirados, @MutaTotal, @Tipo)";
                    comando.Parameters.AddWithValue("@ID", usuario.ID);
                    comando.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                    comando.Parameters.AddWithValue("@Senha", usuario.Senha);
                    comando.Parameters.AddWithValue("@Reservas", usuario.Reservas);
                    comando.Parameters.AddWithValue("@LivrosRetirados", usuario.LivrosRetirados);
                    comando.Parameters.AddWithValue("@MutaTotal", usuario.MultaTotal);
                    comando.Parameters.AddWithValue("@Tipo", usuario.Tipo);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static bool AddUsuario(int id, string usuario, string senha, string tipo)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var comando = new SQLiteCommand(conexao))
                    {
                        comando.CommandText = "INSERT INTO Usuarios (ID, Usuario, Senha, Tipo) VALUES (@ID, @Usuario, @Senha, @Tipo)";
                        comando.Parameters.AddWithValue("@ID", id);
                        comando.Parameters.AddWithValue("@Usuario", usuario);
                        comando.Parameters.AddWithValue("@Senha", senha);
                        comando.Parameters.AddWithValue("@Tipo", tipo);
                        comando.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool UpdateUsuario(Usuarios usuario)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        using (var comando = new SQLiteCommand(conexao))
                        {
                            comando.CommandText = "UPDATE Usuarios SET Usuario = @Usuario, Senha = @Senha, Reservas = @Reservas, LivrosRetirados = @LivrosRetirados, MultaTotal = @MutaTotal, Tipo = @Tipo WHERE ID = @ID";
                            comando.Parameters.AddWithValue("@ID", usuario.ID);
                            comando.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                            comando.Parameters.AddWithValue("@Senha", usuario.Senha);
                            comando.Parameters.AddWithValue("@Reservas", usuario.Reservas);
                            comando.Parameters.AddWithValue("@LivrosRetirados", usuario.LivrosRetirados);
                            comando.Parameters.AddWithValue("@MutaTotal", usuario.MultaTotal);
                            comando.Parameters.AddWithValue("@Tipo", usuario.Tipo);
                            comando.ExecuteNonQuery();
                        }
                        transacao.Commit();
                    }
                }
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool RemoveUsuario(Usuarios usuario)
        {
            try
            {
                using (var conexao = Banco.ConexaoDB())
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        using (var comando = new SQLiteCommand(conexao))
                        {
                            comando.CommandText = "DELETE FROM Usuarios WHERE ID = @ID";
                            comando.Parameters.AddWithValue("@ID", usuario.ID);
                            comando.ExecuteNonQuery();
                        }
                        transacao.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }

}
