namespace AudioRecorder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_start = new Button();
            btn_stop = new Button();
            label1 = new Label();
            label2 = new Label();
            input_devices = new ComboBox();
            output_devices = new ComboBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // btn_start
            // 
            btn_start.BackColor = Color.Aquamarine;
            btn_start.Location = new Point(12, 160);
            btn_start.Name = "btn_start";
            btn_start.Size = new Size(128, 49);
            btn_start.TabIndex = 0;
            btn_start.Text = "Start Recording";
            btn_start.UseVisualStyleBackColor = false;
            btn_start.Click += btn_start_Click;
            // 
            // btn_stop
            // 
            btn_stop.BackColor = Color.LightSalmon;
            btn_stop.Location = new Point(146, 160);
            btn_stop.Name = "btn_stop";
            btn_stop.Size = new Size(128, 49);
            btn_stop.TabIndex = 1;
            btn_stop.Text = "Stop Recording";
            btn_stop.UseVisualStyleBackColor = false;
            btn_stop.Click += btn_stop_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Bright", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(32, 9);
            label1.Name = "label1";
            label1.Size = new Size(225, 31);
            label1.TabIndex = 2;
            label1.Text = "Audio Recorder";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 3;
            label2.Text = "Select Input Device";
            // 
            // input_devices
            // 
            input_devices.FormattingEnabled = true;
            input_devices.Location = new Point(12, 69);
            input_devices.Name = "input_devices";
            input_devices.Size = new Size(262, 23);
            input_devices.TabIndex = 4;
            // 
            // output_devices
            // 
            output_devices.FormattingEnabled = true;
            output_devices.Location = new Point(12, 113);
            output_devices.Name = "output_devices";
            output_devices.Size = new Size(262, 23);
            output_devices.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 95);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 5;
            label3.Text = "Select Output Device";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(290, 223);
            Controls.Add(output_devices);
            Controls.Add(label3);
            Controls.Add(input_devices);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_start);
            Controls.Add(btn_stop);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_start;
        private Button btn_stop;
        private Label label1;
        private Label label2;
        private ComboBox input_devices;
        private ComboBox output_devices;
        private Label label3;
    }
}