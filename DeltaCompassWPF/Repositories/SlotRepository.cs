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

        public void Add(SlotConfiguracao slot)
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
                    command.Parameters.Add("@Sensibilidade", MySqlDbType.VarChar).Value = slot.Sensibilidade;
                    command.Parameters.Add("@IdUsuario", MySqlDbType.Int64).Value = slot.IdUser;
                    command.Parameters.Add("@IdJogo", MySqlDbType.Int64).Value = slot.IdJogo;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write("{0}", ex);
            }
        }

        public void Remove(SlotConfiguracao slot)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM tb_sensibilidade WHERE id_sensibilidade = @IdSlot";
                command.Parameters.Add("@IdSlot", MySqlDbType.Int64).Value = slot.IdSens;
                command.ExecuteNonQuery();
            }
        }

        public void Edit(SlotConfiguracao slot)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE tb_sensibilidade " +
                    "SET vl_sensibilidade = @NovaSens " +
                    "WHERE id_sensibilidade = @IdSens";
                command.Parameters.AddWithValue("@NovaSens", slot.Sensibilidade);
                command.Parameters.AddWithValue("@IdSens", slot.IdSens);
                command.ExecuteNonQuery();
            }
        }
    }
}
