//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.Events;


public class TriggerZone : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onTriggerEntered;
    [SerializeField]
    private UnityEvent m_onTriggerExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_onTriggerEntered.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_onTriggerExited.Invoke();
    }
}
