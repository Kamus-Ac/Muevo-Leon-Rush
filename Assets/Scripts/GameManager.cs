using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Linq;
using System.IO.Ports;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;

    public event EventHandler SongBegins;

    [SerializeField] private float maxHealth = 100f;
    public float Health;

    public RectTransform[] Bar;
    public Image barColor;

    public GameObject[] Canvases;
    public AudioSource song;

    public GameObject CycleDayNight;
    float initial_rotation;
    private float delay = 0.5f;
    public float time;
    public GameObject Sparkles;
    public GameObject MotionLines;

    public int perfect;
    public int good;
    public int mistakes;

    public int score;

    public TextMeshProUGUI [] Points ;

    public int good_value = 100;

    public float Accuracy;

    public int combo;

    private int damange = 5;
    private Coroutine healthBar;

    //Timers
    private float _countDownTimer = 5;

    //Puerto del arduino
    public SerialPort serial = new SerialPort("COM9", 9600);

    public bool puertoExiste = false;
    private enum State
    {
        WaitingToCountDown,
        CountDown,
        GamePlaying,
        Winner,
        GameOver
    }
    private State _currState;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Health = maxHealth;
        _currState = State.WaitingToCountDown;
        initial_rotation = Mathf.Rad2Deg*CycleDayNight.transform.rotation.x*2;
        TryOpenSerial();
    }

    private void Start()
    {
        InputHandler.Instance.OnSwitchAction += InputHandler_OnSwitchAction;
        IH_Rythm.BeginAction += InputHandler_OnSwitchAction2;
        
    }

    private void OnDestroy()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnSwitchAction -= InputHandler_OnSwitchAction;
        }

        IH_Rythm.BeginAction -= InputHandler_OnSwitchAction2;

        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }

            serial.Dispose();
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }

    void OnApplicationQuit()
    {
        if (puertoExiste)
        {
            serial.Close();
        }
        
    }
    private void Update()
    {
        switch (_currState)
        {
            case State.WaitingToCountDown:
                break;

            case State.CountDown:
                _countDownTimer -= Time.deltaTime;
                CambiarModoArduino(5);
                if (_countDownTimer < 0)
                {
                    _currState = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    SongBegins?.Invoke(this, EventArgs.Empty);
                    song.enabled= true;
                }
                break;

            case State.GamePlaying:
                break;

            case State.GameOver:
                break;

        }
        
        
        
        
        
        
        
        
        //Debug.Log(_currState);

    }

    private void FixedUpdate()
    {
        CiclarDiayNoche();
    }

    public bool IsGamePlaying()
    {
        return _currState == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return _currState == State.GameOver;
    }

    public bool IsWinner()
    {
        return _currState == State.Winner;
    }

    public bool IsCountDownActive()
    {
        return _currState == State.CountDown;
    }

    public float GetCountDownTimer()
    {
        return _countDownTimer;
    }


    private void InputHandler_OnSwitchAction(object sender, System.EventArgs e)
    {
        if (_currState == State.WaitingToCountDown)
        {
            _currState = State.CountDown;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }

        if(_currState == State.GameOver)
        {
            _currState = State.WaitingToCountDown;

            Loader.Load(Loader.Scene.FinalGame1);
        }

        if (_currState == State.Winner)
        {
            _currState = State.WaitingToCountDown;

            Loader.Load(Loader.Scene.FinalGame1);
        }

    }

    private void InputHandler_OnSwitchAction2()
    {
        if (_currState == State.WaitingToCountDown)
        {
            _currState = State.CountDown;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }

        if(_currState == State.GameOver)
        {
            _currState = State.WaitingToCountDown;

            Loader.Load(Loader.Scene.FinalGame1);
        }

        if (_currState == State.Winner)
        {
            _currState = State.WaitingToCountDown;

            Loader.Load(Loader.Scene.FinalGame1);
        }
    }

    public void GameOver()
    {
        if (_currState == State.GameOver) return;

        CambiarModoArduino(6);
        _currState = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Winner()
    {
        _currState = State.Winner;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        Canvases[0].SetActive(false);
        Canvases[1].SetActive(true);

    }


    public void RegisterPerfect()
    {
        if (IsGameOver()) return;
        perfect++;
        combo++;
        Check_accuracy();
        CambiarModoArduino(3);
    }

    public void RegisterGood()
    {
        if (IsGameOver()) return;
        good++;
        combo++;
        Check_accuracy();
        CambiarModoArduino(2);
    }

    public void RegisterMistake()
    {
        if (IsGameOver()) return;
        mistakes++;
        combo = 0;
        TakeDamage(damange);
        Check_accuracy();
        CambiarModoArduino(1);
    }

    public void TakeDamage(float damage)
    {
        if(Health<=100 && Health > 0)
        {
            var antHealth = Health;
            Health -= damage;
            healthBar= StartCoroutine(ScaleBar(antHealth));
        }
        

        if (Health == 0)
        {
            Health = 0;
            GameOver();
        }
    }

    void Check_accuracy()
    {
        Accuracy = Mathf.RoundToInt((good + perfect) * 100f / (good + perfect + mistakes));
        score = (good * good_value) + (perfect * good_value*3);
        UpdateScore();
    }

    void LograrCombode10()
    {
        if (combo == 10)
        {
            CambiarModoArduino(4);
        }
    }


    IEnumerator ScaleBar(float antHealth)
    {
        if (healthBar != null)
        {
            StopCoroutine(healthBar);
        }

        float elapsed = 0;
        float duration = 0.5f;

        float final =Health*Bar[0].localScale.x/antHealth;
        float final2 =Health*Bar[1].localScale.x/antHealth;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scalex = Mathf.Lerp(Bar[0].localScale.x, final, elapsed/duration);
            float scalex2 = Mathf.Lerp(Bar[1].localScale.x, final2, elapsed/duration);
            ChangeColorHealthBar();
            Vector3 scale= Bar[0].localScale;
            Vector3 scale2= Bar[1].localScale;
            scale.x=scalex;
            scale2.x=scalex2;
            Bar[0].localScale=scale;
            Bar[1].localScale=scale2;


            yield return null;
        }
        Vector3 finalScale = Bar[0].localScale;
        Vector3 finalScale2 = Bar[1].localScale;
        finalScale.x = final;
        finalScale2.x = final2;
        Bar[0].localScale = finalScale;
        Bar[1].localScale = finalScale2;
    }

    private void ChangeColorHealthBar()
    {
        float healthpercent = Health/maxHealth;
        float healthpercentagecolor= Mathf.InverseLerp(0.2f,1f,healthpercent);
        Color color = Vector4.Lerp(new Vector4(255,0,0,255), new Vector4(0,255,0,255), healthpercentagecolor);
        barColor.color = color;
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

    void CambiarModoArduino(int num)
    {
        if (puertoExiste)
        {
            serial.Write(num.ToString());
        }
    }

    void CiclarDiayNoche()
    {
        time += Time.fixedDeltaTime * delay;
        if (time < 24f)
        {
            CycleDayNight.transform.rotation= Quaternion.AngleAxis(initial_rotation+time*360/24, Vector3.right);
        }

        else
        {
            time = 0;
        }

    }

    public void ActivarSparkles()
    {
        Sparkles.SetActive(true);
    }

    public void ActivarLines()
    {
        MotionLines.SetActive(true);
    }
    
    public void DesactivarSparkles()
    {
        Sparkles.SetActive(false);
    }

    public void DesactivarLines()
    {
        MotionLines.SetActive(false);
    }

    public void UpdateScore()
    {
        for (int i=0; i< Points.Length; i++)
        {
            if (i<2)
            {
                Points[i].text = score.ToString();
            }

            if(i==2)
            {
                Points[i].text = perfect.ToString();
            }

            if(i==3)
            {
                Points[i].text = good.ToString();
            }

            if(i==4)
            {
                Points[i].text = mistakes.ToString();
            }
        }
    }
    
}
