using System;
using UnityEngine;

public class Metronome_Memo : MonoBehaviour
{
    #region Singleton

    public static Metronome_Memo Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    #endregion
    #region events
    public static Action<int> actualBeat;
    #endregion
    public float BPM;
    public float BeatDurationInSec;
    public float Offset; //espacio pequeño de tiempo porque luego el inicio de la cancion no siempre es igual al primer beat
    public float SongPosition;
    public float SongPositionInBeats {get; private set;}
    public float DSPStartSongTime;
    
    private int LastLoggedBeat = 0;

    private void Start()
    {
        GameManager.Instance.SongBegins += Start_Song;
    }

    private void ODestroy()
    {
        GameManager.Instance.SongBegins -= Start_Song;
    }
    public void Start_Song(object sender, System.EventArgs e)
    {
        DSPStartSongTime = (float)AudioSettings.dspTime;
        BeatDurationInSec = 60f / BPM;
    }

    public void Update()
    {
        SongPosition = (float)(AudioSettings.dspTime - DSPStartSongTime ); //aqui habria que restarle offset si es necesario
        SongPositionInBeats = SongPosition / BeatDurationInSec; //convertir el tiempo de la cancion a beats para saber en que beat estamos
        //BeatToShow = SongPositionInBeats + BeatsOnScreen; //el beat que se muestra en pantalla es el beat actual + los beats que queremos mostrar en pantalla

        int CurrentBeat = Mathf.FloorToInt(SongPositionInBeats) + 1;
        if (CurrentBeat > LastLoggedBeat)
        {
            LastLoggedBeat = CurrentBeat;
            //Debug.Log($"Beat actual: {CurrentBeat}");
            actualBeat?.Invoke(CurrentBeat);
        }
    }
}


