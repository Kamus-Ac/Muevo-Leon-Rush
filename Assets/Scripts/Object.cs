using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] private float _pulseSize = 1.5f;
    [SerializeField] private float _returnSpeed = 5.0f;
    private Vector3 _originalScale;

    void Start()
    {
        _originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * _returnSpeed);
    }

    public void Pulse()
    {
        transform.localScale = _originalScale * _pulseSize;
    }
}
