//Copyright 2021 Andrew Young
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private GameObject m_door;
    [SerializeField]
    private Key m_key;

    public void UpdateDoor()
    {
        m_door.SetActive(!m_key.Collected);
    }

    void OnEnable()
    {
        m_key.CollectedChanged += UpdateDoor;
        UpdateDoor();
    }

    void OnDisable()
    {
        m_key.CollectedChanged -= UpdateDoor;
    }
}
