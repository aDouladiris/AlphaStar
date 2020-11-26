using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaStar
{
    static class Helpers
    {
        static Form parent;

        public static string Wizard(string text, string caption, Form parent)
        {
            Helpers.parent = parent;

            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                Name = "prompt_Form"
            };

            prompt.MouseLeave += Prompt_MouseLeave;
            prompt.MouseEnter += Prompt_MouseEnter;

            Size promptLabelSize = GetStringSize(text);

            Label promptLabel = new Label()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Size = promptLabelSize,
                Location = new Point(prompt.Location.X, promptLabelSize.Height),
                Text = text
            }; 

            string confirmButtonText = "Επόμενο";
            Size confirmButtonSize = GetStringSize(confirmButtonText);

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Width = confirmButtonSize.Width * 2,
                Height = confirmButtonSize.Height + 10,
                Text = confirmButtonText,
                Location = new Point((promptLabel.Width / 2) - 2 * (confirmButtonSize.Width / 2), promptLabel.Location.Y + 2 * promptLabel.Height),
                DialogResult = DialogResult.OK
            };

            prompt.Height = (confirmButton.Location.Y + 3 * confirmButton.Height);
            prompt.Width = promptLabelSize.Width + 15;


            confirmButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(promptLabel);
            prompt.Controls.Add(confirmButton);
            prompt.AcceptButton = confirmButton;

            return prompt.ShowDialog() == DialogResult.OK ? "yo" : "";
        }

        private static void Prompt_MouseEnter(object sender, EventArgs e)
        {
            Form _sender = (Form)sender;
            if (_sender.Name.Equals("prompt_Form"))
            {
                Console.WriteLine("form enter");
                //_sender.
            }
            

        }

        private static void Prompt_MouseLeave(object sender, EventArgs e)
        {            
            Form _sender = (Form)sender;

            if(_sender.Name.Equals("prompt_Form"))
            {
                Console.WriteLine("form exit");
                Helpers.parent.Focus();
            }

            
            
        }

        public static string ShowDialogSlowMotion(string text, string caption, int default_time)
        {
            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Size promptLabelSize = GetStringSize(text);

            Label promptLabel = new Label()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Size = promptLabelSize,
                Location = new Point(prompt.Location.X, promptLabelSize.Height),
                Text = text
            };

            Size inputTextboxSize = GetStringSize("99999");

            Console.WriteLine(inputTextboxSize.Width);

            TextBox slowmotion_Textbox = new TextBox()
            {
                Size = inputTextboxSize,
                Location = new Point((promptLabel.Width / 2) - (inputTextboxSize.Width / 2) , 10 + promptLabel.Location.Y + promptLabel.Height),
                Text = $"{default_time}",
                MaxLength = 5,
                TextAlign = HorizontalAlignment.Center
            };
            slowmotion_Textbox.KeyPress += InputTextbox_KeyPress;

            string confirmButtonText = "Επόμενο";
            Size confirmButtonSize = GetStringSize(confirmButtonText);

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Width = confirmButtonSize.Width * 2,
                Height = confirmButtonSize.Height + 10,
                Text = confirmButtonText,
                Location = new Point((promptLabel.Width / 2) - 2*(confirmButtonSize.Width / 2), slowmotion_Textbox.Location.Y + 2 * slowmotion_Textbox.Height),
                DialogResult = DialogResult.OK
            };

            prompt.Height = (confirmButton.Location.Y + 3 * confirmButton.Height);
            prompt.Width = promptLabelSize.Width + 15;


            confirmButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(promptLabel);
            prompt.Controls.Add(slowmotion_Textbox);
            prompt.Controls.Add(confirmButton);
            prompt.AcceptButton = confirmButton;

            return prompt.ShowDialog() == DialogResult.OK ? slowmotion_Textbox.Text : $"{default_time}";
        }


        public static string[] ShowDialog(string text, string caption, int default_X, int default_Y)
        {


            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Size promptLabelSize = GetStringSize(text);            

            Label promptLabel = new Label()
            {                
                Size = promptLabelSize,
                Location = new Point(-5, promptLabelSize.Height),
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Size inputTextboxSize = GetStringSize("999");

            Size horizontalAxisLabelSize = GetStringSize("Οριζόντια πλευρά τετραπλεύρου:");
            Label horizontal_axis_Label = new Label()
            {
                Width = horizontalAxisLabelSize.Width,
                Height = inputTextboxSize.Height + 3,
                Text = "Οριζόντια πλευρά τετραπλεύρου:",
                Location = new Point(20, promptLabel.Location.Y + (2 * promptLabel.Height))
            };

            TextBox horizontal_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,
                Location = new Point((horizontal_axis_Label.Location.X + horizontal_axis_Label.Width + 5), horizontal_axis_Label.Location.Y),
                Text = $"{default_X}",
                MaxLength = 4,
                TextAlign = HorizontalAlignment.Center
            };
            horizontal_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            Size verticalAxisLabelSize = GetStringSize("Κάθετη πλευρά τετραπλεύρου:");
            Label vertical_axis_Label = new Label()
            {
                Width = verticalAxisLabelSize.Width,
                Height = inputTextboxSize.Height + 3,
                Text = "Κάθετη πλευρά τετραπλεύρου:",
                Location = new Point(20, horizontal_axis_Textbox.Location.Y + (1 * horizontal_axis_Textbox.Height))
            };

            TextBox vertical_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,
                Location = new Point(horizontal_axis_Textbox.Location.X, vertical_axis_Label.Location.Y),
                Text = $"{default_Y}",
                MaxLength = 4,
                TextAlign = HorizontalAlignment.Center
            };
            vertical_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            string confirmButtonText = "Επόμενο";
            Size confirmButtonSize = GetStringSize(confirmButtonText);

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Width = confirmButtonSize.Width *2,
                Height = confirmButtonSize.Height + 10,
                Text = confirmButtonText,
                Location = new Point((prompt.Width / 2) - (confirmButtonSize.Width / 2) - 10, vertical_axis_Textbox.Location.Y + 2*vertical_axis_Textbox.Height),
                DialogResult = DialogResult.OK                
            };

            prompt.Width += 20;
            prompt.Height = confirmButton.Location.Y + 3*confirmButton.Height;


            confirmButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(promptLabel);
            prompt.Controls.Add(horizontal_axis_Textbox);
            prompt.Controls.Add(horizontal_axis_Label);
            prompt.Controls.Add(vertical_axis_Label);
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

        public static Size GetStringSize(string stringToMeasure)
        {
            Font stringFont = new Font("Microsoft Sans Serif", 9);
            Graphics graphs = new Label().CreateGraphics();

            string measuredStringWidth = stringToMeasure.Replace(" ", "_");

            return graphs.MeasureString(measuredStringWidth, stringFont).ToSize();
        }







    }
}
