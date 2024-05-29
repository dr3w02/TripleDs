using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePanel;
    CustomInputs input;
    bool isPausePressed;

    void Awake()
    {
        input = new CustomInputs();

        input.PauseMenu.MenuOpenClose.started += onPause;
        input.PauseMenu.MenuOpenClose.performed += onPause;
        input.PauseMenu.MenuOpenClose.canceled += onPause;
    }
    void onPause(InputAction.CallbackContext context)
    {
        isPausePressed = context.ReadValueAsButton();
        Debug.Log("JumpPressed");
    }
    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
