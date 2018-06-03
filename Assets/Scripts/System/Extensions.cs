using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HRExtensions
{
    public static IEnumerable<Type> FindSubClasses(this Type type)
    {
        var baseType = type;
        var assembly = baseType.Assembly;

        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
    }

    public static List<string> ToStringList(this IEnumerable<Type> types)
    {
        var list = new List<string>();
        foreach(var item in types)
        {
            list.Add(item.ToString());
        }
        return list;
    }
}