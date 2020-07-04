using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.UnityCommander
{

[CustomEditor(typeof(CommandDispatcher))]
public class CommandEditor : Editor
{
    private SerializedProperty commandActionsProp;
    private ReorderableList commandActionsReorderableList;

    private bool showCommandDispatcherOverview = true;
    private bool showCommandDispatcherEditor = false;

    void OnEnable()
    {
        commandActionsProp = serializedObject.FindProperty("m_commandActions");
        commandActionsReorderableList = new ReorderableList(serializedObject, commandActionsProp, true, true, true, true);
        commandActionsReorderableList.elementHeightCallback = GetCommandActionElementHeight;
        commandActionsReorderableList.drawElementCallback = DrawCommandActionElement;
        commandActionsReorderableList.drawHeaderCallback = DrawCommandActionsHeader;
        commandActionsReorderableList.onAddCallback = AddElement;
        commandActionsReorderableList.onRemoveCallback = RemoveElement;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        DrawCommandDispatcherOverview();
        EditorGUILayout.Space();
        DrawCommandDispatcherEditor();
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawCommandDispatcherOverview()
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
        foldoutStyle.fontStyle = FontStyle.Bold;
        showCommandDispatcherOverview = EditorGUILayout.Foldout(showCommandDispatcherOverview, "Overview", foldoutStyle);
        if (showCommandDispatcherOverview)
        {
            for (int i = 0; i < commandActionsProp.arraySize; i++)
            {
                SerializedProperty commandActionProp = commandActionsProp.GetArrayElementAtIndex(i);
                SerializedProperty commandProp = commandActionProp.FindPropertyRelative("m_command");
                Command command = (Command) commandProp.objectReferenceValue;
                SerializedProperty actionsCountProp = commandActionProp.FindPropertyRelative("m_actions.m_PersistentCalls.m_Calls");
                int actionsCount = actionsCountProp.arraySize;
                if (command != null)
                {
                    Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width - 90, rect.height), commandProp, GUIContent.none);
                    EditorGUI.EndDisabledGroup();
                    bool rightClicked = GUI.Button(new Rect(rect.xMax - 110, rect.y, 110, rect.height), actionsCount + " action" + (actionsCount == 0 || actionsCount > 1 ? "s" : ""), EditorStyles.miniButtonRight);
                    if (rightClicked)
                    {
                        showCommandDispatcherEditor = true;
                        commandActionsReorderableList.index = FindCommandActionsPropIndexByCommand(command);
                    }
                }
            }
        }
    }

    void DrawCommandDispatcherEditor()
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
        foldoutStyle.fontStyle = FontStyle.Bold;
        showCommandDispatcherEditor = EditorGUILayout.Foldout(showCommandDispatcherEditor, "Editor", foldoutStyle);
        if (showCommandDispatcherEditor)
        {
            commandActionsReorderableList.DoLayoutList();
        }
    }

    void DrawCommandActionsHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Commands");
    }

    void DrawCommandActionElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        if (index >= commandActionsProp.arraySize)
        {
            return;
        }

        SerializedProperty element = commandActionsProp.GetArrayElementAtIndex(index);
        SerializedProperty commandProp = element.FindPropertyRelative("m_command");
        SerializedProperty actionsProp = element.FindPropertyRelative("m_actions");

        rect.y += 2;

        float commandPropHeight = EditorGUI.GetPropertyHeight(commandProp);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, commandPropHeight), commandProp, GUIContent.none);
        rect.y += commandPropHeight;

        rect.y += 2;

        float actionsPropHeight = EditorGUI.GetPropertyHeight(actionsProp);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, actionsPropHeight), actionsProp);
    }

    float GetCommandActionElementHeight(int index)
    {
        if (index >= commandActionsProp.arraySize)
        {
            return 0;
        }

        SerializedProperty element = commandActionsProp.GetArrayElementAtIndex(index);
        SerializedProperty commandProp = element.FindPropertyRelative("m_command");
        float commandPropHeight = EditorGUI.GetPropertyHeight(commandProp);
        SerializedProperty actionsProp = element.FindPropertyRelative("m_actions");
        float actionsPropHeight = EditorGUI.GetPropertyHeight(actionsProp);

        float height = commandPropHeight + actionsPropHeight + 10;

        return height;
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_command").objectReferenceValue = null;
        SerializedProperty actionCallsProp = element.FindPropertyRelative("m_actions.m_PersistentCalls.m_Calls");
        actionCallsProp.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }

    void RemoveElement(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Delete Command Actions", "Are you sure you want to delete?", "Yes", "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        }
    }

    int FindCommandActionsPropIndexByCommand(Command command)
    {
        for (int i = 0; i < commandActionsProp.arraySize; i++)
        {
            SerializedProperty commandActionProp = commandActionsProp.GetArrayElementAtIndex(i);
            if (command == (Command) commandActionProp.FindPropertyRelative("m_command").objectReferenceValue)
            {
                return i;
            }
        }
        return -1;
    }
}

}
