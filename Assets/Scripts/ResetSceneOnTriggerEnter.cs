using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMaskHelper.IsInLayerMask(collision.gameObject, layerMask))
        {
            GameManager._Instance.DieState();
        }
    }
}
