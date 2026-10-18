using FuturePathStudentAdvisingSystem.Models;
namespace FuturePathStudentAdvisingSystem.BusinessLogic;

public static class ReferenceDataValidation
{
    public static List<(string, object?)> Validate(ManagedTable d, IReadOnlyDictionary<string, object?> values)
    {
        var args = new List<(string, object?)>();
        foreach (var f in d.Fields)
        {
            if (!values.TryGetValue(f.Name, out var v)) throw new ArgumentException("Missing field: " + f.Name);
            if (v == DBNull.Value) v = null;
            if (f.Type == typeof(string)) v = Convert.ToString(v)?.Trim();
            if (f.Required && (v == null || v is string s && s.Length == 0)) throw new ArgumentException(f.Name + " is required.");
            if (v is string str && f.MaxLength > 0 && str.Length > f.MaxLength) throw new ArgumentException(f.Name + " is too long.");
            if (v != null && !f.Type.IsInstanceOfType(v)) throw new ArgumentException(f.Name + " has an invalid type.");
            if (v is decimal n && (n < 0 || (f.Name == "MinimumMark" && n > 100) || (f.Name == "Weight" && (n <= 0 || n > 99.99m)))) throw new ArgumentException("Invalid " + f.Name);
            args.Add(("@" + f.Name, v is string empty && empty.Length == 0 && !f.Required ? null : v));
        }
        return args;
    }
}
