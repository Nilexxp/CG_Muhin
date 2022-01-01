namespace FractTreeWinF
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelStartAngle = new System.Windows.Forms.Label();
            this.labelSubAngle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxStartAngle = new System.Windows.Forms.TextBox();
            this.textBoxSubAngle = new System.Windows.Forms.TextBox();
            this.textBoxIter = new System.Windows.Forms.TextBox();
            this.buttonGenTree = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(939, 541);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelStartAngle
            // 
            this.labelStartAngle.AutoSize = true;
            this.labelStartAngle.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStartAngle.Location = new System.Drawing.Point(12, 561);
            this.labelStartAngle.Name = "labelStartAngle";
            this.labelStartAngle.Size = new System.Drawing.Size(121, 19);
            this.labelStartAngle.TabIndex = 1;
            this.labelStartAngle.Text = "Начальный угол";
            // 
            // labelSubAngle
            // 
            this.labelSubAngle.AutoSize = true;
            this.labelSubAngle.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSubAngle.Location = new System.Drawing.Point(327, 561);
            this.labelSubAngle.Name = "labelSubAngle";
            this.labelSubAngle.Size = new System.Drawing.Size(129, 19);
            this.labelSubAngle.TabIndex = 1;
            this.labelSubAngle.Text = "Угол отклонений";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(642, 561);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ограничение итераций";
            // 
            // textBoxStartAngle
            // 
            this.textBoxStartAngle.Font = new System.Drawing.Font("GOST Type BU", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxStartAngle.Location = new System.Drawing.Point(139, 559);
            this.textBoxStartAngle.Name = "textBoxStartAngle";
            this.textBoxStartAngle.Size = new System.Drawing.Size(182, 25);
            this.textBoxStartAngle.TabIndex = 2;
            // 
            // textBoxSubAngle
            // 
            this.textBoxSubAngle.Font = new System.Drawing.Font("GOST Type BU", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxSubAngle.Location = new System.Drawing.Point(462, 559);
            this.textBoxSubAngle.Name = "textBoxSubAngle";
            this.textBoxSubAngle.Size = new System.Drawing.Size(174, 25);
            this.textBoxSubAngle.TabIndex = 2;
            // 
            // textBoxIter
            // 
            this.textBoxIter.Font = new System.Drawing.Font("GOST Type BU", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxIter.Location = new System.Drawing.Point(818, 559);
            this.textBoxIter.Name = "textBoxIter";
            this.textBoxIter.Size = new System.Drawing.Size(133, 25);
            this.textBoxIter.TabIndex = 2;
            // 
            // buttonGenTree
            // 
            this.buttonGenTree.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGenTree.Location = new System.Drawing.Point(12, 587);
            this.buttonGenTree.Name = "buttonGenTree";
            this.buttonGenTree.Size = new System.Drawing.Size(309, 27);
            this.buttonGenTree.TabIndex = 3;
            this.buttonGenTree.Text = "Сгенерировать дерево";
            this.buttonGenTree.UseVisualStyleBackColor = true;
            this.buttonGenTree.Click += new System.EventHandler(this.buttonGenTree_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSave.Location = new System.Drawing.Point(327, 587);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(309, 27);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Сохранить дерево";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSaveTree_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Font = new System.Drawing.Font("GOST Type BU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoad.Location = new System.Drawing.Point(642, 587);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(309, 27);
            this.buttonLoad.TabIndex = 3;
            this.buttonLoad.Text = "Загрузить дерево";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoadTree_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 626);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonGenTree);
            this.Controls.Add(this.textBoxIter);
            this.Controls.Add(this.textBoxSubAngle);
            this.Controls.Add(this.textBoxStartAngle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSubAngle);
            this.Controls.Add(this.labelStartAngle);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Дерево";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelStartAngle;
        private System.Windows.Forms.Label labelSubAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStartAngle;
        private System.Windows.Forms.TextBox textBoxSubAngle;
        private System.Windows.Forms.TextBox textBoxIter;
        private System.Windows.Forms.Button buttonGenTree;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

