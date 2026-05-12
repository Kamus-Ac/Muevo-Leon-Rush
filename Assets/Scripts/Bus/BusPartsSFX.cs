using UnityEngine;

public class BusPartsSFX : MonoBehaviour
{
    private AudioSource _busPartsBounceAS;

    private void Awake()
    {
        _busPartsBounceAS = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_busPartsBounceAS.isPlaying)
        {
            float minValue = 0.25f;
            float maxValue = 0.65f;

            _busPartsBounceAS.pitch = collision.relativeVelocity.magnitude * 0.5f;
            _busPartsBounceAS.pitch = Mathf.Clamp(_busPartsBounceAS.pitch, minValue, maxValue);
            _busPartsBounceAS.Play();
        }
    }
}
