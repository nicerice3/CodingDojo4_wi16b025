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

        private Client clientcom;
        private bool isConnected = false;

        public string ChatName { get; set; }
        public string Message { get; set; }
        public ObservableCollection<string> ReceivedMessages { get; set; }
        public RelayCommand ConnectBtnClickCmd { get; set; }
        public RelayCommand SendBtnClickCmd { get; set; }

        public MainViewModel()
        {
            Message = "";
            ReceivedMessages = new ObservableCollection<string>();
            
            ConnectBtnClickCmd = new RelayCommand(
                () =>
                {
                    isConnected = true;
                    clientcom = new Client("127.0.0.1", 10100, new Action<string>(NewMessageReceived), ClientDissconnected);

                },
            () =>
            {
                return (!isConnected);
            });
            
            SendBtnClickCmd = new RelayCommand(
                () => {
                    clientcom.Send(ChatName + ": " + Message);
                    
                    ReceivedMessages.Add("YOU: " + Message);
                }, () => { return (isConnected && Message.Length >= 1); });
        }

        private void ClientDissconnected()
        {
            isConnected = false;
            
            CommandManager.InvalidateRequerySuggested();
        }

        private void NewMessageReceived(string message)
        {
            
            App.Current.Dispatcher.Invoke(() =>
            {
                ReceivedMessages.Add(message);
            });
        }
    }
}