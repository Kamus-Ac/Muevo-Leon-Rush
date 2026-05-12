using UnityEngine;

public class CarSFX : MonoBehaviour
{
    private AudioSource _carHonkAS;
    [SerializeField] private CarMovement _carMovement;

    private void Awake()
    {
        _carHonkAS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _carMovement.OnCarAheadHonk += CarMovement_OnCarAheadHonk;
    }

    private void OnDestroy()
    {
        _carMovement.OnCarAheadHonk -= CarMovement_OnCarAheadHonk;
    }

    private void CarMovement_OnCarAheadHonk(object sender, System.EventArgs e)
    {
        if (!_carHonkAS.isPlaying)
        {
            _carHonkAS.pitch = Random.Range(0.5f, 1.1f); // Agrega variación de tono para evitar repetición
            _carHonkAS.Play();
        }
    }
}
