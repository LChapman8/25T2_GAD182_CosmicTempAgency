using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVisuals : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private readonly List<Color> _baseColors = new List<Color>();
    private Coroutine _flashCo;
    private bool _isTinted;

    void Awake()
    {
        if (renderers.Count == 0)
            renderers.AddRange(GetComponentsInChildren<SpriteRenderer>(includeInactive: true));

        _baseColors.Clear();
        foreach (var r in renderers)
            _baseColors.Add(r ? r.color : Color.white);
    }

    // Public flash: cancels any running flash and guarantees at least one frame of visible tint
    public IEnumerator Flash(Color flashColor, int pulses, float pulseDuration)
    {
        if (_flashCo != null)
        {
            StopCoroutine(_flashCo);
            _flashCo = null;
            ResetTint();
        }
        _flashCo = StartCoroutine(CoFlash(flashColor, pulses, pulseDuration));
        yield return _flashCo;
        _flashCo = null;
    }

    private IEnumerator CoFlash(Color flashColor, int pulses, float pulseDuration)
    {
        for (int i = 0; i < pulses; i++)
        {
            // ON
            SetTint(flashColor);
            _isTinted = true;
            yield return null; // ensure one rendered frame with the tint

            float t = 0f;
            while (t < pulseDuration)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            // OFF
            ResetTint();
            _isTinted = false;
            yield return null; // ensure one frame in base color

            t = 0f;
            while (t < pulseDuration)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    // Fade from base colors; never from a temporary tint
    public IEnumerator FadeOut(float duration)
    {
        // Don’t fade while flashing
        if (_flashCo != null)
        {
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
        if (_isTinted) { ResetTint(); _isTinted = false; }

        duration = Mathf.Max(0.0001f, duration);
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float aMul = 1f - Mathf.Clamp01(t / duration);

            for (int i = 0; i < renderers.Count; i++)
            {
                var r = renderers[i];
                if (!r) continue;
                var baseC = _baseColors[i];
                var c = baseC;           // start from base color, not current r.color
                c.a = baseC.a * aMul;    // scale only alpha
                r.color = c;
            }
            yield return null;
        }
    }

    public void SetTint(Color c)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i];
            if (!r) continue;
            var baseC = _baseColors[i];
            var tint = c; tint.a = baseC.a; // keep original alpha
            r.color = tint;
        }
    }

    public void ResetTint()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i];
            if (r) r.color = _baseColors[i];
        }
    }
}