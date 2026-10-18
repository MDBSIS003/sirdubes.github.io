using System.Data;
using FuturePath.Business;
using Microsoft.Data.SqlClient;

namespace FuturePath.Desktop;
internal static class Ui
{
    public static readonly Color Navy = Color.FromArgb(21, 40, 60);
    public static readonly Color Teal = Color.FromArgb(16, 116, 99);
    public static readonly Color Background = Color.FromArgb(243, 247, 249);
    public static readonly Color Muted = Color.FromArgb(80, 99, 115);
    public static Font Body(float size = 10, FontStyle style = FontStyle.Regular) => new("Segoe UI", size, style);
    public static Label Label(string text, float size = 10, FontStyle style = FontStyle.Regular) => new()
    {
        Text = text,
        AutoSize = true,
        Font = Body(size, style),
        ForeColor = Navy,
        Margin = new Padding(0, 0, 0, 10),
        MaximumSize = new Size(950, 0)
    };
    public static Button Button(string text, Action action, bool primary = false)
    {
        var b = new Button
        {
            Text = text,
            AutoSize = true,
            MinimumSize = new Size(110, 40),
            Padding = new Padding(14, 5, 14, 5),
            BackColor = primary ? Teal : Color.White,
            ForeColor = primary ? Color.White : Navy,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = Body(10, FontStyle.Bold),
            Margin = new Padding(0, 0, 10, 8)
        };
        b.FlatAppearance.BorderColor = primary ? Teal : Color.FromArgb(204, 215, 221);
        b.Click += (_, _) => Run(action);
        return b;
    }

    public static void Run(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    public static void ShowError(Exception ex)
    {
        var message = ex switch
        {
            ValidationException or AccessDeniedException => ex.Message,
            SqlException => "SQL Server LocalDB could not complete this action. Check that MSSQLLocalDB is running and the database scripts have been applied. Existing completed sessions are retained.",
            IOException or UnauthorizedAccessException => "The file could not be read or saved. Choose a writable location and check available disk space.\n" + ex.Message,
            _ => "The action could not be completed. Please retry, or restart Future Path.\n" + ex.Message
        };
        MessageBox.Show(message, "Future Path — action needed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static FlowLayoutPanel Stack() => new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        Padding = new Padding(24),
        BackColor = Background
    };
    public static FlowLayoutPanel Row(params Control[] controls)
    {
        var row = new FlowLayoutPanel
        {
            AutoSize = true,
            WrapContents = true,
            Margin = new Padding(0, 8, 0, 8),
            MaximumSize = new Size(1100, 0)
        };
        row.Controls.AddRange(controls);
        return row;
    }

    public static Panel Card(string title, string description, int width = 820)
    {
        var p = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            Width = width,
            Padding = new Padding(22),
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 18),
            MinimumSize = new Size(width, 0),
            MaximumSize = new Size(width, 0)
        };
        p.ControlAdded += (_, e) =>
        {
            if (e.Control is Label label)
                label.MaximumSize = new Size(width - 44, 0);
        };
        p.Controls.Add(Label(title, 16, FontStyle.Bold));
        p.Controls.Add(Label(description));
        return p;
    }

    public static ComboBox Combo(IEnumerable<LookupItem> items, int? selected = null, int width = 280)
    {
        var c = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = width,
            Font = Body(),
            DisplayMember = "Name",
            ValueMember = "Id",
            BindingContext = new BindingContext()
        };
        c.DataSource = items.ToList();
        if (selected.HasValue)
            c.SelectedValue = selected.Value;
        return c;
    }

    public static int SelectedId(ComboBox c) => c.SelectedItem is LookupItem item ? item.Id : 0;
    public static ComboBox Choice(IEnumerable<string> values, int width = 280)
    {
        var c = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = width,
            Font = Body()
        };
        c.Items.AddRange(values.Cast<object>().ToArray());
        if (c.Items.Count > 0)
            c.SelectedIndex = 0;
        return c;
    }

    public static TextBox Input(int width = 300) => new()
    {
        Width = width,
        Font = Body(),
        MaxLength = 150
    };
    public static Control Field(string label, Control input)
    {
        input.AccessibleName = label;
        input.Margin = new Padding(0, 0, 0, 12);
        var p = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Margin = new Padding(0, 0, 20, 8)
        };
        p.Controls.Add(Label(label, 9, FontStyle.Bold));
        p.Controls.Add(input);
        return p;
    }

    public static DataGridView Grid() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        BindingContext = new BindingContext(),
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoGenerateColumns = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White,
        BorderStyle = BorderStyle.None,
        RowHeadersVisible = false,
        Font = Body(10),
        ColumnHeadersHeight = 42,
        EnableHeadersVisualStyles = false,
        ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Navy,
            ForeColor = Color.White,
            Font = Body(10, FontStyle.Bold)
        },
        DefaultCellStyle = new DataGridViewCellStyle
        {
            SelectionBackColor = Color.FromArgb(216, 239, 233),
            SelectionForeColor = Navy,
            Padding = new Padding(4)
        },
        AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(247, 250, 251)
        },
        RowTemplate =
        {
            Height = 36
        }
    };
    public static DataTable Table(IEnumerable<ReferenceRow> rows)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        var list = rows.ToList();
        if (list.Count > 0)
            foreach (var key in list[0].Values.Keys)
                table.Columns.Add(key, typeof(string));
        foreach (var row in list)
            table.Rows.Add(new object[] { row.Id }.Concat(row.Values.Values.Select(v => (object)(v is bool flag ? (flag ? "1" : "0") : Convert.ToString(v) ?? ""))).ToArray());
        return table;
    }

    public static bool Confirm(string message) => MessageBox.Show(message, "Future Path", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
}

internal class AppForm : Form
{
    public AppForm()
    {
        Font = Ui.Body();
        ForeColor = Ui.Navy;
        BackColor = Ui.Background;
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(640, 480);
    }
}
