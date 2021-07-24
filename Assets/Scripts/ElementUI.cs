//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour
{
    [SerializeField]
    private Image m_sprite;
    [SerializeField]
    private SpriteNumber m_collectedNumber;
    [SerializeField]
    private Player m_player;

    private Element m_element;

    public void SetElement(Element e)
    {
        if (e != m_element)
        {
            m_element = e;
            m_sprite.sprite = m_element.UISprite;
            m_collectedNumber.SetNumber(m_element.NumberCollected);
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        m_player.CollectionElementChanged += SetElement;
        SetElement(m_player.CollectionElement);
    }

    // Update is called once per frame
    void OnDisable()
    {
        m_player.CollectionElementChanged -= SetElement;
    }
}
