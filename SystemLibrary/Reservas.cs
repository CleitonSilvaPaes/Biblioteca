using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary
{
    public class Reservas
    {
        public int ID { get; set; }
        public int FuncionarioID { get; set; }
        public int UsuarioID { get; set; }
        public int LivroID { get; set; }
        public double Multa { get; set; }

        public static List<Reservas> GetReservas()
        {
            List<Reservas> listaReservas = new List<Reservas>();
            using (var conexao = Banco.ConexaoDB())
            {
                conexao.Open();
                using (var comando = new SQLiteCommand("SELECT * FROM Reservas", conexao))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservas reserva = new Reservas
                            {
                                ID = reader.GetInt32(0),
                                FuncionarioID = reader.GetInt32(1),
                                UsuarioID = reader.GetInt32(2),
                                LivroID = reader.GetInt32(3),
                                Multa = reader.GetDouble(4)
                            };
                            listaReservas.Add(reserva);
                        }
                    }
                }
            }
            return listaReservas;
        }

    }

}
