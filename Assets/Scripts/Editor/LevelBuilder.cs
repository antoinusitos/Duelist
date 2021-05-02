using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class LevelsBuilder : EditorWindow
{
    List<string> myScenesPath = new List<string>();

    private bool myCleanCache = false;
    private bool myRunAfterBuild = false;

    private string myGameName = "Duelist";

    // Add menu item named "Example Window" to the Window menu
    [MenuItem("Duelist/Build Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(LevelsBuilder));
    }

    void OnGUI()
    {
        myCleanCache = GUILayout.Toggle(myCleanCache, "Clean cache");

        GUILayout.Space(8);

        myRunAfterBuild = GUILayout.Toggle(myRunAfterBuild, "Run After Build");

        GUILayout.Space(8);

        if (GUILayout.Button("Build"))
        {
            Build();
        }

        if (GUILayout.Button("Launch"))
        {
            Launch();
        }
    }

    public void Build()
    {
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        string path = Application.dataPath;
        string[] members = path.Split('/');
        path = "";
        for (int i = 0; i < members.Length - 1; i++)
        {
            path += members[i] + "/";
        }
        path += "Build";

        if (myCleanCache)
        {
            Directory.Delete(path, true);
        }

        // Build player.
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/" + myGameName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        // Copy a file from the project folder to the build folder, alongside the built game.
        FileUtil.CopyFileOrDirectory("Assets/Resources/Deck", path + "/" + myGameName + "_Data/Resources/Deck");


        UnityEngine.Debug.Log("Build Complete");


        if (myRunAfterBuild)
        {
            // Run the game (Process class from System.Diagnostics).
            Process proc = new Process();
            proc.StartInfo.FileName = path + "/" + myGameName + ".exe";
            proc.Start();
        }
    }

    public void Launch()
    {
        string path = Application.dataPath;
        string[] members = path.Split('/');
        path = "";
        for (int i = 0; i < members.Length - 1; i++)
        {
            path += members[i] + "/";
        }
        path += "Build";
        Process proc = new Process();
        proc.StartInfo.FileName = path + "/" + myGameName + ".exe";
        proc.Start();
    }
}