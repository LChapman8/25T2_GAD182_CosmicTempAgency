using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePotHighlight : MonoBehaviour
{
    [Header("Targets (leave empty to auto-grab child SpriteRenderers)")]
    [SerializeField] private List<SpriteRenderer> renderers = new();

    [Header("Hover Pulse (when dragging over pot)")]
    [SerializeField] private float pulseSpeed = 6f;       // cycles per second-ish
    [SerializeField] private float pulseStrength = 0.45f; // 0..1 toward white

    public enum FlashMode { OpacityFlash, SelfBrightness }
    [Header("Drop Flash")]
    [SerializeField] private FlashMode flashMode = FlashMode.OpacityFlash;
    [Tooltip("OpacityFlash: how much to dip alpha (0 = none, 1 = fully invisible)")]
    [Range(0f, 1f)][SerializeField] private float alphaDip = 0.45f;
    [Tooltip("SelfBrightness: multiply base color by this; <1 darkens, >1 brightens")]
    [SerializeField] private float brightFactor = 1.4f;
    [Tooltip("Total time the flash effect lasts")]
    [SerializeField] private float flashDuration = 0.12f;

    private readonly List<Color> _base = new();
    private Coroutine _pulseCo, _flashCo;
    private bool _highlightOn;

    void Awake()
    {
        if (renderers.Count == 0)
            renderers.AddRange(GetComponentsInChildren<SpriteRenderer>(includeInactive: true));

        _base.Clear();
        foreach (var r in renderers)
            _base.Add(r ? r.color : Color.white);
    }

    // --- Public API ---
    public void SetHighlight(bool on)
    {
        if (on == _highlightOn) return;
        _highlightOn = on;

        if (_highlightOn)
        {
            if (_flashCo != null) { StopCoroutine(_flashCo); _flashCo = null; }
            if (_pulseCo == null) _pulseCo = StartCoroutine(CoPulse());
        }
        else
        {
            if (_pulseCo != null) { StopCoroutine(_pulseCo); _pulseCo = null; }
            ResetToBase();
        }
    }

    public void KickFlash()
    {
        if (_flashCo != null) StopCoroutine(_flashCo);
        _flashCo = StartCoroutine(CoFlash());
    }

    // --- Effects ---
    private IEnumerator CoPulse()
    {
        while (_highlightOn)
        {
            float t = Mathf.PingPong(Time.unscaledTime * pulseSpeed, 1f) * pulseStrength;
            ApplyTowardWhite(t);
            yield return null;
        }
        _pulseCo = null;
    }

    private IEnumerator CoFlash()
    {
        // Suspend hover pulse during the flash
        bool wasPulsing = _pulseCo != null;
        if (wasPulsing) { StopCoroutine(_pulseCo); _pulseCo = null; }

        switch (flashMode)
        {
            case FlashMode.OpacityFlash:
                yield return CoOpacityFlash();
                break;
            case FlashMode.SelfBrightness:
                yield return CoSelfBrightnessFlash();
                break;
        }

        // Restore hover pulse or base
        if (_highlightOn && wasPulsing) _pulseCo = StartCoroutine(CoPulse());
        else ResetToBase();

        _flashCo = null;
    }

    // Quick dip in opacity, then back to base — very visible even on white sprites
    private IEnumerator CoOpacityFlash()
    {
        // guarantee at least one frame of the dipped state
        SetAlphaScale(1f - Mathf.Clamp01(alphaDip));
        yield return null;

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // restore base alpha
        RestoreBaseAlpha();
    }

    private IEnumerator CoSelfBrightnessFlash()
    {
        SetBrightness(brightFactor);
        yield return null; // ensure a rendered frame

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // restore base color
        ResetToBase();
    }

    // --- Helpers ---
    private void ApplyTowardWhite(float lerp)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i]; if (!r) continue;
            var b = _base[i];
            var target = Color.white; target.a = b.a;
            r.color = Color.Lerp(b, target, Mathf.Clamp01(lerp));
        }
    }

    private void ResetToBase()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i];
            if (r) r.color = _base[i];
        }
    }

    private void SetAlphaScale(float scale)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i]; if (!r) continue;
            var b = _base[i];
            var c = b; c.a = b.a * Mathf.Clamp01(scale);
            r.color = c;
        }
    }

    private void RestoreBaseAlpha()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i];
            if (!r) continue;
            var b = _base[i];
            var c = r.color; c.a = b.a;
            r.color = c;
        }
    }

    private void SetBrightness(float factor)
    {
        // Multiply base RGB, clamp to 0..1; preserves original alpha
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i]; if (!r) continue;
            var b = _base[i];
            var c = new Color(
                Mathf.Clamp01(b.r * factor),
                Mathf.Clamp01(b.g * factor),
                Mathf.Clamp01(b.b * factor),
                b.a
            );
            r.color = c;
        }
    }
}
