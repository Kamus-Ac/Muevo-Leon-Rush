using UnityEngine;

public class EndlessSectionHandler : MonoBehaviour
{
    Transform BusTransform;
    float DistanceToStartLerp = 100.0f;
    float TransitionLength = 150.0f;
    float OffsetPositionY = -10.0f;

    void Start()
    {
        BusTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
     void Update()
     {
        float distanceToPlayer = transform.position.z - BusTransform.position.z; 

        float lerpPercentage = 1.0f - ((distanceToPlayer-DistanceToStartLerp) / TransitionLength);
        lerpPercentage = Mathf.Clamp01(lerpPercentage);

        transform.position = Vector3.Lerp(new Vector3(transform.position.x,OffsetPositionY,transform.position.z), new Vector3(transform.position.x,0,transform.position.z), lerpPercentage);
      
     }
}
