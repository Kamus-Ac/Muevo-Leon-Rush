using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IH_Rythm : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RythmActions _inputaction;
    public InputActionReference Center;
    public InputActionReference Right;
    public InputActionReference Left;

    public int ardCenter = 0;
    public int ardLeft = 0;

    public int ardRight = 0;

    public static int y = -5000;

    bool lastB1 = false;
    bool lastB2 = false;
    bool lastB3 = false;
    bool lastB4 = false;

    public static Action BeginAction;

    public static Action<int, float> buttonPress;

    void Update()
    {
        var serial = GameManager.Instance.serial;

        try
        {
            if (serial == null || !serial.IsOpen)
                return;

            if (serial.BytesToRead > 0)
            {
                string data = serial.ReadLine();

                if (data.Contains("Y:") && data.Contains("BTN:"))
                {
                    string[] partes = data.Split(',');

                    if (partes[0].StartsWith("Y:"))
                    {
                        string yStr = partes[0].Substring(2);
                        int.TryParse(yStr, out y);
                    }

                    string b1Str = partes[1].Substring(4);
                    bool b1 = b1Str == "1";

                    bool b2 = partes[2] == "1";
                    bool b3 = partes[3] == "1";
                    bool b4 = partes[4] == "1";

                    if (b1 && !lastB1)
                        buttonPress?.Invoke(1, Metronome_Memo.Instance.SongPosition);

                    if (b2 && !lastB2)
                        buttonPress?.Invoke(0, Metronome_Memo.Instance.SongPosition);

                    if (b4 && !lastB4)
                        buttonPress?.Invoke(2, Metronome_Memo.Instance.SongPosition);

                    if (b3 && !lastB3)
                        BeginAction?.Invoke();

                    lastB1 = b1;
                    lastB2 = b2;
                    lastB3 = b3;
                    lastB4 = b4;
                }
            }
        }
        catch (TimeoutException)
        {
            // 🔥 IMPORTANTE: esto es normal, ignóralo
        }
    }

    private void OnEnable()
    {

        _inputaction.RythmInput.Enable();
        
    }

    private void OnDisable()
    {
        _inputaction.RythmInput.Disable();
    }

    private void ActionCenter(InputAction.CallbackContext obj)
    {
        //Debug.Log("x");
        buttonPress?.Invoke(1,Metronome_Memo.Instance.SongPosition);
        
    }

    private void ActionRight(InputAction.CallbackContext obj)
    {
        buttonPress?.Invoke(2,Metronome_Memo.Instance.SongPosition);
    }

    private void ActionLeft(InputAction.CallbackContext obj)
    {
        buttonPress?.Invoke(0,Metronome_Memo.Instance.SongPosition);
    }


    void Awake()
    {

        _inputaction = new RythmActions();
        Center.action.started+=ActionCenter;
        Right.action.started+=ActionRight;
        Left.action.started+=ActionLeft;
        
    }

    private void OnDestroy()
    {

        Center.action.started -= ActionCenter;
        Right.action.started -= ActionRight;
        Left.action.started -= ActionLeft;
        _inputaction.Dispose();
    }
}
