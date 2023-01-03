using System;
using System.Linq;
using System.Reflection;
using Godot;

namespace LMTS.DependencyInjection;

public static class DIExtensions
{
    public static void ResolveDependencies(this Node node)
    {
        var at = typeof(InjectAttribute);
        var fields = node.GetType()
            .GetRuntimeFields()
            .Where(f => f.GetCustomAttributes(at, true).Any());

        foreach (var field in fields)
        {
            var obj = DependencyInjectionSystem.Instance.Resolve(field.FieldType);
            try
            {
                field.SetValue(node, obj);
            }
            catch (InvalidCastException)
            {
                GD.PrintErr($"Error converting value " +
                            $"{obj} ({obj.GetType()})" +
                            $" to {field.FieldType}");               
                throw;
            }
        }
    }
}