using System;
using System.Data.SQLite;
using System.IO;

namespace SystemLibrary
{
    class Banco
    {
        public static string dbPath;
        private static SQLiteConnection conexao;

        public static SQLiteConnection ConexaoDB()
        {
            string dbFolder = "DB";
            string dbName = "biblioteca.sqlite3";
            string currentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
            dbPath = Path.Combine(currentDirectory, dbFolder, dbName);
            if (!File.Exists(dbPath))
                CriarDB();
            conexao = new SQLiteConnection($"Data Source={dbPath}");
            return conexao;
        }

        private static void CriarDB()
        {
            string dirPath = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            CriarTabelaUsuario();
            CriarTabelaLivro();
            CriarTabelaReserva();
            CriarTabelaRetirada();
            CriarTabelaFilaReserva();

        }

        private static void CriarTabelaUsuario()
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Usuarios (
                        ID INTEGER PRIMARY KEY,
                        Usuario TEXT UNIQUE NOT NULL,
                        Senha TEXT NOT NULL,
                        Reservas INTEGER DEFAULT 0,
                        LivrosRetirados INTEGER DEFAULT 0,
                        MultaTotal REAL DEFAULT 0,
                        Tipo TEXT CHECK(Tipo IN ('Cliente', 'Funcionario', 'Admin'))
                    );";


                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CriarTabelaLivro()
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Livros (
                    ID INTEGER PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    Autor TEXT NOT NULL,
                    Sinopse TEXT,
                    DataLancamento DATE,
                    Status TEXT CHECK(Status IN ('Disponível', 'Reservado', 'Retirado')),
                    Reservas INTEGER
                );";

                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CriarTabelaReserva()
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Reservas (
                        ID INTEGER PRIMARY KEY,
                        UsuarioID INTEGER,
                        LivroID INTEGER,
                        DataReserva DATE,
                        DataDevolucao DATE,
                        Multa REAL,
                        FOREIGN KEY (UsuarioID) REFERENCES Usuarios (ID),
                        FOREIGN KEY (LivroID) REFERENCES Livros (ID)
                    );";

                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CriarTabelaRetirada()
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Retiradas (
                        ID INTEGER PRIMARY KEY,
                        UsuarioID INTEGER,
                        LivroID INTEGER,
                        DataRetirada DATE,
                        DataDevolucao DATE,
                        Multa REAL,
                        FOREIGN KEY (UsuarioID) REFERENCES Usuarios (ID),
                        FOREIGN KEY (LivroID) REFERENCES Livros (ID)
                    );";

                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CriarTabelaFilaReserva()
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS FilaReservas (
                        ID INTEGER PRIMARY KEY,
                        ReservaID INTEGER,
                        Posicao INTEGER,
                        FOREIGN KEY (ReservaID) REFERENCES Reservas (ID)
                    );";

                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
