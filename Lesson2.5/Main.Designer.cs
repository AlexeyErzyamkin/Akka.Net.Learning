namespace Lesson
{
    partial class Main
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_DiskToggle = new System.Windows.Forms.Button();
            this.button_MemoryToggle = new System.Windows.Forms.Button();
            this.button_CpuToggle = new System.Windows.Forms.Button();
            this.button_Pause = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea2.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea2);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.sysChart.Legends.Add(legend2);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Name = "sysChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.sysChart.Series.Add(series2);
            this.sysChart.Size = new System.Drawing.Size(684, 446);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // button_DiskToggle
            // 
            this.button_DiskToggle.Location = new System.Drawing.Point(560, 411);
            this.button_DiskToggle.Name = "button_DiskToggle";
            this.button_DiskToggle.Size = new System.Drawing.Size(112, 23);
            this.button_DiskToggle.TabIndex = 1;
            this.button_DiskToggle.Text = "Disk";
            this.button_DiskToggle.UseVisualStyleBackColor = true;
            this.button_DiskToggle.Click += new System.EventHandler(this.button_DiskToggle_Click);
            // 
            // button_MemoryToggle
            // 
            this.button_MemoryToggle.Location = new System.Drawing.Point(560, 382);
            this.button_MemoryToggle.Name = "button_MemoryToggle";
            this.button_MemoryToggle.Size = new System.Drawing.Size(112, 23);
            this.button_MemoryToggle.TabIndex = 2;
            this.button_MemoryToggle.Text = "Memory";
            this.button_MemoryToggle.UseVisualStyleBackColor = true;
            this.button_MemoryToggle.Click += new System.EventHandler(this.button_MemoryToggle_Click);
            // 
            // button_CpuToggle
            // 
            this.button_CpuToggle.Location = new System.Drawing.Point(560, 353);
            this.button_CpuToggle.Name = "button_CpuToggle";
            this.button_CpuToggle.Size = new System.Drawing.Size(112, 23);
            this.button_CpuToggle.TabIndex = 3;
            this.button_CpuToggle.Text = "CPU";
            this.button_CpuToggle.UseVisualStyleBackColor = true;
            this.button_CpuToggle.Click += new System.EventHandler(this.button_CpuToggle_Click);
            // 
            // button_Pause
            // 
            this.button_Pause.Location = new System.Drawing.Point(560, 291);
            this.button_Pause.Name = "button_Pause";
            this.button_Pause.Size = new System.Drawing.Size(112, 23);
            this.button_Pause.TabIndex = 4;
            this.button_Pause.Text = "Pause";
            this.button_Pause.UseVisualStyleBackColor = true;
            this.button_Pause.Click += new System.EventHandler(this.button_Pause_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 446);
            this.Controls.Add(this.button_Pause);
            this.Controls.Add(this.button_CpuToggle);
            this.Controls.Add(this.button_MemoryToggle);
            this.Controls.Add(this.button_DiskToggle);
            this.Controls.Add(this.sysChart);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart sysChart;
        private System.Windows.Forms.Button button_DiskToggle;
        private System.Windows.Forms.Button button_MemoryToggle;
        private System.Windows.Forms.Button button_CpuToggle;
        private System.Windows.Forms.Button button_Pause;
    }
}

