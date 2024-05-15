using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Database;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using Mysqlx.Crud;

namespace DeltaCompassWPF.ViewModels
{
    internal class UsuarioViewModel
    {
        private Usuario _usuario;
        private const string ConnectionString = "server=localhost;database=Delta_Compass;port=3307;user=root;password=usbw";


        public Usuario Usuario
        {
            get { return _usuario; }
            set 
            { 
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public ICommand SalvarCommand { get; private set; }

        public UsuarioViewModel()
        {
            SalvarCommand = new RelayCommand(Salvar, Verificar);
            Usuario = new Usuario();
        }

        private void Salvar(object parameter)
        {
            try
            {
                using(MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query =
                        "UPDATE tb_usuario SET ";
                    List<string> campos = new List<string>();
                    if (!string.IsNullOrEmpty(Usuario.Nome))
                        campos.Add("nm_cadastro = @Nome");
                    if (!string.IsNullOrEmpty(Usuario.ApelidoPerfil))
                        campos.Add("nm_apelido = @Apelido");
                    if (!string.IsNullOrEmpty(Usuario.Biografia))
                        campos.Add("ds_bio = @Biografia");
                    if (!string.IsNullOrEmpty(Usuario.ModeloMonitor))
                        campos.Add("nm_monitor = @ModeloMonitor");
                    if (!string.IsNullOrEmpty(Usuario.ModeloMouse))
                        campos.Add("nm_mouse = @ModeloMouse");
                    #pragma warning disable CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null' 
                    if (Usuario.ResolucaoY != null)
                        campos.Add("nr_resolucaoY = @ResolucaoY");
                    #pragma warning restore CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null'
                    #pragma warning disable CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null' 
                    if (Usuario.ResolucaoY != null)
                        campos.Add("nr_resolucaoX = @ResolucaoX");
                    #pragma warning restore CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null'
                    #pragma warning disable CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null' 
                    if (Usuario.DpiMouse != null && Usuario.DpiMouse != 0)
                        campos.Add("dpi_usuario = @DpiMouse");
                    #pragma warning restore CS0472 // O resultado da expressão é sempre o mesmo, pois um valor deste tipo nunca é 'null'

                    query += string.Join(", ", campos);
                    query += " WHERE id_usuario = 1;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", Usuario.Nome);
                        command.Parameters.AddWithValue("@Email", Usuario.Email);
                        command.Parameters.AddWithValue("@Apelido", Usuario.ApelidoPerfil);
                        command.Parameters.AddWithValue("@Telefone", Usuario.Telefone);
                        command.Parameters.AddWithValue("@Biografia", Usuario.Biografia);
                        command.Parameters.AddWithValue("@DpiMouse", Usuario.DpiMouse);
                        command.Parameters.AddWithValue("@ModeloMonitor", Usuario.ModeloMonitor);
                        command.Parameters.AddWithValue("@ModeloMouse", Usuario.ModeloMouse);
                        command.Parameters.AddWithValue("@ResolucaoY", Usuario.ResolucaoY);
                        command.Parameters.AddWithValue("@ResolucaoX", Usuario.ResolucaoX);
                        command.ExecuteNonQuery();
                    }
                }
            } catch (Exception)
            {
                throw;
            }
        }

        private bool Verificar(object parameter)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }
    }
}
