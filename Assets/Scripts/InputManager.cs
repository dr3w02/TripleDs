using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    public bool MenuOpenCloseInput {  get; private set; }

    private PlayerInput _playerInput;


    private InputAction _menuOpenCloseAction;

    //Music Box
    public bool charging;

 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];

       
    }


    // Update is called once per frame
    public void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();

    }


   public bool GetHold()
    {
        return charging;
    }
 
}
