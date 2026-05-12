using System;
using System.Collections;
using UnityEngine;

public class CarMovement : VehicleMovement
{
    public event EventHandler OnCarAheadHonk;

    [SerializeField] private LayerMask _carObstacleLayer;
    [SerializeField] private GameObject _ParentGO;
    private Rigidbody[] _rBsArray;

    private RaycastHit[] _raycastHits = new RaycastHit[1];
    private bool _isCarAhead = false;
    private WaitForSeconds _wait = new WaitForSeconds(0.05f);

    private float _distanceToCarAhead = 23.0f;
    private float _carAheadDistance = 0f;

    const float ROTATIONFORCE = 5.0f;
    private float _explodeForce = 50f;

    private float _accelerationInput = 1.0f;
    private float _steeringInput = 0.0f;

    private int _drivingLane = 0;

    private void Awake()
    {

        _rBsArray = _ParentGO.GetComponentsInChildren<Rigidbody>(true);
    }


    void Start()
    {
        StartCoroutine(UpdateLessoftenCO());
        RB = GetComponent<Rigidbody>();
        AccelerationForce = 1f;
        MaxForwardVelocity = UnityEngine.Random.Range(2.0f, 6.0f);
        _drivingLane = UnityEngine.Random.Range(0, Utils.CarLanes.Length);
        //Debug.Log(_drivingLane);

    }



    // Update is called once per frame
    void Update()
    {
        if (IsExploded) return;

        if (_isCarAhead)
        {
            float distanceToHonk = 15.0f;
            if (_carAheadDistance < distanceToHonk)
            {
                OnCarAheadHonk?.Invoke(this, EventArgs.Empty);
            }
        }

        float currentSpeed = RB.linearVelocity.z;
        float targetSpeed = _isCarAhead ? 0 : MaxForwardVelocity;

        if (currentSpeed < targetSpeed)
        {
            _accelerationInput = 1;
        }
        else if (currentSpeed > targetSpeed)
        {
            _accelerationInput = -0.3f; // freno suave
        }
        else
        {
            _accelerationInput = 0;
        }

        _steeringInput = Mathf.Clamp(_steeringInput, -1.0f, 1.0f);




    }



    private void FixedUpdate()
    {
        if (IsExploded)
        {
            RB.linearDamping = RB.linearVelocity.z * 0.1f;
            RB.linearDamping = Mathf.Clamp(RB.linearDamping, 1.5f, 10f);
            //slowly move car to the center of the lane as it explodes, so it doesn't fly off to the sides as much
            RB.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.fixedDeltaTime * 0.5f));
            RB.linearVelocity = new Vector3(0, 0, 0);
        }

        if (_accelerationInput > 0)
            Accelerate();
        else if (_accelerationInput < 0)
            Break(1);

        //Steer(_steeringInput);

        //if (RB.linearVelocity.z <= 0)
        //    RB.linearVelocity = Vector3.zero;

    }


    IEnumerator UpdateLessoftenCO()
    {
        while (true)
        {
            _isCarAhead = CheckCarAhead();
            yield return _wait;
        }

    }


    private bool CheckCarAhead()
    {

        Vector3 halfExtents = new Vector3(1.0f, 1.0f, 3.0f);

        int numberofHits = Physics.BoxCastNonAlloc(transform.position, halfExtents, transform.forward, _raycastHits, Quaternion.identity, _distanceToCarAhead, _carObstacleLayer);

        if (numberofHits > 0)
        {
            Transform hitTransform = _raycastHits[0].transform;

            if (hitTransform.root != transform.root)
            {
                _carAheadDistance = Vector3.Distance(transform.position, hitTransform.position);
                return true;
            }

        }

        return false;
    }

    public void Explode(Vector3 externalForce)
    {

        
        foreach (Rigidbody RB in _rBsArray)
        {

            RB.transform.parent = null;
            RB.GetComponent<MeshCollider>().enabled = true;
            RB.gameObject.SetActive(true);
            RB.isKinematic = false;
            RB.interpolation = RigidbodyInterpolation.Interpolate;
            RB.AddForce(Vector3.up * _explodeForce + externalForce, ForceMode.Force);
            RB.AddTorque(UnityEngine.Random.insideUnitCircle * 0.5f, ForceMode.Impulse);
        }
        RB.isKinematic = false;
        RB.interpolation = RigidbodyInterpolation.Interpolate;
        RB.AddForce(Vector3.up * _explodeForce + externalForce, ForceMode.Force);
        RB.AddTorque(UnityEngine.Random.insideUnitCircle * 0.5f, ForceMode.Impulse);

    }

    protected override void Accelerate()
    {
        base.Accelerate();
        RB.AddForce(transform.forward * AccelerationForce);
    }


    protected override void Break(int button)
    {
        base.Break(button);
        RB.AddForce(transform.forward * -BreakForce);
    }

    //Eventos
    private void OnCollisionEnter(Collision collision)
    {
        IsExploded = true;
        Explode(RB.linearVelocity);
    }



}
