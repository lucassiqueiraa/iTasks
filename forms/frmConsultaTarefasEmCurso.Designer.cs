namespace iTasks
{
    partial class frmConsultaTarefasEmCurso
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
            this.gvTarefasEmCurso = new System.Windows.Forms.DataGridView();
            this.btFechar = new System.Windows.Forms.Button();
            this.TempoEmFaltaStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AtrasoStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarefasEmCurso)).BeginInit();
            this.SuspendLayout();
            // 
            // gvTarefasEmCurso
            // 
            this.gvTarefasEmCurso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTarefasEmCurso.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TempoEmFaltaStr,
            this.AtrasoStr});
            this.gvTarefasEmCurso.Location = new System.Drawing.Point(16, 15);
            this.gvTarefasEmCurso.Margin = new System.Windows.Forms.Padding(4);
            this.gvTarefasEmCurso.Name = "gvTarefasEmCurso";
            this.gvTarefasEmCurso.RowHeadersWidth = 51;
            this.gvTarefasEmCurso.Size = new System.Drawing.Size(1368, 486);
            this.gvTarefasEmCurso.TabIndex = 0;
            // 
            // btFechar
            // 
            this.btFechar.Location = new System.Drawing.Point(1245, 511);
            this.btFechar.Margin = new System.Windows.Forms.Padding(4);
            this.btFechar.Name = "btFechar";
            this.btFechar.Size = new System.Drawing.Size(139, 28);
            this.btFechar.TabIndex = 30;
            this.btFechar.Text = "Fechar";
            this.btFechar.UseVisualStyleBackColor = true;
            this.btFechar.Click += new System.EventHandler(this.btFechar_Click);
            // 
            // TempoEmFaltaStr
            // 
            this.TempoEmFaltaStr.HeaderText = "Tempo em falta";
            this.TempoEmFaltaStr.MinimumWidth = 6;
            this.TempoEmFaltaStr.Name = "TempoEmFaltaStr";
            this.TempoEmFaltaStr.Width = 125;
            // 
            // AtrasoStr
            // 
            this.AtrasoStr.HeaderText = "Atraso";
            this.AtrasoStr.MinimumWidth = 6;
            this.AtrasoStr.Name = "AtrasoStr";
            this.AtrasoStr.Width = 125;
            // 
            // frmConsultaTarefasEmCurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 554);
            this.Controls.Add(this.btFechar);
            this.Controls.Add(this.gvTarefasEmCurso);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmConsultaTarefasEmCurso";
            this.Text = "frmConsultaTarefasEmCurso";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConsultaTarefasEmCurso_FormClosing);
            this.Load += new System.EventHandler(this.frmConsultaTarefasEmCurso_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvTarefasEmCurso)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvTarefasEmCurso;
        private System.Windows.Forms.Button btFechar;
        private System.Windows.Forms.DataGridViewTextBoxColumn TempoEmFaltaStr;
        private System.Windows.Forms.DataGridViewTextBoxColumn AtrasoStr;
    }
}