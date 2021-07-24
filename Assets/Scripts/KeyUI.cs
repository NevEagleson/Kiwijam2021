//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Key m_key;

    void OnKeyCollected()
    {
        m_image.sprite = m_key.Collected ? m_key.UISprite : m_key.UncollectedUISprite;
    }


    void OnEnable()
    {
        m_key.CollectedChanged += OnKeyCollected;
        OnKeyCollected();
    }

    void OnDisable()
    {
        m_key.CollectedChanged -= OnKeyCollected;
    }
}
