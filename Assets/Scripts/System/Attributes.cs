using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsOnlyAttribute : PropertyAttribute { }
public class TypeNameAttribute : PropertyAttribute
{
    public Type BaseType;

    public TypeNameAttribute(Type BaseType)
    {
        this.BaseType = BaseType;
    }
}