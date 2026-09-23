using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;


namespace WpfAEmailApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupération des données encodées par l'utilisateur
            string expediteur = TxtExpediteur.Text.Trim();
            string password = TxtPassword.Password;
            string destinataire = TxtDestinataire.Text.Trim();
            string objet = TxtObjet.Text;
            string message = TxtMessage.Text;

            // 2. Validation simple
            if (string.IsNullOrEmpty(expediteur) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(destinataire))
            {
                MessageBox.Show("Veuillez remplir au moins votre e-mail, la clé d'application et l'e-mail du destinataire.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Desactivation du bouton pendant l'envoi pour éviter le double-clic
            BtnEnvoyer.IsEnabled = false;

            try
            {
                // 3. Préparation du message (System.Net.Mail)
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(expediteur);
                mail.To.Add(destinataire);
                mail.Subject = objet;
                mail.Body = message;

                // 4. Configuration SMTP pour Gmail
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(expediteur, password),
                    EnableSsl = true
                };

                // 5. Envoi
                smtp.Send(mail);

                MessageBox.Show("Votre e-mail a été envoyé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Réinitialisation des champs du message
                TxtObjet.Clear();
                TxtMessage.Clear();
            }
            catch (SmtpException ex)
            {
                // Capture propre d'une erreur liée au serveur SMTP ou aux identifiants
                MessageBox.Show($"Erreur lors de l'envoi du mail (SMTP) :\n\n{ex.Message}",
                                "Échec d'envoi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                // Capture si l'adresse e-mail saisie n'a pas un format valide
                MessageBox.Show("L'une des adresses e-mail saisies est invalide (format incorrect).",
                                "Erreur de format", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Capture globale pour toute autre erreur (pas d'accès réseau, etc.)
                MessageBox.Show($"Une erreur inattendue est survenue :\n\n{ex.Message}",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Réactivation du bouton d'envoi
                BtnEnvoyer.IsEnabled = true;
            }
        }
    }
}