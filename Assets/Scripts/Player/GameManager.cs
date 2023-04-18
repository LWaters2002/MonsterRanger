using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World Settings")]
    public PlayerController playerController;

    public LUI.UIHolder UIHolderPrefab;

    private void Awake()
    {
        LUI.UIHolder uiHolder = Instantiate(UIHolderPrefab);
        uiHolder.Init();

        playerController = Instantiate(playerController, Vector3.zero, Quaternion.identity);
        playerController.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
