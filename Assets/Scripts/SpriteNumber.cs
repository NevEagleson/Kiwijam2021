//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class SpriteNumber : MonoBehaviour
{
    [SerializeField]
    private Image m_image = default;
    [SerializeField]
    private Sprite[] m_numberSprites = default;
    [SerializeField]
    private Number m_number = default;

    public void SetNumber(Number number)
    {
        if (m_number != null)
        {
            m_number.ValueChanged -= OnValueChanged;
        }
        m_number = number;
        if (m_number != null)
        {
            m_number.ValueChanged += OnValueChanged;
            OnValueChanged(m_number.Value);
        }
        else
        {
            OnValueChanged(0);
        }
        
    }

    void OnValueChanged(int value)
    {
        if (m_numberSprites == null || m_numberSprites.Length == 0) return;
        Mathf.Clamp(value, 0, m_numberSprites.Length - 1);
        m_image.sprite = m_numberSprites[value];
    }

    void OnEnable()
    {
        if (m_number != null)
        {
            m_number.ValueChanged += OnValueChanged;
            OnValueChanged(m_number.Value);
        }
        else
        {
            OnValueChanged(0);
        }
    }
    private void OnDisable()
    {
        if (m_number != null)
        {
            m_number.ValueChanged -= OnValueChanged;
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        m_image = GetComponent<Image>();
    }
    private void OnValidate()
    {
        if (m_image != null && m_number != null)
        {
            OnValueChanged(m_number.Value);
        }
    }
#endif
}
