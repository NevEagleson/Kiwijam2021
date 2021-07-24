//Copyright 2021 Andrew Young
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Key")]
public class Key : Collectable
{
    [SerializeField]
    private Sprite m_uncollectedUISprite;

    public Sprite UncollectedUISprite => m_uncollectedUISprite;
    public bool Collected { get; private set; } = false;

    public event Action CollectedChanged;
    public override event Action Spawn;

    private void OnEnable()
    {
        Collected = false;
    }

    public override void Collect(Player p)
    {
        Collected = true;
        CollectedChanged?.Invoke();
    }
}
