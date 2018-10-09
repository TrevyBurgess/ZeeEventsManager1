//------------------------------------------------------------
// <copyright file="SmileyDialog.xaml" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public sealed partial class SmileyDialog : ContentDialog, INotifyPropertyChanged
    {
        private string mailTo { get; }
        private string appName { get; }
        private ImageSource HappyFace { get; }
        private ImageSource SadFace { get; }

        public SmileyDialog(
            string mailTo,
            string appName,
            ImageSource happyFace,
            ImageSource sadFace)
        {
            InitializeComponent();
            this.mailTo = mailTo;
            this.appName = appName;
            HappyFace = happyFace;
            SadFace = sadFace;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void ContentDialog_SendFeedback(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (EmailAddressBox.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                EmailAddress = string.Empty;
            }

            string messageType = ShowSmileMessage ? Q.Resources.SmileyDialog_Smile :Q.Resources.SmileyDialog_Frown;
            string subject =
                Uri.EscapeDataString(string.Format(Q.Resources.SmileyDialog_EmailTitle, appName, messageType, EmailAddress));

            string messageBody = Uri.EscapeDataString(FeedbackMessage + "\n\n");
            string message = string.Format(Q.Resources.SmileyDialog_EmailMessage, mailTo, subject, messageBody);

            await Launcher.LaunchUriAsync(new Uri(message));
        }

        private void ContentDialog_Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Do nothing
        }

        public bool ShowSmileMessage
        {
            get
            {
                return _ShowSmileMessage;
            }
            set
            {
                if (value != _ShowSmileMessage)
                {
                    _ShowSmileMessage = value;
                    OnStateChanged();
                }
            }
        }
        private bool _ShowSmileMessage;

        public string FeedbackMessage { get; private set; }

        public string EmailAddress { get; private set; }

        private void OnStateChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
