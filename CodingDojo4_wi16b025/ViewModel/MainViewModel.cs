using CodingDojo4_wi16b025.connection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodingDojo4_wi16b025.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        
        public bool isConnected = true;
        private Client clientcommunication; 

        public string Message { get; private set; }
        public string ChatName { get; set; }
        public ObservableCollection<string> MessageReceived { get; set; }
        public RelayCommand ConnectBtnClicked { get; set; }
        public RelayCommand SendBtnClicked { get; set; }

        public MainViewModel()
        {

            Message = "";
            MessageReceived = new ObservableCollection<string>();

            ConnectBtnClicked = new RelayCommand(
                () =>
                    {
                        isConnected = true;
                        //neuen Client anlegen mit IP, Port, Action -> Nachricht gesendet , Action Abbruch
                        clientcommunication = new Client("127.0.0.1", 81, new Action<string>(NewMessageIncoming), ClientDisconnect);
                    }

                );


            SendBtnClicked = new RelayCommand(
                () =>
                {
                    clientcommunication.Send(ChatName +": " + Message);
                });
        }

        private void ClientDisconnect()
        {
            isConnected = false;
            //update forcen
            CommandManager.InvalidateRequerySuggested();

        }

        private void NewMessageIncoming(string message)
        {
            //Applikation des Objektes + sync thread Action + der assoziierte dispatcher.
            App.Current.Dispatcher.Invoke(() =>
                {
                    //Nachrichten einschreiben
                    MessageReceived.Add(message);
                }
                );
        }
    }
}