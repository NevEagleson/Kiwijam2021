//Copyright 2021 Andrew Young
using System;
using UnityEngine;

public class CollectableComponent : MonoBehaviour
{
    [SerializeField]
    private Collectable m_collectable;
    [SerializeField]
    private SpriteRenderer m_sprite;
    [SerializeField]
    private ParticleSystem m_particles;
    [SerializeField]
    private Collider2D m_trigger;
    [SerializeField]
    private AudioSource m_audio;

    private bool m_firstSpawn = true;
    private bool m_spawned = false;

    public void Spawn()
    {
        if (!m_spawned)
        {
            m_spawned = true;
            m_trigger.enabled = true;
            m_sprite.gameObject.SetActive(true);
            if (m_firstSpawn)
            {
                m_firstSpawn = false;
                return;
            }
            m_particles.Stop();
            m_particles.Play();
           
        }
    }
    public void Collect(Player player)
    {
        if (m_spawned)
        {
            m_trigger.enabled = false;
            m_sprite.gameObject.SetActive(false);
            m_particles.Stop();
            m_particles.Play();
            m_audio.PlayOneShot(m_audio.clip);
            m_collectable.Collect(player);
            m_spawned = false;
        }
    }

    void SetupCollectableAppearance()
    {
        m_sprite.sprite = m_collectable.CollectableSprite;
        var module = m_particles.main;
        var color = module.startColor;
        color.color = m_collectable.Color;
        module.startColor = color;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        m_collectable.Spawn += Spawn;
        SetupCollectableAppearance();
        Spawn();
    }
    private void OnDisable()
    {
        m_collectable.Spawn -= Spawn;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var playerController = collider.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Collect(playerController.Player);
            return;
        }
    }

#if UNITY_EDITOR
    private Collectable m_editorCollectable = null;
    private void OnValidate()
    {
        if (m_collectable != m_editorCollectable)
        {
            m_editorCollectable = m_collectable;
            SetupCollectableAppearance();          
        }
    }
#endif
}
