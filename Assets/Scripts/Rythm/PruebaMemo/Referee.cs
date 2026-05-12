using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class Referee : MonoBehaviour
{
    /* public float Accuracy=0;

    private int Perfect=0;

    private int Good=0;

    private int Mistakes=0;

    public int Combo = 0; */

    private bool Begin = false;

    public struct DataNote
    {
        public int Lane;
        public float Beat;
    }

    private Queue<DataNote>[] Notes ;

    public GameObject[] Lanes;

    public static Action<int> Mademistake;
    public static Action<int> Madegood;

    public static Action<int> Madeperfect;

    void OnEnable()
    {
        Move_note.beatFinished+=Miss;
        Composer.noteBegin+=Fill_note;
        IH_Rythm.buttonPress+=Check;
    }

    void OnDisable()
    {
        Move_note.beatFinished-=Miss;
        Composer.noteBegin-=Fill_note;
        IH_Rythm.buttonPress-=Check;
    }

    // Update is called once per frame
    void Start()
    {
        Notes=new Queue<DataNote>[5];
        for (int i = 0; i<5 ; i++)
        {
            Notes[i]= new Queue<DataNote>();
        }
    }
    
    void Fill_note(int lane, float beat)
    {
        DataNote dataNote = new DataNote
        {
            Lane=lane,
            Beat=beat
        };
        Notes[lane].Enqueue(dataNote);
        Begin = true;
        //Debug.Log(Begin);
    }

    void Check(int button, float SongPosition)
    {
        if (!Begin) return;

        if (Notes[button].Count == 0) return; // evitar crash si no hay notas

        DataNote note = Notes[button].Peek();

        float noteTime = (note.Beat / Metronome_Memo.Instance.BPM) * 60f;
        float difference = Mathf.Abs(SongPosition - noteTime);
        //Debug.Log(difference);

        Transform lane = Lanes[button].transform;

        if (lane.childCount > 0)
        {
            Transform lastChild = lane.GetChild(0);

            if (difference <= 0.080f)
            {
                /* Perfect++;
                Combo++; */
                Destroy(lastChild.gameObject);
                Notes[button].Dequeue();
                /* Debug.Log("perfect" + Perfect); */
                GameManager.Instance.RegisterPerfect();
                Madeperfect?.Invoke(button);
            }
            else if (difference <= 0.100f)
            {
                /* Good++;
                Combo++; */
                Destroy(lastChild.gameObject);
                Notes[button].Dequeue();
                /* Debug.Log("Good:" + Good); */
                GameManager.Instance.RegisterGood();
                Madegood?.Invoke(button);
            }
            else if (difference <= 0.120f)
            {
                Miss(button);
                Destroy(lastChild.gameObject);
                GameManager.Instance.RegisterMistake();
                Mademistake?.Invoke(button);
                
            }
            else
            {
                Miss(button);
                Destroy(lastChild.gameObject);
                GameManager.Instance.RegisterMistake();
                Mademistake?.Invoke(button);
            }
        }

        

        /* Accuracy = (Good + Perfect) / (float)(Mistakes + Good + Perfect) * 100; */
        difference = 0;
        //Debug.Log(Accuracy);
    }

    void Miss(int button)
    {
        /* Mistakes++;
        Combo = 0; */
        Notes[button].Dequeue();
        GameManager.Instance.RegisterMistake();
        Mademistake?.Invoke(button);
        //Debug.Log(Mistakes);
    }

}
