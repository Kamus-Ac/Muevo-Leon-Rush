using System;
using UnityEngine;

public class Move_note : MonoBehaviour
{

    public static Action<int> beatFinished;
    RectTransform InitialPos;
    RectTransform FinalPos;

    RectTransform BeatPos;

    bool initialized;

    float BeatNote;
    public float BeatNoteinS;

    int ButtonType;

    float t=0;
    void Start()
    {
        BeatPos=GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(initialized)
        {
            t = (Composer.BeatsOnScreen -(BeatNote - Metronome_Memo.Instance.SongPositionInBeats))
            /(Composer.BeatsOnScreen);
            BeatPos.anchoredPosition = Vector2.Lerp(InitialPos.anchoredPosition, FinalPos.anchoredPosition, t);
            BeatNoteinS=(BeatNote - Metronome_Memo.Instance.SongPositionInBeats)*60
            /Metronome_Memo.Instance.BPM;
            
            ;
            if (BeatNoteinS<=-0.12)
            {

                beatFinished?.Invoke(ButtonType);
                Destroy(gameObject);
            }
        }
        //Debug.Log(BeatPos.anchoredPosition);
    }

    public void Initialize(RectTransform start, RectTransform end, float beat, int buttontype)
    {
        initialized = true;
        InitialPos = start;
        FinalPos = end;
        BeatNote = beat;
        BeatNoteinS = beat/Metronome_Memo.Instance.BPM*60;
        ButtonType = buttontype;
        //Debug.Log(BeatNote);
    }
    /* void Get_Info(GameObject note, float beat)
    {
        InitialPos = note.transform.parent.Find("Initial_Position").GetComponent<RectTransform>();
        FinalPos = note.transform.parent.Find("Final_Position").GetComponent<RectTransform>();
        BeatNote = beat;
        
    } */
}
