using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Composer_Manuel : MonoBehaviour
{
    public GameObject NotePrefab;
    public Queue<GameObject> NotesOnScreen = new Queue<GameObject>();
    public List<GameObject> NotesList = new List<GameObject>();
    public int PoolSize;

    private static Composer_Manuel _instance;
    public static Composer_Manuel Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void OnEnable()
    {
        PoolSize = 4;
        CreateNotes(PoolSize);

    }

    public void CreateNotes(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject Note = Instantiate(NotePrefab);
            Note.SetActive(false);
            NotesList.Add(Note);
            Note.transform.parent = this.transform;
        }
    }

    public void GenerateNotes()
    {
        if (NotesOnScreen.Count > 0)
        {
            NotePrefab = NotesOnScreen.Peek();
            float DistanceToFinishLine = Mathf.Abs(NotePrefab.gameObject.transform.position.y - 0.9f);
        }
    }

    public void AddNote(float beat, Metronome metronome)
    {

        //for (int i = 0; i < NotesList.Count; i++)
        //{
        //    if (!NotesList[i].activeSelf)
        //    {
        //        NotesList[i].SetActive(true);
        //        NotesList[i].transform.position = new Vector3(0, transform.position.y + 0.9f, 0);
        //        NotesOnScreen.Enqueue(NotesList[i]);
        //    }
        //    else
        //    {
        //        //Si no habia ninguna activa, crear una nueva nota
        //        CreateNotes(1);
        //        NotesList[NotesList.Count - 1].SetActive(true);
        //        NotesList[NotesList.Count - 1].transform.position = new Vector3(0, transform.position.y + 0.9f, 0);
        //        NotesOnScreen.Enqueue(NotesList[NotesList.Count - 1]);
        //    }

    
        }

        //MoveNotes(beat, metronome);
    }

    //public void MoveNotes(float beat, Metronome metronome)
    //{
    //    transform.position = new Vector2(transform.position.x, 0.9f + (0 - 0.9f) * (1f - (beat - metronome.SongPosition / metronome.BeatDurationInSec) / metronome.BeatsOnScreen));
    //    if (transform.position.x < 0)
    //    {
    //        for (int i = 0; i < NotesList.Count; i++)
    //        {
    //            if (NotesList[i].activeSelf)
    //            {
    //                NotesList[i].SetActive(false);
    //            }

    //        }
    //    }
    //}


