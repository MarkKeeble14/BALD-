using UnityEngine;

[System.Serializable]
public class HazardSpawn
{
    [SerializeField] private GameObject whatToSpawn;
    [SerializeField] private Vector2 rangeWhereCanSpawn;

    public void Spawn(Transform parent)
    {
        GameObject spawned = Object.Instantiate(whatToSpawn, parent);

        // Move the hazard
        spawned.transform.position = new Vector3(
            parent.transform.position.x, 
            RandomHelper.RandomFloat(rangeWhereCanSpawn), 
            parent.transform.position.z);

        // Track the hazard
        parent.GetComponent<TrackAttachedHazards>().AddSpawnedHazard(spawned);
    }
}
