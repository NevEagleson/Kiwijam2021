//Copyright 2021 Andrew Young
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Element")]
public class Element : Collectable
{
    [SerializeField]
    private Texture2D m_adaptedPlayer;
    [SerializeField]
    private Sprite m_adaptedFace;
    [SerializeField]
    private Number m_numberCollected;

    public event Action<Element> Collected;
    public override event Action Spawn;

    public Texture2D AdaptedPlayerTexture => m_adaptedPlayer;
    public Sprite AdaptedFaceSprite => m_adaptedFace;
    public Number NumberCollected => m_numberCollected;

    public override void Collect(Player p)
    {
        NumberCollected.Value++;
        p.SetCollectionElement(this);
        Collected?.Invoke(this);
    }
    public override void DropAll()
    {
        NumberCollected.Value = 0;
        Spawn?.Invoke();
    }
}
