namespace kubernets
{
    partial class Login
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
            buttonLogin = new Button();
            IpAddress = new Label();
            Token = new Label();
            textBoxIpAddress = new TextBox();
            textBoxToken = new TextBox();
            label1 = new Label();
            textBoxUsername = new TextBox();
            listBox1 = new ListBox();
            buttonToggleToken = new Button();
            SuspendLayout();
            // 
            // buttonLogin
            // 
            buttonLogin.BackColor = Color.DodgerBlue;
            buttonLogin.FlatAppearance.BorderColor = Color.DodgerBlue;
            buttonLogin.FlatAppearance.BorderSize = 0;
            buttonLogin.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
            buttonLogin.FlatAppearance.MouseOverBackColor = Color.RoyalBlue;
            buttonLogin.FlatStyle = FlatStyle.Flat;
            buttonLogin.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonLogin.Location = new Point(204, 379);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(94, 33);
            buttonLogin.TabIndex = 0;
            buttonLogin.Text = "Login";
            buttonLogin.UseVisualStyleBackColor = false;
            buttonLogin.Click += buttonLogin_Click;
            // 
            // IpAddress
            // 
            IpAddress.AutoSize = true;
            IpAddress.BackColor = Color.Transparent;
            IpAddress.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            IpAddress.ForeColor = SystemColors.ControlLightLight;
            IpAddress.Location = new Point(73, 278);
            IpAddress.Name = "IpAddress";
            IpAddress.Size = new Size(94, 23);
            IpAddress.TabIndex = 1;
            IpAddress.Text = "IP Address";
            // 
            // Token
            // 
            Token.AutoSize = true;
            Token.BackColor = Color.Transparent;
            Token.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Token.ForeColor = SystemColors.ControlLightLight;
            Token.Location = new Point(110, 327);
            Token.Name = "Token";
            Token.Size = new Size(57, 23);
            Token.TabIndex = 2;
            Token.Text = "Token";
            // 
            // textBoxIpAddress
            // 
            textBoxIpAddress.Location = new Point(173, 277);
            textBoxIpAddress.Name = "textBoxIpAddress";
            textBoxIpAddress.Size = new Size(125, 27);
            textBoxIpAddress.TabIndex = 3;
            // 
            // textBoxToken
            // 
            textBoxToken.Location = new Point(173, 324);
            textBoxToken.Name = "textBoxToken";
            textBoxToken.Size = new Size(125, 27);
            textBoxToken.TabIndex = 4;
            textBoxToken.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(78, 235);
            label1.Name = "label1";
            label1.Size = new Size(89, 23);
            label1.TabIndex = 5;
            label1.Text = "Username";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(173, 234);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(125, 27);
            textBoxUsername.TabIndex = 6;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(636, 138);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(368, 404);
            listBox1.TabIndex = 7;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // buttonToggleToken
            // 
            buttonToggleToken.BackgroundImage = Properties.Resources.eye_slash_Icon;
            buttonToggleToken.BackgroundImageLayout = ImageLayout.Zoom;
            buttonToggleToken.Location = new Point(304, 324);
            buttonToggleToken.Name = "buttonToggleToken";
            buttonToggleToken.Size = new Size(27, 27);
            buttonToggleToken.TabIndex = 8;
            buttonToggleToken.Click += buttonToggleToken_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Login;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1044, 678);
            Controls.Add(buttonToggleToken);
            Controls.Add(listBox1);
            Controls.Add(textBoxUsername);
            Controls.Add(label1);
            Controls.Add(textBoxToken);
            Controls.Add(textBoxIpAddress);
            Controls.Add(Token);
            Controls.Add(IpAddress);
            Controls.Add(buttonLogin);
            DoubleBuffered = true;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonLogin;
        private Label IpAddress;
        private Label Token;
        private TextBox textBoxIpAddress;
        private TextBox textBoxToken;
        private Label label1;
        private TextBox textBoxUsername;
        private ListBox listBox1;
        private Button buttonToggleToken;
    }
}