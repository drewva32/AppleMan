using TMPro;
using UnityEngine;

public class FlashTextAlpha : MonoBehaviour
{
    [SerializeField] private float flashSpeed;
    public bool enabled = true;
    
    private TextMeshProUGUI _text;
    private Color _regularColor;
    private Color slightlyTransparent = new Color(1f, 1, 1, 0f);


    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _regularColor = _text.faceColor;
    }

    private void Update()
    {
        if (!enabled)
            return;
        
        _text.faceColor = Color.Lerp(_regularColor, slightlyTransparent, Mathf.Sin(Time.unscaledTime * flashSpeed));
    }
}
