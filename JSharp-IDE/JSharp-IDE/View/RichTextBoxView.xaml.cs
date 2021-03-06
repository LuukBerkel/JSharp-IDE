using JSharp_IDE.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace JSharp_IDE.View
{
    /// <summary>
    /// Interaction logic for RichTextBoxView.xaml
    /// </summary>
    public partial class RichTextBoxView : UserControl
    {
        public RichTextBox RichTextBox;
        public static Timer FileUpdateTimer;
        private RichTextBox box;
        private string previous;

      

        public RichTextBoxView()
        {
            InitializeComponent();
            //Assign the xml object to the code variable.
            RichTextBox = CodeTextBox;
            FileUpdateTimer = new Timer(7000);
            FileUpdateTimer.Elapsed += ElapsedHandler;
            //Only call the event once.
            FileUpdateTimer.AutoReset = false;
        }

        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            Project.notifier.ShowInformation("The project is unlocked.");
            RichTextBoxViewModel.Enabled = true;
            Task.Run(async () => await TextFormatter.OnTextPasted(box));
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            if (RichTextBoxViewModel.Enabled)
            {
                //Echo fix....
                if (StringFromRichTextBox(rtb) != previous)
                {
                    Project.SendFileToServer();
                }

                //Remove this event to prevent a recursive call
                rtb.TextChanged -= CodeTextBox_TextChanged;
                int result = 0;
                Task.Run(async () =>
                {
                    result = await TextFormatter.OnTextChanged(rtb);
                //Update after the previous update was finished.
                if (result == 1)
                    {
                    //Add this event back when the operation is finished.
                    rtb.TextChanged += CodeTextBox_TextChanged;
                    }
                });

                //Setting backwards variabels
                box = rtb;
            }
            previous = StringFromRichTextBox(rtb);
        }

        private void CodeTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            Task.Run(async () =>
            {
                await TextFormatter.OnTextPasted(rtb);
            });
        }

     

        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            switch (e.Key)
            {
                case Key.Tab:
                    rtb.CaretPosition.InsertTextInRun("    ");
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }


        public static string StringFromRichTextBox(RichTextBox rtb)
        {
            if (rtb != null)
            {
                TextRange textRange = new TextRange(
                    // TextPointer to the start of content in the RichTextBox.
                    rtb.Document.ContentStart,
                    // TextPointer to the end of content in the RichTextBox.
                    rtb.Document.ContentEnd
                );

                // The Text property on a TextRange object returns a string
                // representing the plain text content of the TextRange.
                return textRange.Text;
            }

            return "";
        }


    }
}
