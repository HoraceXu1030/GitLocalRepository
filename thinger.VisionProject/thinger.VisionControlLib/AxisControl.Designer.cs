
namespace thinger.VisionControlLib
{
    partial class AxisControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_Distance = new System.Windows.Forms.TextBox();
            this.lbl_AxisName = new System.Windows.Forms.Label();
            this.btn_Move = new System.Windows.Forms.Button();
            this.btn_Zero = new System.Windows.Forms.Button();
            this.chk_Absolute = new thinger.VisionControlLib.CheckBoxEx();
            this.SuspendLayout();
            // 
            // txt_Distance
            // 
            this.txt_Distance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Distance.Location = new System.Drawing.Point(190, 14);
            this.txt_Distance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txt_Distance.Name = "txt_Distance";
            this.txt_Distance.Size = new System.Drawing.Size(79, 26);
            this.txt_Distance.TabIndex = 25;
            this.txt_Distance.Text = "10";
            this.txt_Distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_AxisName
            // 
            this.lbl_AxisName.AutoSize = true;
            this.lbl_AxisName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_AxisName.Location = new System.Drawing.Point(29, 17);
            this.lbl_AxisName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_AxisName.Name = "lbl_AxisName";
            this.lbl_AxisName.Size = new System.Drawing.Size(46, 20);
            this.lbl_AxisName.TabIndex = 24;
            this.lbl_AxisName.Text = "X轴：";
            // 
            // btn_Move
            // 
            this.btn_Move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Move.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Move.Image = global::thinger.VisionControlLib.Properties.Resources.move;
            this.btn_Move.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Move.Location = new System.Drawing.Point(292, 8);
            this.btn_Move.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Move.Name = "btn_Move";
            this.btn_Move.Size = new System.Drawing.Size(93, 40);
            this.btn_Move.TabIndex = 22;
            this.btn_Move.Tag = "";
            this.btn_Move.Text = "  运动";
            this.btn_Move.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Move.UseVisualStyleBackColor = true;
            this.btn_Move.Click += new System.EventHandler(this.btn_Move_Click);
            // 
            // btn_Zero
            // 
            this.btn_Zero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Zero.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Zero.Image = global::thinger.VisionControlLib.Properties.Resources.reset;
            this.btn_Zero.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Zero.Location = new System.Drawing.Point(83, 8);
            this.btn_Zero.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Zero.Name = "btn_Zero";
            this.btn_Zero.Size = new System.Drawing.Size(93, 40);
            this.btn_Zero.TabIndex = 23;
            this.btn_Zero.Tag = "";
            this.btn_Zero.Text = "  清零";
            this.btn_Zero.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Zero.UseVisualStyleBackColor = true;
            this.btn_Zero.Click += new System.EventHandler(this.btn_Zero_Click);
            // 
            // chk_Absolute
            // 
            this.chk_Absolute.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(161)))), ((int)(((byte)(224)))));
            this.chk_Absolute.Location = new System.Drawing.Point(411, 17);
            this.chk_Absolute.Name = "chk_Absolute";
            this.chk_Absolute.Size = new System.Drawing.Size(26, 23);
            this.chk_Absolute.TabIndex = 26;
            this.chk_Absolute.UseVisualStyleBackColor = true;
            // 
            // AxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.Controls.Add(this.chk_Absolute);
            this.Controls.Add(this.txt_Distance);
            this.Controls.Add(this.lbl_AxisName);
            this.Controls.Add(this.btn_Move);
            this.Controls.Add(this.btn_Zero);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "AxisControl";
            this.Size = new System.Drawing.Size(460, 55);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Distance;
        private System.Windows.Forms.Label lbl_AxisName;
        private System.Windows.Forms.Button btn_Move;
        private System.Windows.Forms.Button btn_Zero;
        private CheckBoxEx chk_Absolute;
    }
}
