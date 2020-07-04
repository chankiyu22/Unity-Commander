using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Chankiyu22.UnityCommander
{

public class Utils
{
    public static string GetDirectoryForSelectedProjectObject()
    {
        Object selectedObject = Selection.activeObject;
        if (selectedObject != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedObject.GetInstanceID());
            if (path.Length == 0)
            {
                return "";
            }
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                return Path.GetDirectoryName(path);
            }
        }

        return "";
    }

    public static string GetCurrentSceneDirectory()
    {
        Scene activeScene = EditorSceneManager.GetActiveScene();
        return Path.GetDirectoryName(activeScene.path);
    }

    public static string GetActiveDirectory()
    {
        string directory = GetDirectoryForSelectedProjectObject();
        if (directory.Length != 0)
        {
            return directory;
        }
        return GetCurrentSceneDirectory();
    }
}

}
