using UnityEngine;
using UnityEngine.UI;

public class CrosshairEditor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    [SerializeField] private RectTransform centerDot;

    [SerializeField] private Image topImage;
    [SerializeField] private Image bottomImage;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Image centerDotImage;

    [Header("Shape")]
    [Min(0)] public float gap = 8f;
    [Min(1)] public float length = 12f;
    [Min(1)] public float thickness = 3f;
    [Min(0)] public float dotSize = 3f;
    public bool showCenterDot = true;

    [Header("Appearance")]
    public Color color = Color.white;

    private void OnValidate()
    {
        Apply();
    }

    private void Start()
    {
        Apply();
    }

    public void Apply()
    {
        if (!top || !bottom || !left || !right) return;

        // Top
        top.sizeDelta = new Vector2(thickness, length);
        top.anchoredPosition = new Vector2(0f, gap + length * 0.5f);

        // Bottom
        bottom.sizeDelta = new Vector2(thickness, length);
        bottom.anchoredPosition = new Vector2(0f, -(gap + length * 0.5f));

        // Left
        left.sizeDelta = new Vector2(length, thickness);
        left.anchoredPosition = new Vector2(-(gap + length * 0.5f), 0f);

        // Right
        right.sizeDelta = new Vector2(length, thickness);
        right.anchoredPosition = new Vector2(gap + length * 0.5f, 0f);

        // Colors
        if (topImage) topImage.color = color;
        if (bottomImage) bottomImage.color = color;
        if (leftImage) leftImage.color = color;
        if (rightImage) rightImage.color = color;

        // Center dot
        if (centerDot)
        {
            centerDot.gameObject.SetActive(showCenterDot);
            centerDot.sizeDelta = new Vector2(dotSize, dotSize);
            centerDot.anchoredPosition = Vector2.zero;
        }

        if (centerDotImage) centerDotImage.color = color;
    }
}