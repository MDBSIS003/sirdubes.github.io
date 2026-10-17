using FuturePathStudentAdvisingSystem.Models;
using System.Data;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.UI;

public class ManagementForm : Form
{
    private readonly AdminDAL dal = new();
    private readonly string table;
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    };
    public ManagementForm(string table)
    {
        AccessPolicy.RequireRole(Roles.Admin); this.table = table; Text = "Manage " + table; Size = new(1050, 650); StartPosition = FormStartPosition.CenterParent; Font = Theme.Body(10);
        var commands = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 60, Padding = new(10) };
        void Button(string title, System.Action action)
        { var b = Theme.PrimaryButton(title, Theme.TealDark, 155, 36); b.Click += (_, _) => Safe(action); commands.Controls.Add(b); }
        Button("Refresh", Reload); Button("Add", () => Edit(null)); Button("Edit selected", () => Edit(Selected()));
        Button(table is "UserAccount" or "DegreeProgramme" or "FundingOption" ? "Deactivate" : "Delete", () =>
        {
            var row = Selected(); string key = AdminDAL.Definition(table).Key;
            if (MessageBox.Show("Confirm this change? Referenced records cannot be deleted.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            dal.Delete(table, (int)row[key]); Reload();
        });
        if (table == "UserAccount") Button("Reset password", () =>
        {
            int id = (int)Selected()["UserID"];
            using var form = new Form { Text = "Reset password", ClientSize = new(450, 180), StartPosition = FormStartPosition.CenterParent };
            var p = new TextBox { UseSystemPasswordChar = true, MaxLength = 128, Width = 410, Location = new(15, 20), PlaceholderText = "New temporary password (10–128 characters)" };
            var confirm = new TextBox { UseSystemPasswordChar = true, MaxLength = 128, Width = 410, Location = new(15, 60), PlaceholderText = "Confirm password" };
            var save = new Button { Text = "Reset", Location = new(15, 105), Width = 130 };
            save.Click += (_, _) => Safe(() => { if (p.Text != confirm.Text) throw new ArgumentException("Passwords do not match."); dal.ResetPassword(id, p.Text); form.Close(); });
            form.Controls.AddRange([p, confirm, save]); form.ShowDialog(this);
        });
        Controls.Add(grid); Controls.Add(commands); Shown += (_, _) => Safe(Reload);
    }
    private DataRow Selected() => grid.CurrentRow?.DataBoundItem is DataRowView v ? v.Row : throw new ArgumentException("Select a record.");
    private void Reload() => grid.DataSource = dal.List(table);
    private static void Safe(System.Action action)
    {
        try { action(); }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 2601 or 2627) { MessageBox.Show("Duplicate record. Edit the existing record instead."); }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 547) { MessageBox.Show("Invalid reference or record is used by other data. Correct the reference, or deactivate the record."); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Future Path", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    private void Edit(DataRow? row)
    {
        var d = AdminDAL.Definition(table);
        using var form = new Form { Text = (row == null ? "Add " : "Edit ") + table, ClientSize = new(700, 650), StartPosition = FormStartPosition.CenterParent, Font = Theme.Body(10) };
        var fields = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new(20) };
        var inputs = new Dictionary<string, Control>();
        foreach (var f in d.Fields)
        {
            fields.Controls.Add(new Label { Text = f.Name + (f.Required ? " *" : " (optional)"), AutoSize = true, Margin = new(3, 12, 3, 4) });
            object? old = row?[f.Name]; if (old == DBNull.Value) old = null;
            Control c;
            if (f.Name == "UserRole")
            { var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; box.Items.AddRange([Roles.Learner, Roles.Advisor, Roles.Admin]); box.SelectedItem = old; c = box; }
            else if (f.Name == "Province")
            { var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; box.Items.Add(""); box.Items.AddRange(BusinessLogic.Validation.Provinces); box.SelectedItem = old ?? ""; c = box; }
            else if (f.Lookup != null)
            {
                var data = dal.Lookup(f.Lookup); if (!f.Required) { var blank = data.NewRow(); blank["ID"] = DBNull.Value; blank["Label"] = "(none)"; data.Rows.InsertAt(blank, 0); }
                var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DataSource = data, DisplayMember = "Label", ValueMember = "ID" };
                if (old != null) box.SelectedValue = old; else box.SelectedIndex = f.Required ? -1 : 0; c = box;
            }
            else if (f.Type == typeof(bool)) c = new CheckBox { Checked = old is bool active ? active : f.Name != "InActive", Text = "Yes" };
            else if (f.Type == typeof(decimal)) c = new NumericUpDown { Minimum = 0, Maximum = f.Name == "Weight" ? 99.99m : 100, DecimalPlaces = 2, Value = old is decimal n ? n : 0 };
            else if (f.Type == typeof(DateTime)) c = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", ShowCheckBox = true, Checked = old != null, Value = old is DateTime dt ? dt : DateTime.Today };
            else c = new TextBox { Text = Convert.ToString(old), MaxLength = f.MaxLength > 0 ? f.MaxLength : 2000, Multiline = f.MaxLength > 500, Height = f.MaxLength > 500 ? 70 : 28 };
            c.Width = 610; inputs[f.Name] = c; fields.Controls.Add(c);
        }
        TextBox? password = null;
        if (table == "UserAccount" && row == null)
        { fields.Controls.Add(new Label { Text = "Initial password (10–128 characters)", AutoSize = true }); password = new TextBox { UseSystemPasswordChar = true, MaxLength = 128, Width = 610 }; fields.Controls.Add(password); }
        var save = Theme.PrimaryButton("Save", Theme.TealDark, 200, 40);
        save.Click += (_, _) => Safe(() =>
        {
            var values = new Dictionary<string, object?>();
            foreach (var f in d.Fields)
            {
                var c = inputs[f.Name];
                object? v = c switch
                {
                    CheckBox check => check.Checked,
                    NumericUpDown number => number.Value,
                    DateTimePicker date => date.Checked ? date.Value.Date : null,
                    ComboBox box when f.Lookup != null => box.SelectedValue is int id ? id : null,
                    _ => c is ComboBox box ? box.Text : c.Text
                };
                values[f.Name] = v;
            }
            dal.Save(table, row == null ? null : (int)row[d.Key], values, password?.Text); form.DialogResult = DialogResult.OK;
        });
        fields.Controls.Add(save); form.Controls.Add(fields);
        if (form.ShowDialog(this) == DialogResult.OK) Reload();
    }
}
