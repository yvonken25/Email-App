using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WpfAEmailApp.ViewModels
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        private string _selectedProvider = "Gmail";
        private string _senderEmail;
        private string _appPassword;
        private string _recipientEmail;
        private string _subject;
        private string _body;

        public string SelectedProvider
        {
            get => _selectedProvider;
            set { _selectedProvider = value; OnPropertyChanged(); }
        }

        public string SenderEmail
        {
            get => _senderEmail;
            set { _senderEmail = value; OnPropertyChanged(); }
        }

        public string AppPassword
        {
            get => _appPassword;
            set { _appPassword = value; OnPropertyChanged(); }
        }

        public string RecipientEmail
        {
            get => _recipientEmail;
            set { _recipientEmail = value; OnPropertyChanged(); }
        }

        public string Subject
        {
            get => _subject;
            set { _subject = value; OnPropertyChanged(); }
        }

        public string Body
        {
            get => _body;
            set { _body = value; OnPropertyChanged(); }
        }

        public ICommand SendEmailCommand { get; }

        public EmailViewModel()
        {
            SendEmailCommand = new RelayCommand(SendEmail, CanSendEmail);
        }

        private bool CanSendEmail()
        {
            return !string.IsNullOrWhiteSpace(SenderEmail) &&
                   !string.IsNullOrWhiteSpace(AppPassword) &&
                   !string.IsNullOrWhiteSpace(RecipientEmail);
        }

        private void SendEmail()
        {
            try
            {
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;

                if (SelectedProvider == "Outlook")
                {
                    smtpHost = "smtp.office365.com";
                    smtpPort = 587;
                }

                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(SenderEmail, AppPassword);

                    using (var mailMessage = new MailMessage(SenderEmail, RecipientEmail, Subject, Body))
                    {
                        client.Send(mailMessage);
                        MessageBox.Show("E-mail envoyé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}