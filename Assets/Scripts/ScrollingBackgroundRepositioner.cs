using UnityEngine;

public class ScrollingBackgroundRepositioner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float cullAfterXPos;
    [SerializeField] private float tileWidth;

    [Header("References")]
    [SerializeField] private Transform[] backgroundComponents;

    // Used to store the last child of the row
    private Transform lastChild;

    // Update is called once per frame
    void Update()
    {
        // For each of the background components
        foreach (Transform t in backgroundComponents)
        {
            // Check if the component is past a certain threshold (x pos)
            if (t.position.x < cullAfterXPos)
            {
                lastChild = t.parent.GetChild(t.parent.childCount - 1);

                // Update the position and move the component to the end of the row
                t.position = lastChild.position + new Vector3(tileWidth, 0, 0);
                t.SetAsLastSibling();
            }
        }
    }
}
