namespace FuturePathStudentAdvisingSystem.Models;

public record ManagedField(string Name, Type Type, bool Required = true, int MaxLength = 0, string? Lookup = null);
public record ManagedTable(string Name, string Key, params ManagedField[] Fields);

