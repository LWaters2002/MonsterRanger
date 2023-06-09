using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    PlayerCharacter player;

    public Slider sensitivitySlider;

    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

