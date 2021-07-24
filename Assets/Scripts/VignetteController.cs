//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    [SerializeField]
    private Image m_vignette;
    [SerializeField]
    private AnimationCurve m_scaleCurve;
    [SerializeField]
    private AnimationCurve m_alphaCurve;
    [SerializeField]
    private AnimationCurve m_audioCurve;
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private float m_fadeTime = 0.2f;
    [SerializeField]
    private float m_maxValue = 9f;
    [SerializeField]
    private AudioSource m_music;

    private Color m_startColor;
    private Color m_targetColor;
    private float m_currentColor = 1f;
    private float m_startValue;
    private float m_targetValue;
    private float m_currentValue = 1f;
    private float m_currentAlpha = 0f;

    void OnEnable()
    {
        m_player.EnvironmentElementChanged += OnEnvironmentElementChanged;
        m_player.Health.ValueChanged += OnHealthChanged;

        m_startValue = m_player.Health.Value;
        m_targetValue = m_startValue;
        m_startColor = m_player.EnvironmentalElement.Color;
        m_targetColor = m_startColor;
        UpdateColor();
        UpdateScale();
    }

    void OnDisable()
    {
        m_player.EnvironmentElementChanged -= OnEnvironmentElementChanged;
        m_player.Health.ValueChanged -= OnHealthChanged;
    }

    void Update()
    {
        if (m_currentValue < 1f)
        {
            m_currentValue = Mathf.MoveTowards(m_currentValue, 1f, m_fadeTime * Time.deltaTime);
            UpdateScale();
        }
        if (m_currentColor < 1f)
        {
            m_currentColor = Mathf.MoveTowards(m_currentColor, 1f, m_fadeTime * Time.deltaTime);
            UpdateColor();
        }
    }

    void UpdateScale()
    {
        float value = Mathf.Lerp(m_startValue, m_targetValue, m_currentValue);
        float scale = m_scaleCurve.Evaluate(value);
        m_vignette.transform.localScale = new Vector3(scale, scale, scale);
        m_currentAlpha = m_alphaCurve.Evaluate(value);
        Color c = m_vignette.color;
        c.a = m_currentAlpha;
        m_vignette.color = c;
        if (value > 8.99f)
        {
            m_vignette.enabled = false;
        }
        else
        {
            m_vignette.enabled = true;
        }
        float vol = m_audioCurve.Evaluate(value);
        m_music.volume = vol;
    }
    void UpdateColor()
    {
        Color c = Color.Lerp(m_startColor, m_targetColor, m_currentColor);
        c.a = m_currentAlpha;
        m_vignette.color = c;
    }

    void OnEnvironmentElementChanged(Element e)
    {
        m_startColor = m_vignette.color;
        m_targetColor = e.Color;
        m_currentColor = 0f;
    }

    void OnHealthChanged(int value)
    {
        m_startValue = m_targetValue;
        m_targetValue = value;
        m_currentValue = 0f;
    }
}
