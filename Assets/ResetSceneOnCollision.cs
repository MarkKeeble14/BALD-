using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskHelper.IsInLayerMask(collision.gameObject, layerMask))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
