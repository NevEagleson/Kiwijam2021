//Copyright 2021 Andrew Young
using UnityEngine;

public class ResetAll : MonoBehaviour
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private Collectable[] m_collectables;

    // Start is called before the first frame update
    void Start()
    {
        m_player.Respawn();
        foreach (Collectable c in m_collectables)
        {
            c.DropAll();
        }
    }

}
