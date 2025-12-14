using System.Collections.Generic;
using UnityEngine;

public class TrackAttachedHazards : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnedHazards = new List<GameObject>();

    public void AddSpawnedHazard(GameObject obj)
    {
        spawnedHazards.Add(obj);
    }

    public void AnnihilateHazards()
    {
        while (spawnedHazards.Count > 0)
        {
            GameObject hazard = spawnedHazards[0];
            spawnedHazards.RemoveAt(0);
            Object.Destroy(hazard);
        }
    }
}
