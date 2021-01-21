using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        List<string> sexoUsuario = new List<string> { "Hombre", "Mujer" };

        public string ColorFondo { get; set; }
        public string ColorUsuario { get; set; }
        public string ColorRobot { get; set; }
        public string SexoUsuario { get; set; }

        public Configuracion()
        {
            InitializeComponent();
            DataContext = this;
            colorFondoComboBox.ItemsSource = typeof(Colors).GetProperties();
            colorUsuarioComboBox.ItemsSource = typeof(Colors).GetProperties();
            colorRobotComboBox.ItemsSource = typeof(Colors).GetProperties();
            sexoUsuarioComboBox.ItemsSource = sexoUsuario;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
