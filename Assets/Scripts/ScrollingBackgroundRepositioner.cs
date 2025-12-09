using UnityEngine;

public class ScrollingBackgroundRepositioner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float cullAfterXPos;
    [SerializeField] private float repositionToXPos;

    [Header("References")]
    [SerializeField] private Transform[] backgroundComponents;
    
    // Update is called once per frame
    void Update()
    {
        // For each of the background components
        foreach (Transform t in backgroundComponents)
        {
            // Check if the component is past a certain threshold (x pos)
            if (t.position.x < cullAfterXPos)
            {
                // Move the component to the end of the row
                t.position = new Vector3(repositionToXPos, t.position.y, t.position.z);
            }
        }
    }
}
