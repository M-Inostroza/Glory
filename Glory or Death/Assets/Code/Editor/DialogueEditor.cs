using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueManager))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DialogueManager DM = (DialogueManager)target;
        GUILayout.Label("Dialogues:");
        
        if (GUILayout.Button("Next Interaction"))
        {
            DM.StartCoroutine(DM.Interactions(2, 0));
        }
    }
}
