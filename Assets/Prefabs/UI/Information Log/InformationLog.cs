using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LUI;
using UnityEngine.InputSystem;
using System;

public class InformationLog : PlayerUI
{
    private PlayerStats _playerStats;

    [SerializeField]
    private GameObject _UI_Container;
    [SerializeField]
    private GameObject[] _informationImages;

    public override void Init(PlayerCharacter playerCharacter)
    {
        base.Init(playerCharacter);

        playerCharacter.controls.Gameplay.ToggleJournal.performed += JournalButtonPressed;
    }

    private void JournalButtonPressed(InputAction.CallbackContext obj)
    {
        _UI_Container.SetActive(!_UI_Container.activeSelf);
    }

    private void RevealInformation(int infoID)
    {
        _informationImages[infoID].SetActive(true);
    }

}
