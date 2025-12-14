using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PercentageMap<HazardSpawn> potentialHazards = new PercentageMap<HazardSpawn>();
    [SerializeField] private Vector2 minMaxDistBetweenSpawns;
    [SerializeField] private float distanceToNextSpawn = 10;
    private float distanceSinceLastSpawn;

    [Header("References")]
    [SerializeField] private Transform backgroundTileContainer;
    private Transform attachHazardTo => backgroundTileContainer.GetChild(backgroundTileContainer.childCount - 1);

    // Update is called once per frame
    void Update()
    {
        distanceSinceLastSpawn += GameManager._Instance.DistanceTravelledLastFrame;

        // Check if we have covered enough distance to spawn our next hazard
        if (distanceSinceLastSpawn >= distanceToNextSpawn)
        {
            SpawnHazard();

            distanceSinceLastSpawn = 0;
            distanceToNextSpawn = RandomHelper.RandomFloat(minMaxDistBetweenSpawns);
        }
    }

    private void SpawnHazard()
    {
        HazardSpawn toSpawn = potentialHazards.GetOption();
        toSpawn.Spawn(attachHazardTo);
    }
}
