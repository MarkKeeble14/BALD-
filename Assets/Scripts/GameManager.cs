using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance { get; private set; }

    // The distance the player has travelled
    private float distanceTravelled;
    // Automatically rounds the distance down to the nearest int
    public float DistanceTravelled { get { return Mathf.FloorToInt(distanceTravelled); } }

    // The distance the player has travelled in the last frame
    private float distanceTravelledLastFrame;
    public float DistanceTravelledLastFrame => distanceTravelledLastFrame;

    [Header("References")]
    [SerializeField] private ScrollingBackground scrollingBackground;

    [Header("UI References")]
    [SerializeField] private TMPFloatDisplay distanceTravelledDisplay;


    [Header("Died UI")]
    [SerializeField] private TMPFloatDisplay diedDistanceTravelledDisplay;
    [SerializeField] private GameObject diedUI;

    public bool BlockInput { get; private set; }

    private void Awake()
    {
        // Singleton
        if ( _Instance != null) Destroy(_Instance.gameObject);
        _Instance = this;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void TrackDistance(float distance)
    {
        distanceTravelled += distance;
        distanceTravelledLastFrame = distance;
    }

    private void UpdateUI()
    {
        distanceTravelledDisplay.SetFloat(DistanceTravelled);
        diedDistanceTravelledDisplay.SetFloat(DistanceTravelled);
    }

    public void DieState()
    {
        BlockInput = true;
        diedUI.SetActive(true);
        scrollingBackground.InitiateSlowToAStop();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
