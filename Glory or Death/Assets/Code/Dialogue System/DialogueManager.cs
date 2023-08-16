using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
}

public class DialogueManager : MonoBehaviour
{
    public List<DialogueLine> dialogueLines;

    private void Start()
    {
        LoadDialogue("Assets/Code/Dialogue System/dialogue.json");
    }

    public void LoadDialogue(string jsonFilePath)
    {
        string json = System.IO.File.ReadAllText(jsonFilePath);
        Debug.Log(dialogueLines.Count);
    }
}
