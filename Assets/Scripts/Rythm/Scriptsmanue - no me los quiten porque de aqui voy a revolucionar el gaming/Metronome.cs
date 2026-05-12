using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float BPM;
    public float BeatDurationInSec;
    //public float Offset; //espacio pequeño de tiempo porque luego el inicio de la cancion no siempre es igual al primer beat
    public float SongPosition;
    public float SongPositionInBeats;
    private float DSPStartSongTime;
    //public float BeatsOnScreen = 4f;
    //public float BeatToShow;

    public GameObject NotePrefab; //x
    public float nextBeatPosition;
    public int lastBeat = 0;

    public void Start()
    {
        DSPStartSongTime = (float)AudioSettings.dspTime;
        BeatDurationInSec = 60f / BPM;
        nextBeatPosition = BeatDurationInSec;
    }

    public void Update()
    {
        SongPosition = (float)(AudioSettings.dspTime - DSPStartSongTime); //aqui habria que restarle offset si es necesario
        SongPositionInBeats = SongPosition / BeatDurationInSec; //convertir el tiempo de la cancion a beats para saber en que beat estamos
        //BeatToShow = SongPositionInBeats + BeatsOnScreen; //el beat que se muestra en pantalla es el beat actual + los beats que queremos mostrar en pantalla
       
        if(SongPositionInBeats >= nextBeatPosition)
        {
            lastBeat += 1;
            nextBeatPosition += BeatDurationInSec;
        }





        /* if(IndexOfNextBeat < MusicNotes.Length && MusicNotes[IndexOfNextBeat] < BeatToShow){ 
            //si todavia falta por recorrer el array de notas y 
            Debug.Log("Show Note");
            IndexOfNextBeat++;
        } */
    }
}


