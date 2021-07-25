//Copyright 2021 Andrew Young
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_background;

    private static Background s_instance;

    // Start is called before the first frame update
    void Awake()
    {
        s_instance = this;
    }

    public static void SetBackground(Sprite s)
    {
        s_instance.m_background.sprite = s;
    }
}
