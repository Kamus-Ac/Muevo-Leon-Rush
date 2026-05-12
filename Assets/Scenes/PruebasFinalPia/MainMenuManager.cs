using TMPro;
using System;
using System.Collections;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI Start_text;
    float time;
    float duration = 3f;
    public SerialPort serial = new SerialPort("COM9", 9600);

    public bool puertoExiste = false;

    #region puerto
    private RythmActions _inputaction;

    public static int y = -5000;

    bool lastB1 = false;
    bool lastB2 = false;
    bool lastB3 = false;
    bool lastB4 = false;

    public static Action BeginAction;

    public static Action<int, float> buttonPress;

    #endregion

    void Start()
    {
        TryOpenSerial();
    }

    void Awake()
    {
        _inputaction = new RythmActions();
    }

    void OnEnable()
    {
        _inputaction.Drive.Switch.performed += Switch_perfomed;
        _inputaction.Drive.Enable();
        BeginAction += StartGame;
    }

    void OnDestroy()
    {
        BeginAction -= StartGame;

        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }

            serial.Dispose();
        }

        _inputaction.Dispose();
    }

    void OnDisable()
    {
        _inputaction.Drive.Switch.performed -= Switch_perfomed;
        _inputaction.Drive.Disable();
        BeginAction -= StartGame;
    }

    void Switch_perfomed(InputAction.CallbackContext context)
    {
        Loader.Load(Loader.Scene.FinalGame1);
    }

    void StartGame()
    {
        Loader.Load(Loader.Scene.FinalGame1);
    }

    // Update is called once per frame
    void Update()
    {

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

    void FixedUpdate()
    {
        FadeStart();
    }

    void FadeStart()
    {
        time+=Time.fixedDeltaTime;
        if (time < duration)
        {
            Start_text.alpha=Mathf.Lerp(1.0f,0, time/duration);
        }
        else
        {
            time=0;
        }

    }

    void OnApplicationQuit()
    {
        if (puertoExiste)
        {
            serial.Close();
        }
        
    }
    void TryOpenSerial()
    {
        string[] ports = SerialPort.GetPortNames();

        Debug.Log("Puertos disponibles: " + string.Join(", ", ports));

        if (ports.Contains("COM9")) // 👈 tu puerto
        {
            try
            {
                serial.ReadTimeout = 100;
                serial.Open();

                Debug.Log("Arduino conectado en COM");
                puertoExiste=true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("No se pudo abrir el puerto: " + e.Message);
                puertoExiste=false;
            }
        }
        else
        {
            Debug.LogWarning("COM no encontrado. Arduino no conectado.");
            puertoExiste=false;
        }
    }
}
