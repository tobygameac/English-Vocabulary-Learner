namespace EnglishVocabularyLearner
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
      this.buttonFileRead = new System.Windows.Forms.Button();
      this.richTextBoxQuestion = new System.Windows.Forms.RichTextBox();
      this.labelFileReadStatus = new System.Windows.Forms.Label();
      this.textBoxNumberOfTest1 = new System.Windows.Forms.TextBox();
      this.textBoxNumberOfTest2 = new System.Windows.Forms.TextBox();
      this.labelWord2 = new System.Windows.Forms.Label();
      this.labelWord4 = new System.Windows.Forms.Label();
      this.labelWord1 = new System.Windows.Forms.Label();
      this.labelWord3 = new System.Windows.Forms.Label();
      this.buttonStartTest1 = new System.Windows.Forms.Button();
      this.buttonStartTest2 = new System.Windows.Forms.Button();
      this.checkBoxAutoClose = new System.Windows.Forms.CheckBox();
      this.groupBoxMainOperation = new System.Windows.Forms.GroupBox();
      this.groupBoxFileRead = new System.Windows.Forms.GroupBox();
      this.groupBoxTest = new System.Windows.Forms.GroupBox();
      this.richTextBoxExapmle = new System.Windows.Forms.RichTextBox();
      this.buttonFinishTest = new System.Windows.Forms.Button();
      this.textBoxAnswer = new System.Windows.Forms.TextBox();
      this.richTextBoxQuestionDone = new System.Windows.Forms.RichTextBox();
      this.buttonPrevQuestion = new System.Windows.Forms.Button();
      this.buttonNextQuestion = new System.Windows.Forms.Button();
      this.groupBoxMainOperation.SuspendLayout();
      this.groupBoxFileRead.SuspendLayout();
      this.groupBoxTest.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonFileRead
      // 
      this.buttonFileRead.Location = new System.Drawing.Point(10, 21);
      this.buttonFileRead.Name = "buttonFileRead";
      this.buttonFileRead.Size = new System.Drawing.Size(75, 23);
      this.buttonFileRead.TabIndex = 3;
      this.buttonFileRead.Text = "讀取單字庫";
      this.buttonFileRead.UseVisualStyleBackColor = true;
      // 
      // richTextBoxQuestion
      // 
      this.richTextBoxQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.richTextBoxQuestion.Font = new System.Drawing.Font("PMingLiU", 20F);
      this.richTextBoxQuestion.Location = new System.Drawing.Point(10, 17);
      this.richTextBoxQuestion.Name = "richTextBoxQuestion";
      this.richTextBoxQuestion.ReadOnly = true;
      this.richTextBoxQuestion.Size = new System.Drawing.Size(373, 262);
      this.richTextBoxQuestion.TabIndex = 4;
      this.richTextBoxQuestion.Text = "";
      // 
      // labelFileReadStatus
      // 
      this.labelFileReadStatus.AutoSize = true;
      this.labelFileReadStatus.Location = new System.Drawing.Point(91, 26);
      this.labelFileReadStatus.Name = "labelFileReadStatus";
      this.labelFileReadStatus.Size = new System.Drawing.Size(77, 12);
      this.labelFileReadStatus.TabIndex = 6;
      this.labelFileReadStatus.Text = "檔案讀取狀態";
      // 
      // textBoxNumberOfTest1
      // 
      this.textBoxNumberOfTest1.Location = new System.Drawing.Point(90, 20);
      this.textBoxNumberOfTest1.Name = "textBoxNumberOfTest1";
      this.textBoxNumberOfTest1.Size = new System.Drawing.Size(29, 22);
      this.textBoxNumberOfTest1.TabIndex = 7;
      this.textBoxNumberOfTest1.Text = "10";
      // 
      // textBoxNumberOfTest2
      // 
      this.textBoxNumberOfTest2.Location = new System.Drawing.Point(90, 48);
      this.textBoxNumberOfTest2.Name = "textBoxNumberOfTest2";
      this.textBoxNumberOfTest2.Size = new System.Drawing.Size(29, 22);
      this.textBoxNumberOfTest2.TabIndex = 8;
      this.textBoxNumberOfTest2.Text = "3";
      // 
      // labelWord2
      // 
      this.labelWord2.AutoSize = true;
      this.labelWord2.Location = new System.Drawing.Point(125, 23);
      this.labelWord2.Name = "labelWord2";
      this.labelWord2.Size = new System.Drawing.Size(17, 12);
      this.labelWord2.TabIndex = 9;
      this.labelWord2.Text = "題";
      // 
      // labelWord4
      // 
      this.labelWord4.AutoSize = true;
      this.labelWord4.Location = new System.Drawing.Point(125, 51);
      this.labelWord4.Name = "labelWord4";
      this.labelWord4.Size = new System.Drawing.Size(17, 12);
      this.labelWord4.TabIndex = 10;
      this.labelWord4.Text = "題";
      // 
      // labelWord1
      // 
      this.labelWord1.AutoSize = true;
      this.labelWord1.Location = new System.Drawing.Point(55, 23);
      this.labelWord1.Name = "labelWord1";
      this.labelWord1.Size = new System.Drawing.Size(29, 12);
      this.labelWord1.TabIndex = 11;
      this.labelWord1.Text = "測驗";
      // 
      // labelWord3
      // 
      this.labelWord3.AutoSize = true;
      this.labelWord3.Location = new System.Drawing.Point(19, 51);
      this.labelWord3.Name = "labelWord3";
      this.labelWord3.Size = new System.Drawing.Size(65, 12);
      this.labelWord3.TabIndex = 12;
      this.labelWord3.Text = "測驗至答錯";
      // 
      // buttonStartTest1
      // 
      this.buttonStartTest1.Location = new System.Drawing.Point(149, 18);
      this.buttonStartTest1.Name = "buttonStartTest1";
      this.buttonStartTest1.Size = new System.Drawing.Size(75, 23);
      this.buttonStartTest1.TabIndex = 13;
      this.buttonStartTest1.Text = "Go!";
      this.buttonStartTest1.UseVisualStyleBackColor = true;
      // 
      // buttonStartTest2
      // 
      this.buttonStartTest2.Location = new System.Drawing.Point(149, 48);
      this.buttonStartTest2.Name = "buttonStartTest2";
      this.buttonStartTest2.Size = new System.Drawing.Size(75, 23);
      this.buttonStartTest2.TabIndex = 14;
      this.buttonStartTest2.Text = "Go!";
      this.buttonStartTest2.UseVisualStyleBackColor = true;
      // 
      // checkBoxAutoClose
      // 
      this.checkBoxAutoClose.AutoSize = true;
      this.checkBoxAutoClose.Checked = true;
      this.checkBoxAutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxAutoClose.Location = new System.Drawing.Point(10, 76);
      this.checkBoxAutoClose.Name = "checkBoxAutoClose";
      this.checkBoxAutoClose.Size = new System.Drawing.Size(132, 16);
      this.checkBoxAutoClose.TabIndex = 15;
      this.checkBoxAutoClose.Text = "測驗後自動關閉程式";
      this.checkBoxAutoClose.UseVisualStyleBackColor = true;
      // 
      // groupBoxMainOperation
      // 
      this.groupBoxMainOperation.Controls.Add(this.labelWord4);
      this.groupBoxMainOperation.Controls.Add(this.textBoxNumberOfTest2);
      this.groupBoxMainOperation.Controls.Add(this.labelWord2);
      this.groupBoxMainOperation.Controls.Add(this.buttonStartTest1);
      this.groupBoxMainOperation.Controls.Add(this.checkBoxAutoClose);
      this.groupBoxMainOperation.Controls.Add(this.buttonStartTest2);
      this.groupBoxMainOperation.Controls.Add(this.textBoxNumberOfTest1);
      this.groupBoxMainOperation.Controls.Add(this.labelWord3);
      this.groupBoxMainOperation.Controls.Add(this.labelWord1);
      this.groupBoxMainOperation.Location = new System.Drawing.Point(12, 89);
      this.groupBoxMainOperation.Name = "groupBoxMainOperation";
      this.groupBoxMainOperation.Size = new System.Drawing.Size(232, 105);
      this.groupBoxMainOperation.TabIndex = 16;
      this.groupBoxMainOperation.TabStop = false;
      this.groupBoxMainOperation.Visible = false;
      // 
      // groupBoxFileRead
      // 
      this.groupBoxFileRead.AutoSize = true;
      this.groupBoxFileRead.Controls.Add(this.buttonFileRead);
      this.groupBoxFileRead.Controls.Add(this.labelFileReadStatus);
      this.groupBoxFileRead.Location = new System.Drawing.Point(12, 18);
      this.groupBoxFileRead.Name = "groupBoxFileRead";
      this.groupBoxFileRead.Size = new System.Drawing.Size(232, 65);
      this.groupBoxFileRead.TabIndex = 17;
      this.groupBoxFileRead.TabStop = false;
      // 
      // groupBoxTest
      // 
      this.groupBoxTest.AutoSize = true;
      this.groupBoxTest.Controls.Add(this.richTextBoxExapmle);
      this.groupBoxTest.Controls.Add(this.buttonFinishTest);
      this.groupBoxTest.Controls.Add(this.textBoxAnswer);
      this.groupBoxTest.Controls.Add(this.richTextBoxQuestionDone);
      this.groupBoxTest.Controls.Add(this.buttonPrevQuestion);
      this.groupBoxTest.Controls.Add(this.buttonNextQuestion);
      this.groupBoxTest.Controls.Add(this.richTextBoxQuestion);
      this.groupBoxTest.Location = new System.Drawing.Point(12, 18);
      this.groupBoxTest.Name = "groupBoxTest";
      this.groupBoxTest.Size = new System.Drawing.Size(1007, 410);
      this.groupBoxTest.TabIndex = 18;
      this.groupBoxTest.TabStop = false;
      this.groupBoxTest.Visible = false;
      // 
      // richTextBoxExapmle
      // 
      this.richTextBoxExapmle.Location = new System.Drawing.Point(389, 17);
      this.richTextBoxExapmle.Name = "richTextBoxExapmle";
      this.richTextBoxExapmle.ReadOnly = true;
      this.richTextBoxExapmle.Size = new System.Drawing.Size(493, 372);
      this.richTextBoxExapmle.TabIndex = 8;
      this.richTextBoxExapmle.Text = "";
      // 
      // buttonFinishTest
      // 
      this.buttonFinishTest.Location = new System.Drawing.Point(283, 366);
      this.buttonFinishTest.Name = "buttonFinishTest";
      this.buttonFinishTest.Size = new System.Drawing.Size(100, 23);
      this.buttonFinishTest.TabIndex = 7;
      this.buttonFinishTest.Text = "Finish";
      this.buttonFinishTest.UseVisualStyleBackColor = true;
      // 
      // textBoxAnswer
      // 
      this.textBoxAnswer.Font = new System.Drawing.Font("PMingLiU", 20F);
      this.textBoxAnswer.Location = new System.Drawing.Point(10, 285);
      this.textBoxAnswer.Name = "textBoxAnswer";
      this.textBoxAnswer.Size = new System.Drawing.Size(373, 39);
      this.textBoxAnswer.TabIndex = 5;
      // 
      // richTextBoxQuestionDone
      // 
      this.richTextBoxQuestionDone.Location = new System.Drawing.Point(888, 17);
      this.richTextBoxQuestionDone.Name = "richTextBoxQuestionDone";
      this.richTextBoxQuestionDone.ReadOnly = true;
      this.richTextBoxQuestionDone.Size = new System.Drawing.Size(100, 372);
      this.richTextBoxQuestionDone.TabIndex = 6;
      this.richTextBoxQuestionDone.Text = "";
      // 
      // buttonPrevQuestion
      // 
      this.buttonPrevQuestion.Location = new System.Drawing.Point(93, 330);
      this.buttonPrevQuestion.Name = "buttonPrevQuestion";
      this.buttonPrevQuestion.Size = new System.Drawing.Size(75, 23);
      this.buttonPrevQuestion.TabIndex = 4;
      this.buttonPrevQuestion.Text = "<- Prev";
      this.buttonPrevQuestion.UseVisualStyleBackColor = true;
      // 
      // buttonNextQuestion
      // 
      this.buttonNextQuestion.Location = new System.Drawing.Point(174, 330);
      this.buttonNextQuestion.Name = "buttonNextQuestion";
      this.buttonNextQuestion.Size = new System.Drawing.Size(75, 23);
      this.buttonNextQuestion.TabIndex = 3;
      this.buttonNextQuestion.Text = "Next ->";
      this.buttonNextQuestion.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1031, 433);
      this.Controls.Add(this.groupBoxFileRead);
      this.Controls.Add(this.groupBoxMainOperation);
      this.Controls.Add(this.groupBoxTest);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBoxMainOperation.ResumeLayout(false);
      this.groupBoxMainOperation.PerformLayout();
      this.groupBoxFileRead.ResumeLayout(false);
      this.groupBoxFileRead.PerformLayout();
      this.groupBoxTest.ResumeLayout(false);
      this.groupBoxTest.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonFileRead;
    private System.Windows.Forms.RichTextBox richTextBoxQuestion;
    private System.Windows.Forms.Label labelFileReadStatus;
    private System.Windows.Forms.TextBox textBoxNumberOfTest1;
    private System.Windows.Forms.TextBox textBoxNumberOfTest2;
    private System.Windows.Forms.Label labelWord2;
    private System.Windows.Forms.Label labelWord4;
    private System.Windows.Forms.Label labelWord1;
    private System.Windows.Forms.Label labelWord3;
    private System.Windows.Forms.Button buttonStartTest1;
    private System.Windows.Forms.Button buttonStartTest2;
    private System.Windows.Forms.CheckBox checkBoxAutoClose;
    private System.Windows.Forms.GroupBox groupBoxMainOperation;
    private System.Windows.Forms.GroupBox groupBoxFileRead;
    private System.Windows.Forms.GroupBox groupBoxTest;
    private System.Windows.Forms.Button buttonPrevQuestion;
    private System.Windows.Forms.Button buttonNextQuestion;
    private System.Windows.Forms.TextBox textBoxAnswer;
    private System.Windows.Forms.RichTextBox richTextBoxQuestionDone;
    private System.Windows.Forms.Button buttonFinishTest;
    private System.Windows.Forms.RichTextBox richTextBoxExapmle;

  }
}

