using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JSharp_IDE.ViewModel
{
    class RichTextBoxViewModel
    {
        public RichTextBoxViewModel()
        {
            Enabled = true;
        }

        public static bool Enabled { get; set; }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Enabled)
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


        }
    }
}
