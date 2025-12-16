using UnityEngine;

[System.Serializable]
public class FloorPitData
{
    public GameObject obj;
    public SpriteRenderer sr;
    public BoxCollider2D collider;

    public static int _FloorLayer = 10;
    public static int _PitLayer = 9;

    public void SetSolid()
    {
        sr.enabled = true;
        obj.layer = _FloorLayer;
    }

    public void SetPit()
    {
        sr.enabled = false;
        obj.layer = _PitLayer;
    }
}
