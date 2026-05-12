using System.Collections;
using UnityEngine;

public class EndlessLevel : MonoBehaviour
{
    [SerializeField]
    GameObject[] SectionsPrefabs;

    // Pool of level sections, objetos de lso que se pueden elegir y reelegir
    GameObject[] SectionsPool = new GameObject[20];

    // Sections that are picked from the pool and are currently visible
    GameObject[] VisibleSections = new GameObject[15];

    Transform BusTransform;

    const float SECTIONLENGTH = 22.625f;

    WaitForSeconds WaitFor100ms = new WaitForSeconds(0.1f);

    float OffsetPositionY = -10.0f;

  

    private void Start()
    {
        BusTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        //se crea el pool de secciones

        for (int i = 0; i < SectionsPool.Length; i++)
        {
            SectionsPool[i] = Instantiate(SectionsPrefabs[prefabIndex]);
            SectionsPool[i].SetActive(false);
            prefabIndex++;

            if (prefabIndex >= SectionsPrefabs.Length)
                prefabIndex = 0;

        }


        //Crea las primeras secciones del road, llena el visible sections con secciones random del pool y las posiciona
        for (int i = 0; i < VisibleSections.Length; i++)
        {
            //get the random section
            GameObject randomSection = GetRandomSectionFromPool();

            //move it into position and set it to active                                      
            //x = del prefab
            //y = 0
            //z = seccion i a la distancia correcta de cada road
            randomSection.transform.position = new Vector3(SectionsPool[i].transform.position.x, 0, i * SECTIONLENGTH);
            randomSection.SetActive(true);

            VisibleSections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    //La corutina checka si ya se paso de seccion y se necesita crear una nueva

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return WaitFor100ms;

        }
    }

    void UpdateSectionPositions()
    {
        for (int i = 0; i < VisibleSections.Length; i++)
        {

            //check if the section is too far behind the bus
            if (VisibleSections[i].transform.position.z - BusTransform.position.z < -SECTIONLENGTH)
            {

                //Store the position of the section and disable it
                Vector3 lastSectionPos = VisibleSections[i].transform.position;
                VisibleSections[i].SetActive(false);

                //Get a new random section from the pool
                VisibleSections[i] = GetRandomSectionFromPool();

                //Move the new section
                VisibleSections[i].transform.position = new Vector3(lastSectionPos.x, OffsetPositionY, lastSectionPos.z + (SECTIONLENGTH * VisibleSections.Length));
                VisibleSections[i].SetActive(true);

            }
        }
    }

    //regresa una seccion random del pool y que no esté activa 
    GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, SectionsPool.Length);
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {

            //Check if the sections is not active already
            if (!SectionsPool[randomIndex].activeInHierarchy)
                isNewSectionFound = true;

            else
            {
                //si ya esta activo, seguimos buscando
                randomIndex++;

                //si llegamos al final del pool, volvemos al principio
                if (randomIndex >= SectionsPool.Length)
                    randomIndex = 0;
            }
        }
        return SectionsPool[randomIndex];
    }
}

