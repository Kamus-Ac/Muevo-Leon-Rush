using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //And Effects Manager siiii
    [SerializeField] private AudioSource _busEngineAS;
    [SerializeField] private AudioSource _busCrashAS;
    [SerializeField] private AnimationCurve _busPitchAnimationCurve;

    [SerializeField] private BusHandler _busHandler;

    bool stateEffect = false;
    bool stateEffectLines = false;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        _busEngineAS.volume = 0.25f;

    } 

    private void Update()
    {
        UpdateCarAudio();
        ActivateSparkles();
        ActivateLines();

        if (GameManager.Instance.IsGameOver())
        {
            FadeOutCarAudio();
        }
    }

    private void OnDestroy()
    {
        stateEffect = false;
        stateEffectLines = false;
        if (GameManager.Instance)
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }


    //Funciones
    private void PlayAudioSourceSound(AudioSource audioSource)
    {
        audioSource.Play();
    }

    private void UpdateCarAudio(){

        if(_busEngineAS.isPlaying)
            _busEngineAS.pitch = _busPitchAnimationCurve.Evaluate(_busHandler.GetSpeedPercentage());
    }

    private void FadeOutCarAudio()
    {
        float multiplier = 10f;
        _busEngineAS.volume = Mathf.Lerp(_busEngineAS.volume, 0, Time.deltaTime * multiplier);

    }

    //Eventos
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsGamePlaying() && !_busEngineAS.isPlaying)
        {
            PlayAudioSourceSound(_busEngineAS);
        }

        if(GameManager.Instance.IsGameOver())
        {
            float maxSpeedPercentage = _busHandler.GetSpeedPercentage();
            
            _busCrashAS.volume = maxSpeedPercentage;
            _busCrashAS.pitch = maxSpeedPercentage;
            
            float minVolume = 0.35f;
            float maxVolume = 0.7f;

            _busCrashAS.volume = Mathf.Clamp(_busCrashAS.volume, minVolume, maxVolume);
            _busCrashAS.pitch = Mathf.Clamp(_busCrashAS.pitch, minVolume + 0.2f, maxVolume);
            PlayAudioSourceSound(_busCrashAS);

        }

    }

    private void ActivateSparkles()
    {
        //print(_busHandler.GetSpeedPercentage());
        if (_busHandler.GetSpeedPercentage() > 0.7f)
        {
            if (!stateEffect)
            {
                GameManager.Instance.ActivarSparkles();
                stateEffect=true;
            }
        }
        else if (_busHandler.GetSpeedPercentage() <=0.7f)
        {
            if (stateEffect)
            {
                GameManager.Instance.DesactivarSparkles();
                stateEffect=false;
            }
        }
    }

    private void ActivateLines()
    {
        //print(_busHandler.GetSpeedPercentage());
        if (_busHandler.GetSpeedPercentage() > 0.9f)
        {
            if (!stateEffectLines)
            {
                GameManager.Instance.ActivarLines();
                stateEffectLines=true;
            }
        }
        else if (_busHandler.GetSpeedPercentage() <=0.9f)
        {
            if (stateEffectLines)
            {
                GameManager.Instance.DesactivarLines();
                stateEffectLines=false;
            }
        }
    }
}

