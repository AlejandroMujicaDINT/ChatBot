
using System.ComponentModel;

namespace ChatBot
{
    class Mensaje : INotifyPropertyChanged
    {
        private string texto;
        private string emisor;

        public string Texto
        {
            get => texto;
            set
            {
                if (texto != value)
                {
                    texto = value;
                    NotifyPropetyChanged("Texto");
                }
            }
        }

        public string Emisor
        {
            get => emisor;
            set
            {
                if (emisor != value)
                {
                    emisor = value;
                    NotifyPropetyChanged("Emisor");
                }
            }
        }

        public Mensaje(string texto, string emisor)
        {
            Texto = texto;
            Emisor = emisor;
        }

        public override string ToString()
        {
            return emisor+"\n"+texto+"\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
