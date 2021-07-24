//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class FaceUI : MonoBehaviour
{
    [SerializeField]
    private Image m_sprite;
    [SerializeField]
    private Player m_player;

    private Element m_element;

    public void SetElement(Element e)
    {
        if (e != m_element)
        {
            m_element = e;
            m_sprite.sprite = m_element.AdaptedFaceSprite;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        m_player.AdaptedElementChanged += SetElement;
        SetElement(m_player.AdaptedElement);
    }

    // Update is called once per frame
    void OnDisable()
    {
        m_player.AdaptedElementChanged -= SetElement;
    }
}
