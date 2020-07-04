using UnityEngine;
using UnityEditor;

namespace Chankiyu22.UnityCommander
{

[CustomPropertyDrawer(typeof(Command))]
public class CommandPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Command command = (Command) property.objectReferenceValue;
        if (command == null)
        {
            Rect propertyRect = new Rect(position.x, position.y, position.width - 70, position.height);
            Rect buttonRect = new Rect(position.xMax - 60, position.y, 60, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);
            if (GUI.Button(buttonRect, "New"))
            {
                string directory = Utils.GetActiveDirectory();
                string createdAssetPath = EditorUtility.SaveFilePanel("New Command", directory, "Command", "asset");
                if (createdAssetPath.Length != 0)
                {
                    createdAssetPath = createdAssetPath.Replace(Application.dataPath, "Assets");
                    Command asset = ScriptableObject.CreateInstance<Command>();
                    AssetDatabase.CreateAsset(asset, createdAssetPath);
                    AssetDatabase.SaveAssets();
                    property.objectReferenceValue = asset;
                }
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
        EditorGUI.EndProperty();
    }
}

}
