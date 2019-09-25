using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SetupScene : EditorWindow
{
    public List<GameObject> objectsToDesactivate;
    public List<GameObject> objectsToActivate;
    Vector2 scrollPos;
    [MenuItem("Window/Setup Scene")]
    public static void ShowWindow()
    {       
        GetWindow<SetupScene>("Setup Scene");        
    }

    private void OnGUI()
    {       
        GUILayout.Label("This editor window has been created to quickly setup the scene so \n" +
                        "it is ready to play or build and everything is activated/desactivated properly", EditorStyles.boldLabel);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("GameObjects to Desactivate before playing", EditorStyles.label);
        objectsToDesactivate = FindObjectOfType<EditorHelper>().GetDesactivationList();
        objectsToActivate = FindObjectOfType<EditorHelper>().GetActivationList();

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty desactivationListProperty = so.FindProperty("objectsToDesactivate");
        EditorGUILayout.PropertyField(desactivationListProperty, true);

        GUILayout.Label("\n GameObjects to Activate before playing", EditorStyles.label);
        objectsToActivate = FindObjectOfType<EditorHelper>().GetActivationList();        
        SerializedObject activationSO = new SerializedObject(target);
        SerializedProperty activationListProperty = activationSO.FindProperty("objectsToActivate");
        EditorGUILayout.PropertyField(activationListProperty, true);
        so.ApplyModifiedProperties();
        activationSO.ApplyModifiedProperties();

        if (GUILayout.Button("Setup Scene"))
        {
            foreach (var gameObject in objectsToDesactivate)
            {
                gameObject.SetActive(false);
            }
            foreach (var gameObject in objectsToActivate)
            {
                gameObject.SetActive(true);
            }
        }
        GUILayout.EndScrollView();
    }
}
