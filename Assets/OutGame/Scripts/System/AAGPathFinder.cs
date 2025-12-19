using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AAGPathFinder
{
    public static string GetAAGPath<T>(string keyWord)
    {
        FieldInfo[] fields = typeof(T).GetFields();
        FieldInfo field = fields.FirstOrDefault(f => f.Name.Contains(keyWord));
        return field.GetRawConstantValue().ToString();
    }

    public static string GetAAGPathWithID<T>(uint id)
    {
        string keyWord = "ID_" + id;
        return GetAAGPath<T>(keyWord);
    }

    public static string GetAAGPathWithEnum<T>(T enumType)
    {
        string keyWord = enumType.ToString();
        return GetAAGPath<T>(keyWord);
    }
}
