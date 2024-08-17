using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{

    [Header("Menu Objects")]
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _settingsMenuCanvas;
    [SerializeField] private GameObject _creidtsCanvas;
    [SerializeField] private GameObject _gamePadCanvas;
    [SerializeField] private GameObject _keyboardCanvas;
    [SerializeField] private GameObject _UnstuckCanvas;

    [Header("Player Scripts to deactivate on Pause")]

    [SerializeField] private characterMovement _player;
    //[SerializeField] private CharacterController _player;

    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _creditsMenuFirst;
    [SerializeField] private GameObject _gamePadMenuFirst;
    [SerializeField] private GameObject _keyboardMenuFirst;

    private bool isPaused;

    public characterMovement characterMovement;

    private void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }


    public void Pause()
    {
        isPaused = true;

        Time.timeScale = 0f;

        _player.enabled = false;

        Cursor.lockState = CursorLockMode.Confined;
        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;

        Time.timeScale = 1f;

        _player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        CloseAllMenus();
    }

    private void OpenSettingsMenuHandle()
    {
        _settingsMenuCanvas.SetActive(true);
        _mainMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
    }

    //Canvas Activations / Deactivations
    private void OpenMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);
        _creidtsCanvas.SetActive(false);
        _gamePadCanvas.SetActive(false);
        _keyboardCanvas.SetActive(false);
        _UnstuckCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }


    private void CloseAllMenus()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _creidtsCanvas.SetActive(false);
        _gamePadCanvas.SetActive(false);
        _keyboardCanvas.SetActive(false);
        _UnstuckCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }


    public void OnSettingsPress()
    {
        OpenSettingsMenuHandle();
    }


    public void OnResumePressed()
    {
        Unpause();
    }

    public void OnCreditsPress()
    {
        OpenCreditsMenuHandle();

    }


    public void OnUnStuckPress()
    {
        characterMovement.RespawnPlayer();
    }

    public void OnGamePadPress()
    {
        OpenGamePadHandle();
    }

    public void OnKeyboardPress()
    {
        OpenKeyboardHandle();
    }


    private void OpenCreditsMenuHandle()
    {
        _settingsMenuCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _creidtsCanvas.SetActive(true);
        _gamePadCanvas.SetActive(false);
        _keyboardCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_creditsMenuFirst);
    }

    private void OpenGamePadHandle()
    {
        _settingsMenuCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _creidtsCanvas.SetActive(false);
        _gamePadCanvas.SetActive(true);
        _keyboardCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_gamePadMenuFirst);
    }

    private void OpenKeyboardHandle()
    {
        _settingsMenuCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _creidtsCanvas.SetActive(false);
        _gamePadCanvas.SetActive(false);
        _keyboardCanvas.SetActive(true);


        EventSystem.current.SetSelectedGameObject(_keyboardMenuFirst);
    }

    //////BackButton//////////////////

    public void OnSettingsBackPressed()
    {
        OpenMainMenu();
    }



}

