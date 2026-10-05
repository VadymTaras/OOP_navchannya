namespace Lab06_MDI;

partial class ChildDocumentForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.RichTextBox rtbEditor;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.rtbEditor = new System.Windows.Forms.RichTextBox();
        this.SuspendLayout();

        // rtbEditor
        this.rtbEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.rtbEditor.Dock = System.Windows.Forms.DockStyle.Fill;
        this.rtbEditor.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.rtbEditor.Location = new System.Drawing.Point(0, 0);
        this.rtbEditor.Name = "rtbEditor";
        this.rtbEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
        this.rtbEditor.Size = new System.Drawing.Size(700, 450);
        this.rtbEditor.TabIndex = 0;
        this.rtbEditor.Text = "";
        this.rtbEditor.TextChanged += new System.EventHandler(this.RtbEditor_TextChanged);
        this.rtbEditor.SelectionChanged += new System.EventHandler(this.RtbEditor_SelectionChanged);

        // ChildDocumentForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(700, 450);
        this.Controls.Add(this.rtbEditor);
        this.Name = "ChildDocumentForm";
        this.Text = "Документ";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChildDocumentForm_FormClosing);
        this.ResumeLayout(false);
    }
}
