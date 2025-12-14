using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPFloatDisplay : MonoBehaviour
{
    private float var;
    private TextMeshProUGUI text;

    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    private void Awake()
    {
        // Fetch the TMP component
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update Text
        text.text = prefix + var + suffix;
    }

    public void SetFloat(float f)
    {
        var = f;
    }

    public float GetFloat()
    {
        return var;
    }
}
