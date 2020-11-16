﻿using System;
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
                Location = new Point((prompt.Width / 2) - 3 * (inputTextboxSize.Width / 2), 10 + promptLabel.Location.Y + promptLabel.Height),
                Text = $"{default_time}",
                MaxLength = 5
            };
            slowmotion_Textbox.KeyPress += InputTextbox_KeyPress;

            string confirmButtonText = "Επόμενο";
            Size confirmButtonSize = GetStringSize(confirmButtonText);

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Width = confirmButtonSize.Width * 2,
                Height = confirmButtonSize.Height * 2,
                Text = confirmButtonText,
                Location = new Point((promptLabel.Width / 2) - 2 * (confirmButtonSize.Width / 2), 5 + slowmotion_Textbox.Location.Y + slowmotion_Textbox.Height),
                DialogResult = DialogResult.OK
            };

            prompt.Height = (confirmButton.Location.Y + 3 * confirmButton.Height);
            prompt.Width = promptLabelSize.Width + 15;

            Console.WriteLine(prompt.Location.X - promptLabel.Location.X);

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
                TextAlign = ContentAlignment.MiddleCenter,
                Size = promptLabelSize,
                Location = new Point(prompt.Location.X, promptLabelSize.Height),
                Text = text
            };

            Size inputTextboxSize = GetStringSize("999");

            TextBox horizontal_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,                
                Text = $"{default_X}",
                MaxLength = 3
            };
            horizontal_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            TextBox vertical_axis_Textbox = new TextBox()
            {
                Size = inputTextboxSize,                
                Text = $"{default_Y}",
                MaxLength = 3
            };
            vertical_axis_Textbox.KeyPress += InputTextbox_KeyPress;

            string confirmButtonText = "Επόμενο";
            Size confirmButtonSize = GetStringSize(confirmButtonText);

            Button confirmButton = new Button()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Width = confirmButtonSize.Width,
                Height = confirmButtonSize.Height*2,
                Text = confirmButtonText,                
                DialogResult = DialogResult.OK
            };

            
            prompt.Width = promptLabelSize.Width;

            horizontal_axis_Textbox.Location = new Point((prompt.Width / 2) - (horizontal_axis_Textbox.Width /2), 10 + promptLabel.Location.Y + promptLabel.Height);
            vertical_axis_Textbox.Location = new Point((prompt.Width / 2) - (vertical_axis_Textbox.Width / 2), 5 + horizontal_axis_Textbox.Location.Y + horizontal_axis_Textbox.Height);
            confirmButton.Location = new Point((promptLabel.Width / 2) - 2 * (confirmButtonSize.Width / 2), 5 + vertical_axis_Textbox.Location.Y + vertical_axis_Textbox.Height);
            prompt.Height = (confirmButton.Location.Y + 2 * confirmButton.Height - 5);

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

        public static Size GetStringSize(string stringToMeasure)
        {

            //Font stringFont = new FontConverter().ConvertFromString(stringToMeasure) as Font;
            //Console.WriteLine("s: " + stringFont.Size);
            Graphics graphs = new Label().CreateGraphics();

            Font f = new Font("Microsoft Sans Serif", 14);

            string measuredStringWidth = stringToMeasure.Replace(" ", "_");

            return graphs.MeasureString(measuredStringWidth, f).ToSize();
        }







    }
}
