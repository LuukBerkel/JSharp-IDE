using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace IDE_testing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
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
        }

        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            switch (e.Key)
            {
                case Key.Tab:
                    rtb.CaretPosition.InsertTextInRun("    ");
                    break;
                default:
                    break;
            }
        }

        private void CodeTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            Task.Run(async () =>
            {
                Debug.WriteLine("CodeTextBox_Pasting");
                await TextFormatter.OnTextPasted(rtb);
            });
        }
    }
}
