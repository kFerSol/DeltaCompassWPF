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
    public class SlotRepository : RepositoryBase, ISlotRepository
    {
        private readonly UserService _userService;

        public SlotRepository()
        {
            _userService = UserService.Instance;
        }

        public void Add(ObservableCollection<SlotConfiguracao> slot, int idUsuario, int idJogo)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO tb_sensibilidade(vl_sensibilidade, cd_usuario, cd_jogos) " +
                        "VALUES(@Sensibilidade, @IdUsuario, @IdJogo)";
                    //command.Parameters.Add("@Sensibilidade", MySqlDbType.VarChar).Value = slot.Sensibilidade;
                    command.Parameters.Add("@IdUsuario", MySqlDbType.Int64).Value = idUsuario;
                    command.Parameters.Add("@IdJogo", MySqlDbType.Int64).Value = idJogo;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write("{0}", ex);
            }
        }

        public void Remove(SlotConfiguracao slot, int idUsuario)
        {
            throw new NotImplementedException();
        }

        public void Edit(int idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
