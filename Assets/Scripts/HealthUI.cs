//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Image m_heartFill;
    [SerializeField]
    private Animator m_animation;
    [SerializeField]
    private Player m_player;

    void OnHealthChanged(int value)
    {
        m_heartFill.fillAmount = value / (float)m_player.Health.InitialValue;
        m_animation.SetTrigger("beat");
    }

    void OnHealthReset()
    {
        m_heartFill.fillAmount = 1f;
        m_animation.ResetTrigger("beat");
        m_animation.SetTrigger("reset");
    }

    void OnPlayerSafeChanged(bool safe)
    {
        m_animation.SetBool("safe", safe);
    }

    void OnEnable()
    {
        m_player.Health.ValueChanged += OnHealthChanged;
        m_player.SafeChanged += OnPlayerSafeChanged;
        m_heartFill.fillAmount = m_player.Health.Value / (float)m_player.Health.InitialValue;
    }

    void OnDisable()
    {
        m_player.Health.ValueChanged -= OnHealthChanged;
        m_player.SafeChanged -= OnPlayerSafeChanged;
    }
}
