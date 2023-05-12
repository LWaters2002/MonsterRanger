using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "ScriptObjects/DialogueSequence", order = 0)]
public class DialogueSequence : ScriptableObject
{
    public bool skippable;
    public bool isCutsceneDialogue;
    public Dialogue[] dialogues;
}

[System.Serializable]
public struct Dialogue
{
    [Multiline]
    public string text;
    public float characterPrintTime;
}