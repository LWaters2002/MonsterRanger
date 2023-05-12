using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World Settings")]
    public PlayerController playerController;

    private PlayerCharacter playerCharacter;

    public LUI.UIHolder UIHolderPrefab;

    public static GameManager Get() { return Instance; }
    protected static GameManager Instance;

    public System.Action<bool> OnCutsceneChange;

    private void Awake()
    {
        Instance = this;

        LUI.UIHolder uiHolder = Instantiate(UIHolderPrefab);
        uiHolder.Init();

        playerController = Instantiate(playerController, Vector3.zero, Quaternion.identity);
        playerController.Init();

        playerCharacter = FindObjectOfType<PlayerCharacter>();
    }

    public void PlayDialogue(DialogueSequence sequence)
    {
        if (!playerCharacter) return;

        playerCharacter.dialogueBox.PlayDialogueSequence(sequence);
    }
}
