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
            label9 = new Label();
            label8 = new Label();
            textBoxPort = new TextBox();
            textBoxImage = new TextBox();
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
            button7 = new Button();
            label16 = new Label();
            numericUpDown1 = new NumericUpDown();
            label10 = new Label();
            label11 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            label12 = new Label();
            label13 = new Label();
            comboBox2 = new ComboBox();
            button8 = new Button();
            button9 = new Button();
            textBox5 = new TextBox();
            label14 = new Label();
            listBox3 = new ListBox();
            label15 = new Label();
            tabPage5 = new TabPage();
            comboBox4 = new ComboBox();
            comboBox3 = new ComboBox();
            button10 = new Button();
            label18 = new Label();
            label19 = new Label();
            textBox7 = new TextBox();
            label20 = new Label();
            label21 = new Label();
            button11 = new Button();
            button12 = new Button();
            textBox8 = new TextBox();
            label22 = new Label();
            listBox4 = new ListBox();
            label23 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            tabPage5.SuspendLayout();
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
            label3.Location = new Point(951, 54);
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
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(886, 297);
            label9.Name = "label9";
            label9.Size = new Size(44, 25);
            label9.TabIndex = 22;
            label9.Text = "Port";
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
            // textBoxPort
            // 
            textBoxPort.Location = new Point(942, 294);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.Size = new Size(248, 31);
            textBoxPort.TabIndex = 20;
            // 
            // textBoxImage
            // 
            textBoxImage.Location = new Point(942, 223);
            textBoxImage.Name = "textBoxImage";
            textBoxImage.Size = new Size(248, 31);
            textBoxImage.TabIndex = 19;
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
            tabPage4.Controls.Add(button7);
            tabPage4.Controls.Add(label16);
            tabPage4.Controls.Add(numericUpDown1);
            tabPage4.Controls.Add(label10);
            tabPage4.Controls.Add(label11);
            tabPage4.Controls.Add(textBox3);
            tabPage4.Controls.Add(textBox4);
            tabPage4.Controls.Add(label12);
            tabPage4.Controls.Add(label13);
            tabPage4.Controls.Add(comboBox2);
            tabPage4.Controls.Add(button8);
            tabPage4.Controls.Add(button9);
            tabPage4.Controls.Add(textBox5);
            tabPage4.Controls.Add(label14);
            tabPage4.Controls.Add(listBox3);
            tabPage4.Controls.Add(label15);
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1290, 675);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Deployments";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Image = (Image)resources.GetObject("button7.Image");
            button7.Location = new Point(644, 544);
            button7.Name = "button7";
            button7.Size = new Size(46, 40);
            button7.TabIndex = 39;
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(798, 357);
            label16.Name = "label16";
            label16.Size = new Size(167, 25);
            label16.TabIndex = 38;
            label16.Text = "Number of Replicas";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(938, 355);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(248, 31);
            numericUpDown1.TabIndex = 37;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(888, 287);
            label10.Name = "label10";
            label10.Size = new Size(44, 25);
            label10.TabIndex = 36;
            label10.Text = "Port";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(870, 216);
            label11.Name = "label11";
            label11.Size = new Size(62, 25);
            label11.TabIndex = 35;
            label11.Text = "Image";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(938, 284);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(248, 31);
            textBox3.TabIndex = 34;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(938, 213);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(248, 31);
            textBox4.TabIndex = 33;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(873, 148);
            label12.Name = "label12";
            label12.Size = new Size(59, 25);
            label12.TabIndex = 32;
            label12.Text = "Name";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(828, 86);
            label13.Name = "label13";
            label13.Size = new Size(104, 25);
            label13.TabIndex = 31;
            label13.Text = "Namespace";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(938, 83);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(248, 33);
            comboBox2.TabIndex = 30;
            // 
            // button8
            // 
            button8.BackColor = Color.DarkOrange;
            button8.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(644, 73);
            button8.Name = "button8";
            button8.Size = new Size(100, 41);
            button8.TabIndex = 28;
            button8.Text = "Delete";
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.SkyBlue;
            button9.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button9.Location = new Point(1010, 420);
            button9.Name = "button9";
            button9.Size = new Size(100, 41);
            button9.TabIndex = 27;
            button9.Text = "Create";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click_1;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(938, 145);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(248, 31);
            textBox5.TabIndex = 26;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(972, 39);
            label14.Name = "label14";
            label14.Size = new Size(176, 25);
            label14.TabIndex = 25;
            label14.Text = "Create Deployment";
            // 
            // listBox3
            // 
            listBox3.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 25;
            listBox3.Location = new Point(52, 73);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(567, 529);
            listBox3.TabIndex = 24;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.Location = new Point(246, 39);
            label15.Name = "label15";
            label15.Size = new Size(158, 25);
            label15.TabIndex = 23;
            label15.Text = "Deployments List";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(comboBox4);
            tabPage5.Controls.Add(comboBox3);
            tabPage5.Controls.Add(button10);
            tabPage5.Controls.Add(label18);
            tabPage5.Controls.Add(label19);
            tabPage5.Controls.Add(textBox7);
            tabPage5.Controls.Add(label20);
            tabPage5.Controls.Add(label21);
            tabPage5.Controls.Add(button11);
            tabPage5.Controls.Add(button12);
            tabPage5.Controls.Add(textBox8);
            tabPage5.Controls.Add(label22);
            tabPage5.Controls.Add(listBox4);
            tabPage5.Controls.Add(label23);
            tabPage5.Location = new Point(4, 34);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(1290, 675);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Services/Ingress";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(934, 222);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(248, 33);
            comboBox4.TabIndex = 57;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(934, 92);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(248, 33);
            comboBox3.TabIndex = 56;
            // 
            // button10
            // 
            button10.Image = (Image)resources.GetObject("button10.Image");
            button10.Location = new Point(640, 553);
            button10.Name = "button10";
            button10.Size = new Size(46, 40);
            button10.TabIndex = 55;
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(884, 225);
            label18.Name = "label18";
            label18.Size = new Size(44, 25);
            label18.TabIndex = 52;
            label18.Text = "Port";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(824, 296);
            label19.Name = "label19";
            label19.Size = new Size(104, 25);
            label19.TabIndex = 51;
            label19.Text = "Service Port";
            // 
            // textBox7
            // 
            textBox7.Location = new Point(934, 293);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(248, 31);
            textBox7.TabIndex = 49;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(853, 157);
            label20.Name = "label20";
            label20.Size = new Size(75, 25);
            label20.TabIndex = 48;
            label20.Text = "Domain";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(818, 95);
            label21.Name = "label21";
            label21.Size = new Size(110, 25);
            label21.TabIndex = 47;
            label21.Text = "Deployment";
            // 
            // button11
            // 
            button11.BackColor = Color.DarkOrange;
            button11.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button11.Location = new Point(640, 82);
            button11.Name = "button11";
            button11.Size = new Size(100, 41);
            button11.TabIndex = 45;
            button11.Text = "Delete";
            button11.UseVisualStyleBackColor = false;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.BackColor = Color.SkyBlue;
            button12.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button12.Location = new Point(1001, 361);
            button12.Name = "button12";
            button12.Size = new Size(100, 41);
            button12.TabIndex = 44;
            button12.Text = "Create";
            button12.UseVisualStyleBackColor = false;
            button12.Click += button12_Click;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(934, 154);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(248, 31);
            textBox8.TabIndex = 43;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label22.Location = new Point(985, 48);
            label22.Name = "label22";
            label22.Size = new Size(133, 25);
            label22.TabIndex = 42;
            label22.Text = "Create Ingress";
            // 
            // listBox4
            // 
            listBox4.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox4.FormattingEnabled = true;
            listBox4.ItemHeight = 25;
            listBox4.Location = new Point(48, 82);
            listBox4.Name = "listBox4";
            listBox4.Size = new Size(567, 529);
            listBox4.TabIndex = 41;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label23.Location = new Point(242, 48);
            label23.Name = "label23";
            label23.Size = new Size(107, 25);
            label23.TabIndex = 40;
            label23.Text = "Ingress List";
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
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
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
        private Label label10;
        private Label label11;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label12;
        private Label label13;
        private ComboBox comboBox2;
        private Button button8;
        private Button button9;
        private TextBox textBox5;
        private Label label14;
        private ListBox listBox3;
        private Label label15;
        private Label label16;
        private NumericUpDown numericUpDown1;
        private Button button7;
        private ComboBox comboBox3;
        private Button button10;
        private Label label18;
        private Label label19;
        private TextBox textBox7;
        private Label label20;
        private Label label21;
        private Button button11;
        private Button button12;
        private TextBox textBox8;
        private Label label22;
        private ListBox listBox4;
        private Label label23;
        private ComboBox comboBox4;
    }
}