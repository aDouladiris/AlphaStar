namespace AlphaStar
{
    partial class AlphaStarAlgorithm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.exit_button = new System.Windows.Forms.Button();
            this.grid_panel = new System.Windows.Forms.Panel();
            this.obstacles_button = new System.Windows.Forms.Button();
            this.algo_button = new System.Windows.Forms.Button();
            this.clear_button = new System.Windows.Forms.Button();
            this.clearAll_button = new System.Windows.Forms.Button();
            this.debug_button = new System.Windows.Forms.Button();
            this.resize_button = new System.Windows.Forms.Button();
            this.timer_label = new System.Windows.Forms.Label();
            this.slow_motion_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.OrangeRed;
            this.exit_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.exit_button.Location = new System.Drawing.Point(1610, 12);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(60, 35);
            this.exit_button.TabIndex = 0;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // grid_panel
            // 
            this.grid_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid_panel.Location = new System.Drawing.Point(12, 12);
            this.grid_panel.Name = "grid_panel";
            this.grid_panel.Size = new System.Drawing.Size(1464, 651);
            this.grid_panel.TabIndex = 1;
            // 
            // obstacles_button
            // 
            this.obstacles_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.obstacles_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.obstacles_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.obstacles_button.Location = new System.Drawing.Point(1610, 67);
            this.obstacles_button.Name = "obstacles_button";
            this.obstacles_button.Size = new System.Drawing.Size(60, 35);
            this.obstacles_button.TabIndex = 2;
            this.obstacles_button.Text = "Black";
            this.obstacles_button.UseVisualStyleBackColor = false;
            this.obstacles_button.Click += new System.EventHandler(this.obstacles_button_Click);
            // 
            // algo_button
            // 
            this.algo_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.algo_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.algo_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.algo_button.Location = new System.Drawing.Point(1610, 124);
            this.algo_button.Name = "algo_button";
            this.algo_button.Size = new System.Drawing.Size(60, 35);
            this.algo_button.TabIndex = 3;
            this.algo_button.Text = "Algo";
            this.algo_button.UseVisualStyleBackColor = false;
            this.algo_button.Click += new System.EventHandler(this.algo_button_Click);
            // 
            // clear_button
            // 
            this.clear_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.clear_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.clear_button.Location = new System.Drawing.Point(1610, 180);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(60, 35);
            this.clear_button.TabIndex = 4;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = false;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // clearAll_button
            // 
            this.clearAll_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.clearAll_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.clearAll_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.clearAll_button.Location = new System.Drawing.Point(1610, 238);
            this.clearAll_button.Name = "clearAll_button";
            this.clearAll_button.Size = new System.Drawing.Size(60, 35);
            this.clearAll_button.TabIndex = 5;
            this.clearAll_button.Text = "Clear All";
            this.clearAll_button.UseVisualStyleBackColor = false;
            this.clearAll_button.Click += new System.EventHandler(this.clearAll_button_Click);
            // 
            // debug_button
            // 
            this.debug_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.debug_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.debug_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.debug_button.Location = new System.Drawing.Point(1610, 293);
            this.debug_button.Name = "debug_button";
            this.debug_button.Size = new System.Drawing.Size(60, 35);
            this.debug_button.TabIndex = 6;
            this.debug_button.Text = "Debug";
            this.debug_button.UseVisualStyleBackColor = false;
            this.debug_button.Click += new System.EventHandler(this.debug_button_Click);
            // 
            // resize_button
            // 
            this.resize_button.BackColor = System.Drawing.Color.DodgerBlue;
            this.resize_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.resize_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.resize_button.Location = new System.Drawing.Point(1610, 346);
            this.resize_button.Name = "resize_button";
            this.resize_button.Size = new System.Drawing.Size(60, 35);
            this.resize_button.TabIndex = 7;
            this.resize_button.Text = "Resize";
            this.resize_button.UseVisualStyleBackColor = false;
            this.resize_button.Click += new System.EventHandler(this.resize_button_Click);
            // 
            // timer_label
            // 
            this.timer_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.timer_label.Location = new System.Drawing.Point(1610, 476);
            this.timer_label.Name = "timer_label";
            this.timer_label.Size = new System.Drawing.Size(60, 43);
            this.timer_label.TabIndex = 8;
            this.timer_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // slow_motion_button
            // 
            this.slow_motion_button.BackColor = System.Drawing.Color.SlateGray;
            this.slow_motion_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.slow_motion_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.slow_motion_button.Location = new System.Drawing.Point(1610, 403);
            this.slow_motion_button.Name = "slow_motion_button";
            this.slow_motion_button.Size = new System.Drawing.Size(60, 35);
            this.slow_motion_button.TabIndex = 9;
            this.slow_motion_button.Text = "Slow";
            this.slow_motion_button.UseVisualStyleBackColor = false;
            this.slow_motion_button.Click += new System.EventHandler(this.slow_motion_button_Click);
            // 
            // AlphaStarAlgorithm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1682, 675);
            this.Controls.Add(this.slow_motion_button);
            this.Controls.Add(this.timer_label);
            this.Controls.Add(this.resize_button);
            this.Controls.Add(this.debug_button);
            this.Controls.Add(this.clearAll_button);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.algo_button);
            this.Controls.Add(this.obstacles_button);
            this.Controls.Add(this.grid_panel);
            this.Controls.Add(this.exit_button);
            this.Name = "AlphaStarAlgorithm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Panel grid_panel;
        private System.Windows.Forms.Button obstacles_button;
        private System.Windows.Forms.Button algo_button;
        private System.Windows.Forms.Button clear_button;
        private System.Windows.Forms.Button clearAll_button;
        private System.Windows.Forms.Button debug_button;
        private System.Windows.Forms.Button resize_button;
        private System.Windows.Forms.Label timer_label;
        private System.Windows.Forms.Button slow_motion_button;
    }
}

