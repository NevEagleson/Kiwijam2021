//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerActivatedButton : MonoBehaviour
{
    [SerializeField]
    private Graphic m_graphic;
    [SerializeField]
    private Color m_defaultColor;
    [SerializeField]
    private Color m_selectedColor;
    [SerializeField]
    private UnityEvent m_onActivated;
    [SerializeField]
    private AudioSource m_audio;
    [SerializeField]
    private AudioClip m_selectionClip;
    [SerializeField]
    private AudioClip m_activationClip;


    private bool m_selected = false;


    public void Select(bool select)
    {
        if (select == m_selected) return;
        m_selected = select;
        m_graphic.color = m_selected ? m_selectedColor : m_defaultColor;
        m_audio.PlayOneShot(m_selectionClip);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selected && Input.GetButtonDown("Jump"))
        {
            m_audio.PlayOneShot(m_activationClip);
            m_onActivated.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Select(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Select(false);
    }
}
