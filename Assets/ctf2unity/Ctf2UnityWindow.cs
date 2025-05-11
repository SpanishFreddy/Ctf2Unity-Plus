#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ctf2Unity
{
    public class Ctf2UnityWindow : EditorWindow
    {
        [MenuItem("Tools/Ctf2Unity")]
        private static void Init()
        {
            var win = GetWindow<Ctf2UnityWindow>();
            win.titleContent = new GUIContent("Ctf2Unity");
            win.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Start"))
                OnStart();
        }

        private void OnStart()
        {
            var ctfPath = EditorUtility.OpenFilePanel("Open CTF app", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "exe");
            if (string.IsNullOrEmpty(ctfPath))
                return;
            var proc = new Ctf2UnityProcessor(ctfPath);
            proc.Start();
        }
    }
}
#endif