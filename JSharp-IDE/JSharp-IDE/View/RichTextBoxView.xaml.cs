using System;
using System.Collections.Generic;
using System.IO;
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

namespace JSharp_IDE.View
{
    /// <summary>
    /// Interaction logic for RichTextBoxView.xaml
    /// </summary>
    public partial class RichTextBoxView : UserControl
    {
        public RichTextBox RichTextBox;

        public RichTextBoxView()
        {
            InitializeComponent();
            //Assign the xml object to the code variable.
            RichTextBox = CodeTextBox;
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

        private void CodeTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            Task.Run(async () =>
            {
                await TextFormatter.OnTextPasted(rtb);
            });
        }

        public void Update(string path)
        {
            RichTextBox.Dispatcher.Invoke(() =>
            {
                FlowDocument doc = RichTextBox.Document;
                doc.Blocks.Clear();
                //Add each line to the document as a separate block.
                foreach (string line in File.ReadAllLines(path))
                {
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(new Run(line));
                    p.Margin = new Thickness(0);
                    doc.Blocks.Add(p);
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
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
