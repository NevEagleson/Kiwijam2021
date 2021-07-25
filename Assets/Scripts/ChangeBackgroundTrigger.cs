//Copyright 2021 Andrew Young
using UnityEngine;

public class ChangeBackgroundTrigger : MonoBehaviour
{
    [SerializeField]
    private Sprite m_bgSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Background.SetBackground(m_bgSprite);
    }
}
