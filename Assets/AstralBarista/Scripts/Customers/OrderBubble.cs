using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderBubbleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI glyphText;
    [SerializeField] private Image bubbleBg;


    public void SetGlyph(string glyph, TMP_FontAsset font)
    {
        if (!glyphText) glyphText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (font) glyphText.font = font;
        glyphText.text = glyph;
    }

    // Optional helpers
    public void SetBackground(Sprite sprite)
    {
        if (bubbleBg && sprite) bubbleBg.sprite = sprite;
    }
}