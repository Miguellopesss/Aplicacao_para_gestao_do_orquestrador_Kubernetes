namespace kubernets
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            listView1 = new ListView();
            Nome = new ColumnHeader();
            IP = new ColumnHeader();
            Kubelet = new ColumnHeader();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label1 = new Label();
            tabPage2 = new TabPage();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            textBox1 = new TextBox();
            label3 = new Label();
            listBox1 = new ListBox();
            label2 = new Label();
            tabPage3 = new TabPage();
            label7 = new Label();
            label6 = new Label();
            comboBox1 = new ComboBox();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            textBox2 = new TextBox();
            label4 = new Label();
            listBox2 = new ListBox();
            label5 = new Label();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            textBoxImage = new TextBox();
            textBoxPort = new TextBox();
            label8 = new Label();
            label9 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { Nome, IP, Kubelet });
            listView1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listView1.GridLines = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.HideSelection = true;
            listView1.Location = new Point(343, 132);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new Size(642, 471);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // Nome
            // 
            Nome.Text = "Nome";
            Nome.Width = 200;
            // 
            // IP
            // 
            IP.Text = "IP";
            IP.Width = 200;
            // 
            // Kubelet
            // 
            Kubelet.Text = "Kubelet";
            Kubelet.Width = 200;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(22, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1298, 713);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(listView1);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1290, 675);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Nodes";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(604, 91);
            label1.Name = "label1";
            label1.Size = new Size(100, 25);
            label1.TabIndex = 1;
            label1.Text = "Nodes List";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button3);
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(textBox1);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(listBox1);
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1290, 675);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Namespaces";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Image = (Image)resources.GetObject("button3.Image");
            button3.Location = new Point(638, 561);
            button3.Name = "button3";
            button3.Size = new Size(46, 40);
            button3.TabIndex = 8;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkOrange;
            button2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(638, 88);
            button2.Name = "button2";
            button2.Size = new Size(100, 41);
            button2.TabIndex = 7;
            button2.Text = "Delete";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.SkyBlue;
            button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(976, 148);
            button1.Name = "button1";
            button1.Size = new Size(100, 41);
            button1.TabIndex = 6;
            button1.Text = "Create";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(893, 99);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(249, 31);
            textBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(923, 54);
            label3.Name = "label3";
            label3.Size = new Size(170, 25);
            label3.TabIndex = 4;
            label3.Text = "Create Namespace";
            // 
            // listBox1
            // 
            listBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(46, 88);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(567, 529);
            listBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(249, 54);
            label2.Name = "label2";
            label2.Size = new Size(152, 25);
            label2.TabIndex = 2;
            label2.Text = "Namespaces List";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(textBoxPort);
            tabPage3.Controls.Add(textBoxImage);
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(label6);
            tabPage3.Controls.Add(comboBox1);
            tabPage3.Controls.Add(button4);
            tabPage3.Controls.Add(button5);
            tabPage3.Controls.Add(button6);
            tabPage3.Controls.Add(textBox2);
            tabPage3.Controls.Add(label4);
            tabPage3.Controls.Add(listBox2);
            tabPage3.Controls.Add(label5);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1290, 675);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Pods";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(868, 158);
            label7.Name = "label7";
            label7.Size = new Size(59, 25);
            label7.TabIndex = 18;
            label7.Text = "Name";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(823, 96);
            label6.Name = "label6";
            label6.Size = new Size(104, 25);
            label6.TabIndex = 17;
            label6.Text = "Namespace";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(942, 93);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(248, 33);
            comboBox1.TabIndex = 16;
            // 
            // button4
            // 
            button4.Image = (Image)resources.GetObject("button4.Image");
            button4.Location = new Point(641, 553);
            button4.Name = "button4";
            button4.Size = new Size(46, 40);
            button4.TabIndex = 15;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // button5
            // 
            button5.BackColor = Color.DarkOrange;
            button5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(641, 83);
            button5.Name = "button5";
            button5.Size = new Size(100, 41);
            button5.TabIndex = 14;
            button5.Text = "Delete";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.SkyBlue;
            button6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(1018, 357);
            button6.Name = "button6";
            button6.Size = new Size(100, 41);
            button6.TabIndex = 13;
            button6.Text = "Create";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(942, 155);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(248, 31);
            textBox2.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(1014, 49);
            label4.Name = "label4";
            label4.Size = new Size(104, 25);
            label4.TabIndex = 11;
            label4.Text = "Create Pod";
            // 
            // listBox2
            // 
            listBox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 25;
            listBox2.Location = new Point(49, 83);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(567, 529);
            listBox2.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(270, 49);
            label5.Name = "label5";
            label5.Size = new Size(86, 25);
            label5.TabIndex = 9;
            label5.Text = "Pods List";
            // 
            // tabPage4
            // 
            tabPage4.BackgroundImage = Properties.Resources.ChatGPT_Image_8_05_2025__15_53_27;
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1290, 675);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Deployments";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 34);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(1290, 675);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Services/Ingress";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBoxImage
            // 
            textBoxImage.Location = new Point(942, 223);
            textBoxImage.Name = "textBoxImage";
            textBoxImage.Size = new Size(248, 31);
            textBoxImage.TabIndex = 19;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new Point(942, 294);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.Size = new Size(248, 31);
            textBoxPort.TabIndex = 20;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(868, 226);
            label8.Name = "label8";
            label8.Size = new Size(62, 25);
            label8.TabIndex = 21;
            label8.Text = "Image";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(886, 297);
            label9.Name = "label9";
            label9.Size = new Size(44, 25);
            label9.TabIndex = 22;
            label9.Text = "Port";
            // 
            // Form2
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1332, 753);
            Controls.Add(tabControl1);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView listView1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private Label label1;
        private ColumnHeader Nome;
        private ColumnHeader IP;
        private ColumnHeader Kubelet;
        private Label label2;
        private ListBox listBox1;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private Label label3;
        private Button button3;
        private ComboBox comboBox1;
        private Button button4;
        private Button button5;
        private Button button6;
        private TextBox textBox2;
        private Label label4;
        private ListBox listBox2;
        private Label label5;
        private Label label7;
        private Label label6;
        private TextBox textBoxImage;
        private Label label9;
        private Label label8;
        private TextBox textBoxPort;
    }
}