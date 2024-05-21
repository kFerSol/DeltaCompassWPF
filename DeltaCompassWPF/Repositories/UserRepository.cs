using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories.Authentication;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly UserService _userService;

        public UserRepository()
        {
            _userService = UserService.Instance;
        }

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
            if(Thread.CurrentPrincipal is CustomPrincipal customPrincipal)
            {
                usuario.Id = customPrincipal.CustomIdentity.Id;
            }

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
                if (usuario.ResolucaoY != null)
                    campos.Add("nr_resolucaoY = @ResolucaoY");
                if (usuario.ResolucaoY != null)
                    campos.Add("nr_resolucaoX = @ResolucaoX");
                if (usuario.DpiMouse != null && usuario.DpiMouse != 0)
                    campos.Add("dpi_usuario = @DpiMouse");
                if (!string.IsNullOrEmpty(usuario.Telefone))
                    campos.Add("nr_telefone = @Telefone");
                if (usuario.ImagemPerfil != null)
                    campos.Add("img_perfil = @ImagemPerfil");
                if (usuario.ImagemFundo != null)
                    campos.Add("img_fundo = @ImagemFundo");

                if(campos.Count > 0)
                {
                    query += string.Join(", ", campos);
                    query += " WHERE id_usuario = @Id;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (usuario.Email != null || usuario.ApelidoPerfil != "")
                            command.Parameters.AddWithValue("@Email", usuario.Email);
                        if (usuario.ApelidoPerfil != null || usuario.ApelidoPerfil != "")
                            command.Parameters.AddWithValue("@Apelido", usuario.ApelidoPerfil);
                        if (usuario.Telefone != null || usuario.Telefone != "")
                            command.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                        if (usuario.Biografia != null || usuario.Biografia != "")
                            command.Parameters.AddWithValue("@Biografia", usuario.Biografia);
                        if (usuario.DpiMouse != null)
                            command.Parameters.AddWithValue("@DpiMouse", usuario.DpiMouse);
                        if (usuario.ModeloMonitor != null || usuario.ModeloMonitor != "")
                            command.Parameters.AddWithValue("@ModeloMonitor", usuario.ModeloMonitor);
                        if (usuario.ModeloMouse != null || usuario.ModeloMouse != "")
                            command.Parameters.AddWithValue("@ModeloMouse", usuario.ModeloMouse);
                        if (usuario.ResolucaoY != null)
                            command.Parameters.AddWithValue("@ResolucaoY", usuario.ResolucaoY);
                        if (usuario.ResolucaoX != null)
                            command.Parameters.AddWithValue("@ResolucaoX", usuario.ResolucaoX);
                        if (usuario.ImagemPerfil != null)
                            command.Parameters.AddWithValue("@ImagemPerfil", usuario.ImagemPerfil);
                        if (usuario.ImagemFundo != null)
                            command.Parameters.AddWithValue("@ImagemFundo", usuario.ImagemFundo);
                        command.Parameters.AddWithValue("@Id", usuario.Id);
                        command.ExecuteNonQuery();
                    }
                }
                
            }
            var currentUser = UserService.Instance.CurrentUser;
            if (!string.IsNullOrEmpty(usuario.ApelidoPerfil))
                currentUser.ApelidoPerfil = usuario.ApelidoPerfil;
            if (!string.IsNullOrEmpty(usuario.Biografia))
                currentUser.Biografia = usuario.Biografia;
            if (!string.IsNullOrEmpty(usuario.Telefone))
                currentUser.Telefone = usuario.Telefone;
            if (!string.IsNullOrEmpty(usuario.ModeloMonitor))
                currentUser.ModeloMonitor = usuario.ModeloMonitor;
            if (!string.IsNullOrEmpty(usuario.ModeloMouse))
                currentUser.ModeloMouse = usuario.ModeloMouse;
            if (usuario.ResolucaoY != null)
                currentUser.ResolucaoY = usuario.ResolucaoY;
            if (usuario.ResolucaoX != null)
                currentUser.ResolucaoX = usuario.ResolucaoX;
            if (usuario.DpiMouse != null && usuario.DpiMouse != 0)
                currentUser.DpiMouse = usuario.DpiMouse;
            if (usuario.ImagemPerfil != null)
                currentUser.ImagemPerfil = usuario.ImagemPerfil;
            if (usuario.ImagemFundo != null)
                currentUser.ImagemFundo = usuario.ImagemFundo;

            _userService.CurrentUser = currentUser;
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

        public IEnumerable<SlotConfiguracao> GetSensibilidadeByUserId(int userId)
        {
            var sensibilidades = new List<SlotConfiguracao>();

            using(var connection = GetConnection())
            using(var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    SELECT s.vl_sensibilidade, s.cd_jogos, j.nm_jogo
                    FROM tb_sensibilidade s
                    JOIN tb_jogos j ON s.cd_jogos = j.id_jogos
                    WHERE s.cd_usuario = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sensibilidade = new SlotConfiguracao
                        {
                            Sensibilidade = reader.GetInt32("vl_sensibilidade"),
                            JogoId = reader.GetInt32("cd_jogos"),
                            NomeJogo = reader.GetString("nm_jogo")
                        };
                        sensibilidades.Add(sensibilidade);
                    }
                }
            }
            return sensibilidades;
        }

        public Usuario GetInformacoesAutenticadas(string nome)
        {
            Usuario user = null;

            using(var connection = GetConnection())
            using(var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tb_usuario WHERE nm_cadastro = @Username";
                command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = nome;

                using( var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        user = new Usuario()
                        {
                            Id = reader.GetInt32("id_usuario"),
                            Nome = reader.GetString("nm_cadastro"),
                            Email = reader.GetString("ds_email"),
                            ApelidoPerfil = reader.IsDBNull(reader.GetOrdinal("nm_apelido")) ? null : reader.GetString("nm_apelido"),
                            Biografia = reader.IsDBNull(reader.GetOrdinal("ds_bio")) ? null : reader.GetString("ds_bio"),
                            ModeloMonitor = reader.IsDBNull(reader.GetOrdinal("nm_monitor")) ? null : reader.GetString("nm_monitor"),
                            ModeloMouse = reader.IsDBNull(reader.GetOrdinal("nm_mouse")) ? null : reader.GetString("nm_mouse"),
                            ResolucaoX = reader.IsDBNull(reader.GetOrdinal("nr_resolucaoX")) ? (int?)null : reader.GetInt32("nr_resolucaoX"),
                            ResolucaoY = reader.IsDBNull(reader.GetOrdinal("nr_resolucaoY")) ? (int?)null : reader.GetInt32("nr_resolucaoY"),
                            DpiMouse = reader.IsDBNull(reader.GetOrdinal("dpi_usuario")) ? (int?)null : reader.GetInt32("dpi_usuario"),
                            ImagemPerfil = reader.IsDBNull(reader.GetOrdinal("img_perfil")) ? null : (byte[])reader["img_perfil"],
                            ImagemFundo = reader.IsDBNull(reader.GetOrdinal("img_fundo")) ? null : (byte[])reader["img_fundo"]
                        };
                    }
                }
            }
            return user;
        }
    }
}
