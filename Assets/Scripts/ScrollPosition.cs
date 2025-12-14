using UnityEngine;

public class ScrollPosition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform thingToMove;

    public void Scroll(Vector2 scrollByAmount)
    {
        // Move the thing
        thingToMove.position += new Vector3(scrollByAmount.x, scrollByAmount.y, 0);
    }
}
