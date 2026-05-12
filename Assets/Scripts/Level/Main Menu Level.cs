using UnityEngine;

public class MainMenuLevel : MonoBehaviour
{
    [SerializeField]
    GameObject[] SectionsPrefabs;

    [SerializeField]
    int SectionsToSpawn = 15;

    [SerializeField]
    float SectionLength = 22.625f;

    [SerializeField]
    float OffsetPositionY = 0.0f;

    [SerializeField]
    bool RandomizeSections = true;

    void Start()
    {
        if (SectionsPrefabs == null || SectionsPrefabs.Length == 0)
        {
            Debug.LogWarning("MainMenuLevel: No hay prefabs de secciones asignados.");
            return;
        }

        for (int i = 0; i < SectionsToSpawn; i++)
        {
            GameObject prefabToSpawn;

            if (RandomizeSections)
            {
                int randomIndex = Random.Range(0, SectionsPrefabs.Length);
                prefabToSpawn = SectionsPrefabs[randomIndex];
            }
            else
            {
                prefabToSpawn = SectionsPrefabs[i % SectionsPrefabs.Length];
            }

            GameObject section = Instantiate(prefabToSpawn, transform);
            section.transform.position = new Vector3(prefabToSpawn.transform.position.x, OffsetPositionY, i * SectionLength);
            section.SetActive(true);
        }
    }
}
