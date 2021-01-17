using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Mensaje> listaMensajes;
        Mensaje mensaje;
        private QnAMakerRuntimeClient cliente;
        private const string usuarioHumano = "Usuario";
        private const string usuarioRobot = "Robot";
        private const string falloRobot = "Lo siento, algo ha salido mal";
        public MainWindow()
        {
            InitializeComponent();
            listaMensajes = new ObservableCollection<Mensaje>();
            mensajesItemsControl.DataContext = listaMensajes;
        }

        private void newCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            listaMensajes.Clear();
        }

        private void newCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (mensajesItemsControl != null && mensajesItemsControl.HasItems)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void guardarCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StringBuilder conversacion = new StringBuilder();
            foreach (Mensaje m in listaMensajes)
            {
                conversacion.Append(m.ToString());
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TXT files (*.txt;)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, conversacion.ToString());
        }

        private void guardarCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (mensajesItemsControl != null && mensajesItemsControl.HasItems)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void salirCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void salirCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void configCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Configuracion configuracion = new Configuracion();
            configuracion.Owner = this;

            configuracion.ColorUsuario = Properties.Settings.Default.UsuarioColor;
            configuracion.ColorFondo = Properties.Settings.Default.FondoColor;
            configuracion.ColorRobot = Properties.Settings.Default.RobotColor;
            configuracion.SexoUsuario = Properties.Settings.Default.SexoUsuario;

            if (configuracion.ShowDialog() == true)
            {
                Properties.Settings.Default.UsuarioColor = configuracion.ColorUsuario.ToString();
                Properties.Settings.Default.FondoColor = configuracion.ColorFondo.ToString();
                Properties.Settings.Default.RobotColor = configuracion.ColorRobot.ToString();
                Properties.Settings.Default.SexoUsuario = configuracion.SexoUsuario;
                Properties.Settings.Default.Save();
            }
        }

        private void configCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private async void checkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(await RespuestaRobotAsync("Hola") != falloRobot)
            {
                MessageBox.Show("Conexión correcta con el servidor del Bot", "Comprobar conexión", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Conexión fallida con el servidor del Bot", "Comprobar conexión", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void checkCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private async void sendCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string pregunta = textoUsuarioTextBox.Text;
            mensaje = new Mensaje(textoUsuarioTextBox.Text, usuarioHumano);
            textoUsuarioTextBox.Text = "";
            listaMensajes.Add(mensaje);
            mensaje = new Mensaje("Procesando...", usuarioRobot);
            listaMensajes.Add(mensaje);
            textoScrollViewer.ScrollToEnd();
            listaMensajes[listaMensajes.Count - 1].Texto = await RespuestaRobotAsync(pregunta);
        }

        private void sendCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textoUsuarioTextBox != null && textoUsuarioTextBox.Text != "")
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        public async System.Threading.Tasks.Task<string> RespuestaRobotAsync(string pregunta)
        {
            string respuesta;
            try
            {
                string EndPoint = "https://alejandrochatbot.azurewebsites.net";
                string EndPointKey = "edf23ac8-a06b-45be-b66b-ea40b104bd52";
                string KnowledgeBaseId = "8fadfbdd-ce9d-456d-b371-ca8575b398d8";
                cliente = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(EndPointKey)) { RuntimeEndpoint = EndPoint };

                QnASearchResultList response = await cliente.Runtime.GenerateAnswerAsync(KnowledgeBaseId, new QueryDTO { Question = pregunta });
                respuesta = response.Answers[0].Answer;
            }
            catch (Exception)
            {
                respuesta = falloRobot;
            }
            return respuesta;
        }
    }
}
