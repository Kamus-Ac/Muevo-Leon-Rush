using UnityEngine;

public class RandomizeObjects : MonoBehaviour
{
    [SerializeField]
    Vector3 localRotationMin = Vector3.zero;

    [SerializeField]
    Vector3 localRotationMax = Vector3.zero;

    [SerializeField]
    float localScaleMin = 2.0f;

    [SerializeField]
    float localScaleMax = 3.5f;

    Vector3 localScaleOriginal = Vector3.one;

    private void Start()
    {
        localScaleOriginal = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(
            Random.Range(localRotationMin.x, localRotationMax.x),
            Random.Range(localRotationMin.y, localRotationMax.y),
            Random.Range(localRotationMin.z, localRotationMax.z)
        );

        transform.localScale = localScaleOriginal * Random.Range(localScaleMin, localScaleMax);
    }

}
