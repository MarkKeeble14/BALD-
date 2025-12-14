using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Stage Scrolling Settings")]
    [SerializeField] private ScrollPosition[] stageScrollers;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Vector2 scrollAxis = new Vector2(-1, 0);
    [SerializeField] private float rateOfSpeedIncrease = 0.0001f;
    private float currentIncrease = 1;
    private Vector3 scrollByAmount;

    [Header("Stage Culling")]
    [SerializeField] private float cullAfterXPos;
    [SerializeField] private float tileWidth;

    // Used to store the last child of the row
    private Transform lastChild;

    // The distance the player has travelled
    private float distanceTravelled;
    // Automatically rounds the distance down to the nearest int
    public float DistanceTravelled { get { return Mathf.FloorToInt(distanceTravelled); } }

    private void Update()
    {
        // Increase the speed of the stage
        currentIncrease += rateOfSpeedIncrease;

        // Calculate how much to scroll by
        // scrollSpeed = the base scroll speed
        // currentIncrease = the increase to scroll speed over time
        scrollByAmount = scrollAxis * scrollSpeed * currentIncrease * Time.deltaTime;
        scrollByAmount.z = 0;

        // Apply scrolling to everything needed to scroll
        Transform scrollingTransform;
        TrackAttachedHazards trackAttachedHazards;
        foreach (ScrollPosition toScroll in stageScrollers)
        {
            toScroll.Scroll(scrollByAmount);

            scrollingTransform = toScroll.transform;
            trackAttachedHazards = toScroll.GetComponent<TrackAttachedHazards>();

            // Check if the component is past a certain threshold (x pos)
            if (scrollingTransform.position.x < cullAfterXPos)
            {
                lastChild = scrollingTransform.parent.GetChild(scrollingTransform.parent.childCount - 1);

                // Update the position and move the component to the end of the row
                scrollingTransform.position = lastChild.position + new Vector3(tileWidth, 0, 0);
                scrollingTransform.SetAsLastSibling();

                // Destroys all hazards attached to repositioned column
                trackAttachedHazards.AnnihilateHazards();
            }
        }

        // Tell the GameManager how much distance has been travelled this frame
        // Only ever moving one direction, so taking the absolute value of the movement
        GameManager._Instance.TrackDistance(Mathf.Abs(scrollByAmount.x));
    }
}
