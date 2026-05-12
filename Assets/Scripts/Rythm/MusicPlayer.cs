using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource Music;
    void Start()
    {
        Music = GetComponent<AudioSource>();
        Music.Play();
    }


}
