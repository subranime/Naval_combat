namespace Naval_combat.UI
{
    partial class MainForm
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
            this.Start_game_button = new System.Windows.Forms.Button();
            this.gamePictureBox = new System.Windows.Forms.PictureBox();
            this.Shoot_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gamePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Start_game_button
            // 
            this.Start_game_button.Location = new System.Drawing.Point(12, 534);
            this.Start_game_button.Name = "Start_game_button";
            this.Start_game_button.Size = new System.Drawing.Size(98, 27);
            this.Start_game_button.TabIndex = 2;
            this.Start_game_button.Text = "Старт";
            this.Start_game_button.UseVisualStyleBackColor = true;
            this.Start_game_button.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // gamePictureBox
            // 
            this.gamePictureBox.Location = new System.Drawing.Point(12, 12);
            this.gamePictureBox.Name = "gamePictureBox";
            this.gamePictureBox.Size = new System.Drawing.Size(500, 500);
            this.gamePictureBox.TabIndex = 3;
            this.gamePictureBox.TabStop = false;
            // 
            // Shoot_button
            // 
            this.Shoot_button.Location = new System.Drawing.Point(211, 527);
            this.Shoot_button.Name = "Shoot_button";
            this.Shoot_button.Size = new System.Drawing.Size(108, 41);
            this.Shoot_button.TabIndex = 4;
            this.Shoot_button.Text = "Выстрел";
            this.Shoot_button.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 587);
            this.Controls.Add(this.Shoot_button);
            this.Controls.Add(this.gamePictureBox);
            this.Controls.Add(this.Start_game_button);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gamePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Start_game_button;
        private System.Windows.Forms.PictureBox gamePictureBox;
        private System.Windows.Forms.Button Shoot_button;
    }
}