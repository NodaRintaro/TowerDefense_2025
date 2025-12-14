using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using CharacterData;

#if UNITY_EDITOR
public class SaveDataDebugger
{
    [MenuItem("Tools/SaveDataDebugger")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SaveDataDebugger));
    }

    void OnGUI()
    {

    }
}
#endif