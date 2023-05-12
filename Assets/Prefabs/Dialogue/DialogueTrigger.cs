using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LUI;

public class DialogueTrigger : MonoBehaviour
{

    public DialogueSequence dialogueSequence;
    public int revealNumber = -1;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        PlayerCharacter character = other.GetComponentInParent<PlayerCharacter>();

        if (character)
        {
            hasPlayed = true;
            character.dialogueBox.PlayDialogueSequence(dialogueSequence);
            character.informationLog.RevealInformation(revealNumber);
        }
    }

}
