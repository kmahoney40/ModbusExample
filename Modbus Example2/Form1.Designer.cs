namespace Modbus_Example2
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.write0 = new System.Windows.Forms.Button();
            this.write1 = new System.Windows.Forms.Button();
            this.readBit = new System.Windows.Forms.Button();
            this.write16 = new System.Windows.Forms.Button();
            this.read16 = new System.Windows.Forms.Button();
            this.MessageRecieved = new System.Windows.Forms.TextBox();
            this.MessageSent = new System.Windows.Forms.TextBox();
            this.readBitValue = new System.Windows.Forms.TextBox();
            this.read16Value = new System.Windows.Forms.TextBox();
            this.write16Value = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.PLCStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.write32Value = new System.Windows.Forms.NumericUpDown();
            this.write32 = new System.Windows.Forms.Button();
            this.read32 = new System.Windows.Forms.Button();
            this.writeFloatValue = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.writeFloat = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.read32Value = new System.Windows.Forms.TextBox();
            this.readFloat = new System.Windows.Forms.Button();
            this.read32FloatValue = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.read32InFloatValue = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.readInFloat = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.write16Value)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.write32Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeFloatValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Write Bit (0x05)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Read Bit (0x01)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(288, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Write 16Bit Value (0x06)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(434, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Read 16Bit Value (0x04)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(473, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Value:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 336);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Message Sent:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(64, 362);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Message Recieved:";
            // 
            // write0
            // 
            this.write0.Location = new System.Drawing.Point(39, 44);
            this.write0.Name = "write0";
            this.write0.Size = new System.Drawing.Size(75, 23);
            this.write0.TabIndex = 8;
            this.write0.Text = "0";
            this.write0.UseVisualStyleBackColor = true;
            this.write0.Click += new System.EventHandler(this.write0_Click);
            // 
            // write1
            // 
            this.write1.Location = new System.Drawing.Point(39, 73);
            this.write1.Name = "write1";
            this.write1.Size = new System.Drawing.Size(75, 23);
            this.write1.TabIndex = 9;
            this.write1.Text = "1";
            this.write1.UseVisualStyleBackColor = true;
            this.write1.Click += new System.EventHandler(this.write1_Click);
            // 
            // readBit
            // 
            this.readBit.Location = new System.Drawing.Point(172, 44);
            this.readBit.Name = "readBit";
            this.readBit.Size = new System.Drawing.Size(75, 23);
            this.readBit.TabIndex = 10;
            this.readBit.Text = "Read";
            this.readBit.UseVisualStyleBackColor = true;
            this.readBit.Click += new System.EventHandler(this.readBit_Click);
            // 
            // write16
            // 
            this.write16.Location = new System.Drawing.Point(334, 73);
            this.write16.Name = "write16";
            this.write16.Size = new System.Drawing.Size(75, 23);
            this.write16.TabIndex = 11;
            this.write16.Text = "Write";
            this.write16.UseVisualStyleBackColor = true;
            this.write16.Click += new System.EventHandler(this.write16_Click);
            // 
            // read16
            // 
            this.read16.Location = new System.Drawing.Point(481, 44);
            this.read16.Name = "read16";
            this.read16.Size = new System.Drawing.Size(75, 23);
            this.read16.TabIndex = 12;
            this.read16.Text = "Read";
            this.read16.UseVisualStyleBackColor = true;
            this.read16.Click += new System.EventHandler(this.read16_Click);
            // 
            // MessageRecieved
            // 
            this.MessageRecieved.Location = new System.Drawing.Point(169, 359);
            this.MessageRecieved.Name = "MessageRecieved";
            this.MessageRecieved.Size = new System.Drawing.Size(344, 20);
            this.MessageRecieved.TabIndex = 13;
            this.MessageRecieved.TextChanged += new System.EventHandler(this.MessageRecieved_TextChanged);
            // 
            // MessageSent
            // 
            this.MessageSent.Location = new System.Drawing.Point(169, 333);
            this.MessageSent.Name = "MessageSent";
            this.MessageSent.Size = new System.Drawing.Size(344, 20);
            this.MessageSent.TabIndex = 14;
            // 
            // readBitValue
            // 
            this.readBitValue.Location = new System.Drawing.Point(203, 75);
            this.readBitValue.Name = "readBitValue";
            this.readBitValue.Size = new System.Drawing.Size(44, 20);
            this.readBitValue.TabIndex = 15;
            // 
            // read16Value
            // 
            this.read16Value.Location = new System.Drawing.Point(512, 76);
            this.read16Value.Name = "read16Value";
            this.read16Value.Size = new System.Drawing.Size(44, 20);
            this.read16Value.TabIndex = 16;
            // 
            // write16Value
            // 
            this.write16Value.Location = new System.Drawing.Point(334, 44);
            this.write16Value.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.write16Value.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.write16Value.Name = "write16Value";
            this.write16Value.Size = new System.Drawing.Size(75, 20);
            this.write16Value.TabIndex = 17;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PLCStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 404);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1096, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // PLCStatusLabel
            // 
            this.PLCStatusLabel.Name = "PLCStatusLabel";
            this.PLCStatusLabel.Size = new System.Drawing.Size(128, 17);
            this.PLCStatusLabel.Text = "PLC Connection Status";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(601, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Write 32Bit Value (0x10)";
            // 
            // write32Value
            // 
            this.write32Value.Location = new System.Drawing.Point(645, 44);
            this.write32Value.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.write32Value.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.write32Value.Name = "write32Value";
            this.write32Value.Size = new System.Drawing.Size(120, 20);
            this.write32Value.TabIndex = 21;
            this.write32Value.ValueChanged += new System.EventHandler(this.write32value_ValueChanged);
            // 
            // write32
            // 
            this.write32.Location = new System.Drawing.Point(645, 70);
            this.write32.Name = "write32";
            this.write32.Size = new System.Drawing.Size(75, 23);
            this.write32.TabIndex = 22;
            this.write32.Text = "Write";
            this.write32.UseVisualStyleBackColor = true;
            this.write32.Click += new System.EventHandler(this.write32_Click);
            // 
            // read32
            // 
            this.read32.Location = new System.Drawing.Point(830, 44);
            this.read32.Name = "read32";
            this.read32.Size = new System.Drawing.Size(75, 23);
            this.read32.TabIndex = 23;
            this.read32.Text = "Read";
            this.read32.UseVisualStyleBackColor = true;
            this.read32.Click += new System.EventHandler(this.read32_Click);
            // 
            // writeFloatValue
            // 
            this.writeFloatValue.DecimalPlaces = 1;
            this.writeFloatValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.writeFloatValue.Location = new System.Drawing.Point(645, 142);
            this.writeFloatValue.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.writeFloatValue.Minimum = new decimal(new int[] {
            34000,
            0,
            0,
            -2147483648});
            this.writeFloatValue.Name = "writeFloatValue";
            this.writeFloatValue.Size = new System.Drawing.Size(120, 20);
            this.writeFloatValue.TabIndex = 24;
            this.writeFloatValue.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(644, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Write 32bit Float (0x10)";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // writeFloat
            // 
            this.writeFloat.Location = new System.Drawing.Point(645, 165);
            this.writeFloat.Name = "writeFloat";
            this.writeFloat.Size = new System.Drawing.Size(83, 20);
            this.writeFloat.TabIndex = 26;
            this.writeFloat.Text = "Write";
            this.writeFloat.UseVisualStyleBackColor = true;
            this.writeFloat.Click += new System.EventHandler(this.writeFloat_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(841, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Value";
            // 
            // read32Value
            // 
            this.read32Value.Location = new System.Drawing.Point(881, 77);
            this.read32Value.Name = "read32Value";
            this.read32Value.Size = new System.Drawing.Size(100, 20);
            this.read32Value.TabIndex = 28;
            // 
            // readFloat
            // 
            this.readFloat.Location = new System.Drawing.Point(838, 138);
            this.readFloat.Name = "readFloat";
            this.readFloat.Size = new System.Drawing.Size(75, 23);
            this.readFloat.TabIndex = 29;
            this.readFloat.Text = "Read";
            this.readFloat.UseVisualStyleBackColor = true;
            this.readFloat.Click += new System.EventHandler(this.readFloat_Click);
            // 
            // read32FloatValue
            // 
            this.read32FloatValue.Location = new System.Drawing.Point(833, 186);
            this.read32FloatValue.Name = "read32FloatValue";
            this.read32FloatValue.Size = new System.Drawing.Size(100, 20);
            this.read32FloatValue.TabIndex = 30;
            this.read32FloatValue.TextChanged += new System.EventHandler(this.read32FloatValue_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(769, 198);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Value";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(830, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(122, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Read 32Bit Value (0x04)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(835, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(118, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Read 32Bit Float (0x04)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(837, 243);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 13);
            this.label15.TabIndex = 40;
            this.label15.Text = "Read 32Bit Value (0x03)";
            // 
            // read32InFloatValue
            // 
            this.read32InFloatValue.Location = new System.Drawing.Point(888, 299);
            this.read32InFloatValue.Name = "read32InFloatValue";
            this.read32InFloatValue.Size = new System.Drawing.Size(100, 20);
            this.read32InFloatValue.TabIndex = 39;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(848, 306);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "Value";
            // 
            // readInFloat
            // 
            this.readInFloat.Location = new System.Drawing.Point(837, 266);
            this.readInFloat.Name = "readInFloat";
            this.readInFloat.Size = new System.Drawing.Size(75, 23);
            this.readInFloat.TabIndex = 37;
            this.readInFloat.Text = "Read";
            this.readInFloat.UseVisualStyleBackColor = true;
            this.readInFloat.Click += new System.EventHandler(this.readInFloat_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(652, 292);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "Write";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(652, 266);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 35;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(608, 244);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(121, 13);
            this.label17.TabIndex = 34;
            this.label17.Text = "Write 32Bit Value (0x10)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 426);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.read32InFloatValue);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.readInFloat);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.read32FloatValue);
            this.Controls.Add(this.readFloat);
            this.Controls.Add(this.read32Value);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.writeFloat);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.writeFloatValue);
            this.Controls.Add(this.read32);
            this.Controls.Add(this.write32);
            this.Controls.Add(this.write32Value);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.write16Value);
            this.Controls.Add(this.read16Value);
            this.Controls.Add(this.readBitValue);
            this.Controls.Add(this.MessageSent);
            this.Controls.Add(this.MessageRecieved);
            this.Controls.Add(this.read16);
            this.Controls.Add(this.write16);
            this.Controls.Add(this.readBit);
            this.Controls.Add(this.write1);
            this.Controls.Add(this.write0);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.write16Value)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.write32Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeFloatValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button write0;
        private System.Windows.Forms.Button write1;
        private System.Windows.Forms.Button readBit;
        private System.Windows.Forms.Button write16;
        private System.Windows.Forms.Button read16;
        private System.Windows.Forms.TextBox MessageRecieved;
        private System.Windows.Forms.TextBox MessageSent;
        private System.Windows.Forms.TextBox readBitValue;
        private System.Windows.Forms.TextBox read16Value;
        private System.Windows.Forms.NumericUpDown write16Value;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel PLCStatusLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown write32Value;
        private System.Windows.Forms.Button write32;
        private System.Windows.Forms.Button read32;
        private System.Windows.Forms.NumericUpDown writeFloatValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button writeFloat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox read32Value;
        private System.Windows.Forms.Button readFloat;
        private System.Windows.Forms.TextBox read32FloatValue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox read32InFloatValue;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button readInFloat;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label17;
    }
}

