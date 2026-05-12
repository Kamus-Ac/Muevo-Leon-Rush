using System.Collections;
using UnityEngine;

public class CarSpawner2 : MonoBehaviour
{
    [SerializeField] private GameObject[] _carPrefabs;
    [SerializeField] private Transform _busTransform;
    [SerializeField] private LayerMask _carObstacleLayerMask;

    private Collider[] _overlappedCheckColliders = new Collider[1];
    private GameObject[] _carPool = new GameObject[8];
    private WaitForSeconds _wait = new WaitForSeconds(0.5f);

    private float _spawnDistanceToBus = 150.0f;

    //Timing
    private float _timeSinceLastSpawn = 0;
    private float _timeToSpawnMax = 4.0f;




    void Start()
    {
        int prefabIndex = 0;

        for (int i = 0; i < _carPool.Length; i++)
        {
            _carPool[i] = Instantiate(_carPrefabs[prefabIndex]);
            _carPool[i].SetActive(false);

            prefabIndex++;

            //loop if we run out of prefabs, so we can fill the pool
            if (prefabIndex > _carPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
     

        }
        StartCoroutine(UpdateLessOftenCO());
    }


    //better performance
    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            if (GameManager.Instance.IsGamePlaying()) {
                CleanCarsBeyondView();
                SpawnCars();
                yield return _wait;
            }

        }

    }

    private void SpawnCars()
    {
        if(Time.time - _timeSinceLastSpawn < _timeToSpawnMax)
            return;

        GameObject carToSpawn = null;

        //find a car to spawn in the pool
        foreach (GameObject car in _carPool)
        {
            //skip active cars
            if (car.activeInHierarchy)
                continue;

            carToSpawn = car;
            break;
        }

        //if we didnt find any car to spawn, return
        if (carToSpawn == null)
            return;

        //spawn the car
        Vector3 spawnPosition = new Vector3(0,0,_busTransform.position.z + _spawnDistanceToBus);

        Vector3 halfExtents = new Vector3(1.5f, 1.5f, 15f);

        if (Physics.OverlapBoxNonAlloc(spawnPosition, halfExtents, _overlappedCheckColliders, Quaternion.identity, _carObstacleLayerMask) > 0)
            return; //si hay algo en la zona de spawn, no spawnea el obstaculo, para evitar que spawnee encima del coche

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

       

        _timeSinceLastSpawn = Time.time;
    }

    private void CleanCarsBeyondView()
    {
        foreach (GameObject car in _carPool)
        {
            //skip inactive cars
            if (!car.activeInHierarchy)
                continue;
            
            //check if the car is too far ahead
            if(car.transform.position.z - _busTransform.position.z > _spawnDistanceToBus*2) 
                car.SetActive(false);
    
            //check if the car is too far behind
            if (car.transform.position.z - _busTransform.position.z < -  (_spawnDistanceToBus / 4) )
            {
                car.SetActive(false); 
            }
        }
    }

    



}
