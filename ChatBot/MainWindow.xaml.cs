using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Mensaje> listaMensajes;
        Mensaje mensaje;
        private const string mensajeRobot = "Lo siento, estoy un poco cansado de hablar";
        private const string usuarioRobot = "Robot";
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
            string conversacion = "";
            foreach (Mensaje m in listaMensajes)
            {
                conversacion += m.ToString();
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TXT files (*.txt;)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, conversacion);
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

        }

        private void configCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void checkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
             MessageBox.Show("Conexión correcta con el servidor del Bot","Comprobar conexión",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void checkCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void sendCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mensaje = new Mensaje(textoUsuarioTextBox.Text, "Usuario");
            listaMensajes.Add(mensaje);
            mensaje = new Mensaje(mensajeRobot, usuarioRobot);
            listaMensajes.Add(mensaje);
            textoUsuarioTextBox.Text = "";
            textoScrollViewer.ScrollToEnd();
        }

        private void sendCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textoUsuarioTextBox != null && textoUsuarioTextBox.Text != "")
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
    }
}
