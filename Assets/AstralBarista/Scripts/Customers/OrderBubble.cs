using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int sortingOrder = 200; // above alien sprites

    public void SetGlyph(string glyph, TMP_FontAsset font)
    {
        if (!text) text = GetComponentInChildren<TextMeshPro>(true);
        text.font = font;
        text.text = glyph;
        var r = text.GetComponent<Renderer>();
        if (r) r.sortingOrder = sortingOrder;
    }
}
