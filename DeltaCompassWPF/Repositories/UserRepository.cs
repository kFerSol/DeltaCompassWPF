using DeltaCompassWPF.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(Usuario usuario)
        {
            using(var connection = GetConnection())
            using(var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO tb_usuario (nm_cadastro, cd_senha, ds_email) " +
                    "VALUES (@Username, @Password, @Email)";
                command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = usuario.Nome;
                command.Parameters.Add("@Password", MySqlDbType.VarChar).Value = usuario.Senha;
                command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = usuario.Email;
                command.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tb_usuario WHERE nm_cadastro = @Username and cd_senha = @Password";
                command.Parameters.Add("@Username", MySqlDbType.VarChar).Value=credential.UserName;
                command.Parameters.Add("@Password", MySqlDbType.VarChar).Value=credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(Usuario usuario)
        { 
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query =
                    "UPDATE tb_usuario SET ";
                List<string> campos = new List<string>();
                if (!string.IsNullOrEmpty(usuario.ApelidoPerfil))
                    campos.Add("nm_apelido = @Apelido");
                if (!string.IsNullOrEmpty(usuario.Biografia))
                    campos.Add("ds_bio = @Biografia");
                if (!string.IsNullOrEmpty(usuario.ModeloMonitor))
                    campos.Add("nm_monitor = @ModeloMonitor");
                if (!string.IsNullOrEmpty(usuario.ModeloMouse))
                    campos.Add("nm_mouse = @ModeloMouse");
                #pragma warning disable CS0472
                if (usuario.ResolucaoY != null)
                    campos.Add("nr_resolucaoY = @ResolucaoY");
                #pragma warning restore CS0472
                #pragma warning disable CS0472
                if (usuario.ResolucaoY != null)
                    campos.Add("nr_resolucaoX = @ResolucaoX");
                #pragma warning restore CS0472
                #pragma warning disable CS0472
                if (usuario.DpiMouse != null && usuario.DpiMouse != 0)
                    campos.Add("dpi_usuario = @DpiMouse");
                #pragma warning restore CS0472

                query += string.Join(", ", campos);
                query += " WHERE id_usuario = 1;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", usuario.Nome);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Apelido", usuario.ApelidoPerfil);
                    command.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                    command.Parameters.AddWithValue("@Biografia", usuario.Biografia);
                    command.Parameters.AddWithValue("@DpiMouse", usuario.DpiMouse);
                    command.Parameters.AddWithValue("@ModeloMonitor", usuario.ModeloMonitor);
                    command.Parameters.AddWithValue("@ModeloMouse", usuario.ModeloMouse);
                    command.Parameters.AddWithValue("@ResolucaoY", usuario.ResolucaoY);
                    command.Parameters.AddWithValue("@ResolucaoX", usuario.ResolucaoX);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Usuario> GetByAll()
        {
            throw new NotImplementedException();
        }

        public Usuario GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetUserId(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using(var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id_usuario FROM tb_usuario WHERE nm_cadastro = @Username";
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;

                    var result = command.ExecuteScalar();
                    if(result != null && int.TryParse(result.ToString(), out int userId))
                    {
                        return userId;
                    }
                    else
                    {
                        throw new Exception("Usuário não encontrado.");
                    }
                }
            }
        }

        public Usuario GetByUsername(string nome)
        {
            throw new NotImplementedException();
        }

        public void remove(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario GetInformacoesAutenticadas(string nome)
        {
            Usuario usuario = new Usuario();
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM tb_usuario WHERE nm_cadastro = @Username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", nome);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario.Nome = reader["nm_cadastro"].ToString();
                                usuario.Email = reader["ds_email"].ToString();
                                usuario.ApelidoPerfil = reader["nm_apelido"].ToString();
                                usuario.Biografia = reader["ds_bio"].ToString();
                                usuario.DpiMouse = Convert.ToInt32(reader["dpi_usuario"]);
                                usuario.ModeloMonitor = reader["nm_monitor"].ToString();
                                usuario.ModeloMouse = reader["nm_mouse"].ToString();
                                usuario.ResolucaoX = Convert.ToInt32(reader["nr_resolucaoX"]);
                                usuario.ResolucaoY = Convert.ToInt32(reader["nr_resolucaoY"]);
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return usuario;
        }
    }
}
