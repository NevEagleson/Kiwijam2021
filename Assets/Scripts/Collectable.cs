//Copyright 2021 Andrew Young
using System;
using UnityEngine;

public abstract class Collectable : ScriptableObject
{
    [SerializeField]
    private Sprite m_collectableSprite;
    [SerializeField]
    private Sprite m_uiSprite;
    [SerializeField]
    private Color m_color;

    public Sprite UISprite => m_uiSprite;
    public Sprite CollectableSprite => m_collectableSprite;
    public Color Color => m_color;

    public abstract event Action Spawn;

    public abstract void Collect(Player p);


}
