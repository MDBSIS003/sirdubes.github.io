namespace FuturePathStudentAdvisingSystem
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            grpRole = new GroupBox();
            rdoGuest = new RadioButton();
            rdoLearner = new RadioButton();
            rdoAdvisor = new RadioButton();
            rdoAdmin = new RadioButton();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            lblMessage = new Label();
            btnLogin = new Button();
            btnExit = new Button();
            grpRole.SuspendLayout();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Future Path";
            //
            // lblSubtitle
            //
            lblSubtitle.Location = new Point(32, 70);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(380, 36);
            lblSubtitle.Text = "Student Advising System. Choose how you would like to continue.";
            //
            // grpRole
            //
            grpRole.Controls.Add(rdoGuest);
            grpRole.Controls.Add(rdoLearner);
            grpRole.Controls.Add(rdoAdvisor);
            grpRole.Controls.Add(rdoAdmin);
            grpRole.Location = new Point(30, 112);
            grpRole.Name = "grpRole";
            grpRole.Size = new Size(380, 120);
            grpRole.TabStop = false;
            grpRole.Text = "I am a...";
            //
            // rdoGuest
            //
            rdoGuest.AutoSize = true;
            rdoGuest.Location = new Point(20, 30);
            rdoGuest.Name = "rdoGuest";
            rdoGuest.TabIndex = 0;
            rdoGuest.Text = "Guest";
            //
            // rdoLearner
            //
            rdoLearner.AutoSize = true;
            rdoLearner.Location = new Point(200, 30);
            rdoLearner.Name = "rdoLearner";
            rdoLearner.TabIndex = 1;
            rdoLearner.Text = "Learner";
            //
            // rdoAdvisor
            //
            rdoAdvisor.AutoSize = true;
            rdoAdvisor.Location = new Point(20, 75);
            rdoAdvisor.Name = "rdoAdvisor";
            rdoAdvisor.TabIndex = 2;
            rdoAdvisor.Text = "Career Advisor";
            //
            // rdoAdmin
            //
            rdoAdmin.AutoSize = true;
            rdoAdmin.Location = new Point(200, 75);
            rdoAdmin.Name = "rdoAdmin";
            rdoAdmin.TabIndex = 3;
            rdoAdmin.Text = "System Administrator";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(30, 252);
            lblUsername.Name = "lblUsername";
            lblUsername.Text = "Username";
            //
            // txtUsername
            //
            txtUsername.Location = new Point(30, 274);
            txtUsername.MaxLength = 50;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(380, 27);
            txtUsername.TabIndex = 1;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(30, 314);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "Password";
            //
            // txtPassword
            //
            txtPassword.Location = new Point(30, 336);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(380, 27);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            //
            // chkShowPassword
            //
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(30, 372);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.TabIndex = 3;
            chkShowPassword.Text = "Show password";
            //
            // lblMessage
            //
            lblMessage.ForeColor = Color.Firebrick;
            lblMessage.Location = new Point(30, 404);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(380, 40);
            //
            // btnLogin
            //
            btnLogin.Location = new Point(30, 456);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(180, 40);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Log In";
            btnLogin.UseVisualStyleBackColor = true;
            //
            // btnExit
            //
            btnExit.Location = new Point(230, 456);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(180, 40);
            btnExit.TabIndex = 5;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            //
            // frmLogin
            //
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 520);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(grpRole);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(chkShowPassword);
            Controls.Add(lblMessage);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Future Path - Login";
            grpRole.ResumeLayout(false);
            grpRole.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSubtitle;
        private GroupBox grpRole;
        private RadioButton rdoGuest;
        private RadioButton rdoLearner;
        private RadioButton rdoAdvisor;
        private RadioButton rdoAdmin;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Label lblMessage;
        private Button btnLogin;
        private Button btnExit;
    }
}
