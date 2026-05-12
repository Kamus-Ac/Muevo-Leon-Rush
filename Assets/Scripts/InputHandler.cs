using System;
using System.Security.Cryptography;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    private float _turnInput;
    public float TurnInput => _turnInput;

    private RythmActions _input;
    public static InputHandler Instance;

    public event EventHandler OnSwitchAction;



    private void Awake()
    {
        Instance = this;

        _input = new RythmActions();

        _input.Drive.Turn.performed += Turn;
        _input.Drive.Turn.canceled += TurnCanceled;

        _input.Drive.Switch.performed += Switch_performed; //tengo que suscribirme porque es un evento externo 


    }

    private void OnEnable()
    {
        _input.Drive.Enable();
    }

    private void Switch_performed(InputAction.CallbackContext obj)
    {
        OnSwitchAction?.Invoke(this, EventArgs.Empty);   
    }


    private void Turn(InputAction.CallbackContext context)
    {
        _turnInput = context.ReadValue<float>();
    }

    private void TurnCanceled(InputAction.CallbackContext context)
    {
        _turnInput = 0;
    }

    private void OnDisable()
    {
        _input.Drive.Disable();
    }

    private void TurnArduino()
    {
        if (IH_Rythm.y < -3000 )
        {
            _turnInput=-1;
        }
        else if(IH_Rythm.y > 3000)
        {
            _turnInput=1;
        }

        else
        {
            _turnInput=0;
        }
    }

    private void OnDestroy()
    {
        _input.Drive.Turn.performed -= Turn;
        _input.Drive.Turn.canceled -= TurnCanceled;
        _input.Drive.Switch.performed -= Switch_performed;
        _input.Dispose();
    }

    private void Update()
    {
        if (GameManager.Instance.puertoExiste)
        {
            TurnArduino();
            //print(IH_Rythm.y);
        }
    }
}
