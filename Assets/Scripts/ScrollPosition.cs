using UnityEngine;

public class ScrollPosition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Vector2 scrollAxis;
    [SerializeField] private float rateOfSpeedIncrease = 0.0001f;
    private float currentIncrease = 1;

    [Header("References")]
    [SerializeField] private Transform thingToMove;
    private Vector3 scrollByAmount;

    // Update is called once per frame
    void Update()
    {
        // Increase the speed of the stage
        currentIncrease += rateOfSpeedIncrease;

        // Calculate how much to scroll by
        // scrollAxis = the direction to scroll in
        // scrollSpeed = the base scroll speed
        // currentIncrease = the increase to scroll speed over time
        scrollByAmount = scrollAxis * scrollSpeed * currentIncrease * Time.deltaTime;

        // Move the thing
        thingToMove.position += new Vector3(scrollByAmount.x, scrollByAmount.y, 0);
    }
}
