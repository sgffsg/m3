using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DEBUGGER
{
    public static string DebugStr;
    public static string ToHexColor(this Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
        //string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    public static void Log(object log)
    {
        Debug.Log(string.Format("<color=#FFFFFF>{0}</color>", log));
    }

    public static void Log(Color color, object log)
    {
        string col = "<color=#" + color.ToHexColor() + ">{0}</color>";
        Debug.Log(string.Format(col, log));
    }

    public static void Log(string hexColor, object log)
    {
        string col = "<color=#" + hexColor + ">{0}</color>";
        Debug.Log(string.Format(col, log));
    }

    public static void Log(ColorType type, object log)
    {
        string hexColor = "FFFFFF";
        switch (type)
        {
            case ColorType.Warning:                 //Warnings
                hexColor = "f80000";
                break;
            case ColorType.Attention:               //Attention
                hexColor = "FFE900";
                break;
            case ColorType.Action:                  //Action
                hexColor = "00EAFF";
                break;
            case ColorType.Result:         
                hexColor = "07FF00";
                break;
            case ColorType.System:              //Application messages
                hexColor = "7bd7ff";
                break;
            case ColorType.Other:                //Other
                hexColor = "FF00D2";
                break;
            case ColorType.Change:              //Other
                hexColor = "ffa000";
                break;
            default:
                break;
        }
        string col = "<color=#" + hexColor + ">{0}</color>";
        Debug.Log(string.Format(col, log));
    }

    public static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }
}
public enum ColorType
{
    Warning,
    Attention,
    Action,
    Result,
    System,
    Other,
    Change
}
