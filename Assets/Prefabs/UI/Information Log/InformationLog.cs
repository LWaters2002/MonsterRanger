using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LUI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InformationLog : PlayerUI
{
    private PlayerStats _playerStats;


    private bool _firstReveal = true;

    [SerializeField]
    private GameObject _UI_Container;
    [SerializeField]
    private GameObject[] _informationImages;

    public DialogueSequence firstRevealDialogue;

    [Header("Events")]
    public UnityEvent OnInformationRevealed;
    public UnityEvent OnLogOpen;
    public UnityEvent OnLogClose;

    private bool _isOpen = false;

    public override void Init(PlayerCharacter playerCharacter)
    {
        base.Init(playerCharacter);

        playerCharacter.controls.Gameplay.ToggleJournal.performed += JournalButtonPressed;
    }

    private void JournalButtonPressed(InputAction.CallbackContext obj)
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            OnLogOpen?.Invoke();
        }
        else
        {
            OnLogClose?.Invoke();
        }
    }

    public void RevealInformation(int infoID)
    {

        if (infoID >= 0 && infoID < _informationImages.Length)
        {
            OnInformationRevealed?.Invoke();
            _informationImages[infoID].SetActive(true);
        }
    }

    private void CreateDialogue()
    {
        _firstReveal = false;
        GameManager.Get().PlayDialogue(firstRevealDialogue);
    }
}
