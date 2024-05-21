using DeltaCompassWPF.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories
{
    public class JogoRepository : RepositoryBase, IJogoRepository
    {
        public ObservableCollection<Jogo> GetJogo()
        {
            var jogos = new ObservableCollection<Jogo>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT id_jogos, nm_jogo, nr_calculo FROM tb_jogos";

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {   
                            while (reader.Read())
                            {
                                var jogo = new Jogo
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Calculo = reader.GetDouble(2)
                                };
                                jogos.Add(jogo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return jogos;
        }
    }
}
