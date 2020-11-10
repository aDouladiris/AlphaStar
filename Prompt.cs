using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaStar
{
    static class Prompt
    {
        public static string ShowDialog(string text, string caption, int defaultSize)
        {


            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Size promptLabelSize = GetStringSize(text).ToSize();
            Size inputTextboxSize = GetStringSize("999").ToSize();

            string confirmButtonText = "Confirm";
            Size confirmButtonSize = GetStringSize("Confirm").ToSize();

            Label promptLabel = new Label() { 
                TextAlign = ContentAlignment.MiddleCenter, 
                Size = promptLabelSize, 
                Location = new Point((prompt.Width /2)-(promptLabelSize.Width/2), promptLabelSize.Height), 
                Text = text 
            };
            
            TextBox inputTextbox = new TextBox() { 
                Size = inputTextboxSize, 
                Location = new Point((prompt.Width / 2) - (inputTextboxSize.Width / 2), 5 + promptLabel.Location.Y + promptLabel.Height), 
                Text = $"{defaultSize}",
                MaxLength = 4
            };
            inputTextbox.KeyPress += InputTextbox_KeyPress;
            
            Button confirmButton = new Button() { 
                TextAlign = ContentAlignment.MiddleCenter, 
                Size = confirmButtonSize, 
                Text = confirmButtonText, 
                Location = new Point((prompt.Width / 2) - (confirmButtonSize.Width / 2), 5 + inputTextbox.Location.Y + inputTextbox.Height), 
                DialogResult = DialogResult.OK 
            };

            prompt.Height = 2 * (confirmButton.Location.Y + confirmButton.Height);

            confirmButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(promptLabel);
            prompt.Controls.Add(inputTextbox);
            prompt.Controls.Add(confirmButton);
            prompt.AcceptButton = confirmButton;

            return prompt.ShowDialog() == DialogResult.OK ? inputTextbox.Text : $"{defaultSize}";
        }

        private static void InputTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        public static SizeF GetStringSize(string stringToMeasure)
        {
            Font stringFont = new Font("Microsoft Sans Serif", 12);
            Graphics graphs = new Label().CreateGraphics();

            string measuredStringWidth = stringToMeasure.Replace(" ", "_");
            SizeF stringSize = graphs.MeasureString(measuredStringWidth, stringFont);

            return stringSize;
        }







    }
}
