using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace JSharp_IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIHandler uiHandler;
        private static MainWindow main;
        public MainWindow()
        {
            InitializeComponent();
            this.uiHandler = new UIHandler(this);
            main = this;
            this.WindowState = WindowState.Maximized;
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
            this.uiHandler.CodeTextBox_Pasting(sender, e);
        }

        //This method will display the current project directory (with files) when it's loaded.
        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            this.uiHandler.TreeView_Loaded(sender, e);
        }

        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            this.uiHandler.MenuItem_Open(sender, e);
        }

        private void MenuItem_New(object sender, RoutedEventArgs e)
        {
            this.uiHandler.MenuItem_New(sender, e);
        }

        private void Button_CompileCode(object sender, RoutedEventArgs e)
        {
            this.uiHandler.Button_CompileCode(sender, e);
        }

        private void Button_RunCode(object sender, RoutedEventArgs e)
        {
            this.uiHandler.Button_RunCode(sender, e);
        }
    }
}
