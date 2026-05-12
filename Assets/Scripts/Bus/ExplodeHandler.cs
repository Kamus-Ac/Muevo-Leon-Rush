using UnityEngine;
using System.Collections;
public class ExplodeHandler : MonoBehaviour
{
    [SerializeField] private GameObject _fullBusGOModel;
    [SerializeField] private GameObject _originalGO;
    [SerializeField] private BusHandler _busHandler;

    private float _explodeForce = 220f;

    private Rigidbody[] _rBsArray;

    void Awake()
    {
        _rBsArray = _originalGO.GetComponentsInChildren<Rigidbody>(true);
    }

    private void Start()
    {
        //Explode(Vector3.forward);
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Explode(_busHandler.GetVelocity());
            StartCoroutine(SlowDownTimeCO());
        }
    }


    public void Explode(Vector3 externalForce)
    {
        _fullBusGOModel.SetActive(false);


        foreach (Rigidbody RB in _rBsArray)
        {

            RB.transform.parent = null;
            RB.GetComponent<MeshCollider>().enabled = true;
            RB.gameObject.SetActive(true);
            RB.isKinematic = false;
            RB.interpolation = RigidbodyInterpolation.Interpolate;
            RB.AddForce(Vector3.up * _explodeForce + externalForce, ForceMode.Force);
            RB.AddTorque(Random.insideUnitCircle * 0.5f, ForceMode.Impulse);
        }



    }


    //corutina para darle movimiento matrix al perder
    IEnumerator SlowDownTimeCO()
    {
        while (Time.timeScale >= 0.2)
        {
            Time.timeScale -= Time.deltaTime * 2;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;
    }




}
