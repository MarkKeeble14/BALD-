using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private TrackAttachedHazards trackAttachedHazards;
    [SerializeField] private FloorPitData[] floorTiles;
    [SerializeField] private Vector2 chanceToDisableFloorSegment;

    public void OnReposition(bool disableFloor)
    {
        // Destroy any hazards
        trackAttachedHazards.AnnihilateHazards();

        // Re-enable floor segments
        foreach (FloorPitData tile in floorTiles)
        {
            tile.SetSolid();
        }

        // Disable floor segments randomly
        float chanceToDisable = chanceToDisableFloorSegment.x / chanceToDisableFloorSegment.y;
        if (disableFloor)
        {
            foreach (FloorPitData tile in floorTiles)
            {
                if (RandomHelper.RandomBool(chanceToDisable))
                {
                    tile.SetPit();
                }
            }
        }
    } 
}
