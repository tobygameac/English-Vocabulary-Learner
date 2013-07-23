namespace EnglishVocabularyLearner {
  partial class VocabularyBrowser {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.richTextBoxDefinition = new System.Windows.Forms.RichTextBox();
      this.richTextBoxExample = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // richTextBoxDefinition
      // 
      this.richTextBoxDefinition.Location = new System.Drawing.Point(25, 25);
      this.richTextBoxDefinition.Name = "richTextBoxDefinition";
      this.richTextBoxDefinition.ReadOnly = true;
      this.richTextBoxDefinition.Size = new System.Drawing.Size(500, 225);
      this.richTextBoxDefinition.TabIndex = 0;
      this.richTextBoxDefinition.Text = "";
      // 
      // richTextBoxExample
      // 
      this.richTextBoxExample.Location = new System.Drawing.Point(25, 260);
      this.richTextBoxExample.Name = "richTextBoxExample";
      this.richTextBoxExample.ReadOnly = true;
      this.richTextBoxExample.Size = new System.Drawing.Size(500, 225);
      this.richTextBoxExample.TabIndex = 0;
      this.richTextBoxExample.Text = "";
      // 
      // VocabularyViewer
      // 
      this.Controls.Add(this.richTextBoxDefinition);
      this.Controls.Add(this.richTextBoxExample);
      this.Text = "定義及範例";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBoxDefinition;
    private System.Windows.Forms.RichTextBox richTextBoxExample;
  }
}