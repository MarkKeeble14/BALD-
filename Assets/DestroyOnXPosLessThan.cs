using UnityEngine;

public class DestroyOnXPosLessThan : MonoBehaviour
{
    [SerializeField] private int xBound;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < xBound)
        {
            Destroy(gameObject);
        }
    }
}
