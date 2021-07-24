//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGoal : MonoBehaviour
{
    [SerializeField]
    private string m_nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(m_nextScene);
    }
}
