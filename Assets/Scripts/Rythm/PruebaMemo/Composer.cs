using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Composer : MonoBehaviour
{
    public static Action<int,float> noteBegin;
    public static float BeatsOnScreen = 4f;
    public float BeatToShow;
    private int[]IndexOfNextBeatPerLane = new int[3];
    public int SongPositionInBeats = 0;
    public List<float>[] MusicNotes = new List<float>[3];

    public GameObject[]Buttons;

    public GameObject[]ButtonPrefabs;

    bool[] laneFinished = new bool[3];

    private int CountButton=0;

    public int FinishLanes;

    int beatFinish = -1;
    bool waitingWin = false;

    //public GameObject CentralPedal;
    //public GameObject CentralGameFab;
    RectTransform initialPosCentral;

    //public static Action<GameObject, float> initNote;
    void OnEnable()
    {
        Metronome_Memo.actualBeat += On_beat_changed;
    }

    void OnDisable()
    {
        Metronome_Memo.actualBeat -= On_beat_changed;
    }

    void Start()
    {
                // Inicializar las 3 lanes
        for(int i = 0; i < 3; i++)
        {
            MusicNotes[i] = new List<float>();
        }

        ////
        //// LANE 0 (Left)
        ////
        /* float[] lane0 = {
            // Sección 1 (tal como la tenías)
            12f, 16f, 18f, 20f, 24f, 26f, 30f,

            // Sección 2 (notas en beats enteros, repeticiones y dobles)
            43f, 45f, 46f, 49f, 52f, 54f, 58f, 59f, 61f, 62f, 66f, 69f,

            // Sección 3 (más intensidad, jacks y dobles)
            83f, 84f, 88f, 89f, 92f, 93f, 96f, 99f, 101f, 105f, 106f,
            110f, 111f, 114f, 117f, 120f, 121f, 124f, 127f, 130f, 132f, 136f,

            // Final (clímax)
            140f
        };

        ////
        //// LANE 1 (Middle)
        ////
        float[] lane1 = {
            // Sección 1 (igual)
            14f, 16f, 20f, 22f, 26f, 28f,

            // Sección 2
            42f, 47f, 49f, 52f, 53f, 56f, 57f, 60f, 63f, 66f, 67f,

            // Sección 3
            84f, 85f, 88f, 90f, 91f, 95f, 97f, 98f, 101f, 104f, 107f,
           108f, 110f, 113f, 114f, 118f, 119f, 123f, 125f, 126f, 130f, 133f, 135f,

            // Final
            140f
        };

        ////
        //// LANE 2 (Right)
        ////
        float[] lane2 = {
        //    // Sección 1 (igual)
            14f, 18f, 22f, 24f, 28f, 30f,

        //    // Sección 2
            43f, 44f, 48f, 50f, 51f, 55f, 58f, 60f, 64f, 65f, 68f,

        //    // Sección 3
           82f, 86f, 87f, 91f, 94f, 96f, 100f, 102f, 103f, 109f, 112f,
            115f, 116f, 122f, 124f, 128f, 129f, 131f, 134f,

            // Final
           140f
        }; */


        //LANE 0(Left)


        float[] lane0 = {
            // Sección 1 (tal como la tenías)
            12f
        };

        //
        // LANE 1 (Middle)
        //
        float[] lane1 = {
            // Sección 1 (igual)
            14f
        };

        //
        // LANE 2 (Right)
        //
        float[] lane2 = {
            // Sección 1 (igual)
            14f
        };

        MusicNotes[0].AddRange(lane0);
        MusicNotes[1].AddRange(lane1);
        MusicNotes[2].AddRange(lane2);

        On_beat_changed(SongPositionInBeats);
    }

    void On_beat_changed(int beat)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        SongPositionInBeats = beat;

        if (waitingWin && SongPositionInBeats >= beatFinish + 4)
        {
            GameManager.Instance.Winner();
            waitingWin = false;
        }

        BeatToShow = SongPositionInBeats + BeatsOnScreen;

        CountButton = 0;
        
        foreach (List<float> lane in MusicNotes)
        {
            
            if(FinishLanes==3)return;
            
            if (IndexOfNextBeatPerLane[CountButton] < lane.Count 
            && lane[IndexOfNextBeatPerLane[CountButton]] < BeatToShow)
            {
                GameObject LaneNote = Instantiate(ButtonPrefabs[CountButton], Buttons[CountButton].transform);
                RectTransform NoteActual = LaneNote.GetComponent<RectTransform>();

                RectTransform start = Buttons[CountButton].transform.parent.Find("Initial_Position").GetComponent<RectTransform>();
                RectTransform end = Buttons[CountButton].transform.parent.Find("Final_Position").GetComponent<RectTransform>();

                NoteActual.anchoredPosition = start.anchoredPosition;

                noteBegin?.Invoke(CountButton, lane[IndexOfNextBeatPerLane[CountButton]]);
                Move_note move = LaneNote.GetComponent<Move_note>();
                move.Initialize(start,end,lane[IndexOfNextBeatPerLane[CountButton]], CountButton);
                IndexOfNextBeatPerLane[CountButton]++;
            }

            else if (IndexOfNextBeatPerLane[CountButton] == lane.Count && !laneFinished[CountButton])
            {
                laneFinished[CountButton] = true;
                FinishLanes++;

                if (FinishLanes == 3 && !waitingWin)
                {
                    beatFinish = SongPositionInBeats;
                    waitingWin = true;
                }
            }

            CountButton++;
        }
    }
}
