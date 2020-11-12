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
        public static string[] ShowDialog(string text, string caption, int default_X, int default_Y)
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

            Label promptLabel = new Label()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Size = promptLabelSize,
                Location = new Point((prompt.Width / 2) - (promptLabelSize.Width / 2), promptLabelSize.Height),
                Text = text
            };

            TextBox horizontal_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,
                Location = new Point((prompt.Width / 2) - (inputTextboxSize.Width / 2), 5 + promptLabel.Location.Y + promptLabel.Height),
                Text = $"{default_X}",
                MaxLength = 4
            };
            horizontal_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            TextBox vertical_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,
                Location = new Point((prompt.Width / 2) - (inputTextboxSize.Width / 2), 5 + horizontal_axis_Textbox.Location.Y + horizontal_axis_Textbox.Height),
                Text = $"{default_Y}",
                MaxLength = 4
            };
            vertical_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Size = confirmButtonSize,
                Text = confirmButtonText,
                Location = new Point((prompt.Width / 2) - (confirmButtonSize.Width / 2), 5 + vertical_axis_Textbox.Location.Y + vertical_axis_Textbox.Height),
                DialogResult = DialogResult.OK
            };

            prompt.Height = 2 * (confirmButton.Location.Y + confirmButton.Height);

            confirmButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(promptLabel);
            prompt.Controls.Add(horizontal_axis_Textbox);
            prompt.Controls.Add(vertical_axis_Textbox);
            prompt.Controls.Add(confirmButton);
            prompt.AcceptButton = confirmButton;

            string[] default_returning_array = new string[2] { $"{default_X}", $"{default_Y}" };

            return prompt.ShowDialog() == DialogResult.OK ? new string[2] { horizontal_axis_Textbox.Text, vertical_axis_Textbox.Text } : default_returning_array;
        }

        private static void InputTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
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
