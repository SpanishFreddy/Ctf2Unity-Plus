using Ctf2Unity;
using Ctf2Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AppInfo : ScriptableObject
{
    public static AppInfo current;

    [HideInInspector] public AssetReferences assets;

    public Vector2Int resolution; // base game resolution
    public bool fullScreenStart;
    public bool vsyncStart;

    public string company;
    public string productName;
    public string version;
    public int maxFps;

    public AppInfo()
    {
        current = this;
    }

    private void OnEnable()
    {
        Application.targetFrameRate = maxFps;
    }

    public Vector2Int CalculateNewResolution()
    {
        var res = Screen.currentResolution;
        var scaleFactor = resolution.y > resolution.x ? res.height / resolution.y : res.width / resolution.x;

        return resolution * scaleFactor;
    }

#if UNITY_EDITOR
    public static void Create(Ctf2UnityProcessor processor)
    {
        var app = processor.Header;

        var dir = "Data";
        Directory.CreateDirectory(Path.Combine(Application.dataPath, dir));

        var path = @$"Assets\{dir}\AppInfo.asset";
        var src = CreateInstance<AppInfo>();

        src.resolution = new Vector2Int(app.WindowWidth, app.WindowHeight);
        src.fullScreenStart = app.Flags["FullscreenAtStart"];
        src.vsyncStart = app.NewFlags["VSync"];
        if (processor.Copyright != null)
            src.company = processor.Copyright.Value;
        src.productName = processor.Name.Value;
        src.version = processor.version;
        src.maxFps = app.FrameRate;

        src.ApplyAppSettings();

        src.assets.Update();
        AssetDatabase.CreateAsset(src, path);
    }

    public void ApplyAppSettings()
    {
        PlayerSettings.fullScreenMode = fullScreenStart ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        QualitySettings.vSyncCount = vsyncStart ? 1 : 0;

        var newRes = CalculateNewResolution();
        PlayerSettings.defaultScreenWidth = newRes.x;
        PlayerSettings.defaultScreenHeight = newRes.y;
        PlayerSettings.defaultIsNativeResolution = false;
        PlayerSettings.companyName = company;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;
    }
#endif
}