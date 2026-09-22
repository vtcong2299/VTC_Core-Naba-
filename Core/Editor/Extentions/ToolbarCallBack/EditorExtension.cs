using System.Linq;
using Vtcong.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]

public class SceneSwitchLeftButton
{
    static SceneSwitchLeftButton()
    {
        // EditorApplication.update += Update;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.EnteredEditMode && !EditorParameters.Instance.curEditScene.IsNullOrEmpty())
        {
            if (EditorParameters.Instance.curEditScene != SceneManager.GetActiveScene().path)
            {
                EditorSceneManager.OpenScene(EditorParameters.Instance.curEditScene);
                EditorUtility.SetDirty(EditorParameters.Instance);
                EditorParameters.Instance.curEditScene = string.Empty;
            }
        }
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("open scenes", "Start From LoadScene")))
        {
            SceneEditor.OpenWindown();
        }
        
        // GUILayout.BeginVertical();
        // foreach (var scene in EditorBuildSettings.scenes)
        // {
        //     GUILayout.FlexibleSpace();
        //     if (scene.enabled)
        //     {
        //         string[] strings = scene.path.Split('/');
        //         string _name = strings.LastOrDefault().Replace(".unity", "");
        //         if (GUILayout.Button(new GUIContent(_name, "Start From LoadScene")))
        //         {
        //             EditorSceneManager.OpenScene(scene.path);
        //         }
        //     }
        // }
        // GUILayout.EndVertical();
        // if (GUILayout.Button(new GUIContent("Startup", "Start From LoadScene")))
        // {
        //     EditorParameters.Instance.curEditScene = SceneManager.GetActiveScene().path;
        //     EditorUtility.SetDirty(EditorParameters.Instance);
        //     StartScene("home");
        //     //StartScene(EditorParameters.Instance.startScene);
        // }
    }

    // private static bool isPlaying;
    //
    // static void StartScene(string sceneName)
    // {
    //     if (EditorApplication.isPlaying) return;
    //     if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //     {
    //         string[] guids = AssetDatabase.FindAssets("t:scene " + sceneName, null);
    //         if (guids.Length == 0)
    //         {
    //             Debug.LogWarning("Couldn't find scene file");
    //         }
    //         else
    //         {
    //             string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
    //             EditorSceneManager.OpenScene(scenePath);
    //             EditorApplication.isPlaying = true;
    //         }
    //     }
    // }
}