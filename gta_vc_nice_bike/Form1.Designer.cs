namespace gta_vc_nice_bike
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
            this.components = new System.ComponentModel.Container();
            this.tmr_scan = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tmr_bike = new System.Windows.Forms.Timer(this.components);
            this.bt_about = new System.Windows.Forms.Button();
            this.lb_nice_bike = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tmr_scan
            // 
            this.tmr_scan.Interval = 1000;
            this.tmr_scan.Tick += new System.EventHandler(this.tmr_scan_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 244);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 100);
            this.label1.TabIndex = 3;
            this.label1.Text = "Leave this running in the background,\r\nthen start or return to the game.\r\n\r\nHagyd" +
    " futni a háttérben, majd\r\nindítsd el vagy lépj vissza a játékba.";
            // 
            // tmr_bike
            // 
            this.tmr_bike.Interval = 200;
            this.tmr_bike.Tick += new System.EventHandler(this.tmr_bike_Tick);
            // 
            // bt_about
            // 
            this.bt_about.Location = new System.Drawing.Point(284, 313);
            this.bt_about.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bt_about.Name = "bt_about";
            this.bt_about.Size = new System.Drawing.Size(103, 31);
            this.bt_about.TabIndex = 1;
            this.bt_about.Text = "About";
            this.bt_about.UseVisualStyleBackColor = true;
            this.bt_about.Click += new System.EventHandler(this.bt_about_Click);
            // 
            // lb_nice_bike
            // 
            this.lb_nice_bike.Image = global::gta_vc_nice_bike.Properties.Resources.nice_bike;
            this.lb_nice_bike.Location = new System.Drawing.Point(0, 0);
            this.lb_nice_bike.Name = "lb_nice_bike";
            this.lb_nice_bike.Size = new System.Drawing.Size(400, 240);
            this.lb_nice_bike.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 361);
            this.Controls.Add(this.lb_nice_bike);
            this.Controls.Add(this.bt_about);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GTA: Vice City - Nice Bike Mod";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmr_scan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmr_bike;
        private System.Windows.Forms.Button bt_about;
        private System.Windows.Forms.Label lb_nice_bike;
    }
}

