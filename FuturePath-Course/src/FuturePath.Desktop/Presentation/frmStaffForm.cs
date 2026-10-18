using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuturePath.Desktop
{

    public class frmStaffForm : Form
    {
        private bool hasChanges;
        private bool wasActive;
        protected void TrackChanges(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.TextChanged += EditorChanged;
                }

                if (control is CheckBox check)
                {
                    if (check.Name == "chkActive")
                    {
                        wasActive = check.Checked;
                    }
                    check.CheckedChanged += EditorChanged;
                }

                if (control is NumericUpDown number)
                {
                    number.ValueChanged += EditorChanged;
                }

                if (control is DateTimePicker date)
                {
                    date.ValueChanged += EditorChanged;
                }

                TrackChanges(control);
            }
        }

        protected bool ConfirmDeactivation()
        {
            Control[] controls = Controls.Find("chkActive", true);
            if (wasActive && controls.Length > 0 && controls[0] is CheckBox check && !check.Checked)
            {
                return MessageBox.Show(this, "Deactivate this record?", "Future Path", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }
            return true;
        }

        private void EditorChanged(object? sender, EventArgs e)
        {
            hasChanges = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (hasChanges && DialogResult != DialogResult.OK && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = MessageBox.Show(this, "Discard the unsaved changes in this editor?", "Future Path", MessageBoxButtons.YesNo) != DialogResult.Yes;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (DesignMode || IsMdiChild)
            {
                return;
            }

            Rectangle area = Screen.FromControl(this).WorkingArea;
            Size = new Size(Math.Min(Width, area.Width), Math.Min(Height, area.Height));
            Location = new Point(Math.Max(area.Left, area.Left + (area.Width - Width) / 2), Math.Max(area.Top, area.Top + (area.Height - Height) / 2));
        }
    }
}
