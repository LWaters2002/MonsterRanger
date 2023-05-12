using System.Collections;
using UnityEngine;
using TMPro;
using LUI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueBox : PlayerUI
{
    [Header("References")]
    public TextMeshProUGUI dialogueText;
    public GameObject textBox;
    public AudioSource characterPrintSound;
    public Animator animator;

    [Header("Events")]
    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;

    private DialogueSequence _dialogueSequence;

    private int _dialogueIndex;
    private float _printMultiplier = 1f;


    public override void Init(PlayerCharacter playerCharacter)
    {
        base.Init(playerCharacter);

        GameManager.Get().OnCutsceneChange += CutsceneState;

        Controls controls = playerCharacter.controls;
        controls.Gameplay.SkipDialogue.performed += SkipDialogue;
        controls.Gameplay.EndDialogue.performed += EndDialogue;
    }

    private void EndDialogue(InputAction.CallbackContext context)
    {
        if (!_dialogueSequence) return;

        FinishDialogueSequence();
    }

    private void SkipDialogue(InputAction.CallbackContext context)
    {
        if (!_dialogueSequence) return;
        _printMultiplier = .1f;
    }

    private void CutsceneState(bool cutsceneStart)
    {
        if (!_dialogueSequence) return;

        if (cutsceneStart && !_dialogueSequence.isCutsceneDialogue)
        {
            dialogueText.text = "";
            textBox.SetActive(false);
            return;
        }

        if (!cutsceneStart)
        {
            textBox.SetActive(true);
        }

    }

    public override void RemoveUI()
    {
        base.RemoveUI();
    }

    public void PlayDialogueSequence(DialogueSequence dialogueSequence)
    {
        animator.speed = 1;
        _dialogueSequence = dialogueSequence;
        _dialogueIndex = 0;
        dialogueText.maxVisibleCharacters = 0;

        OnDialogueStart?.Invoke();
        StartCoroutine(PrintDialogue());
    }

    private void NextDialogue()
    {
        _dialogueIndex++;

        if (_dialogueIndex >= _dialogueSequence.dialogues.Length) { FinishDialogueSequence(); return; }

        dialogueText.maxVisibleCharacters = 0;
        StartCoroutine(PrintDialogue());

    }

    IEnumerator PrintDialogue()
    {
        Dialogue currentDialogue = _dialogueSequence.dialogues[_dialogueIndex];
        dialogueText.text = currentDialogue.text;
        _printMultiplier = 1;

        for (int i = 0; i < currentDialogue.text.Length; i++)
        {
            dialogueText.maxVisibleCharacters = i + 1;
            characterPrintSound.pitch = Random.Range(.8f, 1.2f);
            characterPrintSound?.Play();
            float printTime = currentDialogue.characterPrintTime;

            animator.speed = (1 / (printTime * _printMultiplier)) / 20;

            yield return new WaitForSeconds(printTime * _printMultiplier);
        }

        animator.speed = 0;

        yield return new WaitForSeconds(1f);

        NextDialogue();
    }

    public void FinishDialogueSequence()
    {
        animator.speed = 1;
        _dialogueSequence = null;
        StopAllCoroutines();
        OnDialogueEnd?.Invoke();
    }


}
