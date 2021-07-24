//Copyright 2021 Andrew Young
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentalZone : MonoBehaviour
{
    [SerializeField]
    private Element m_element;

    private void OnEnable()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.LogFormat("Enter zone {0}", m_element.name);
        var playerController = collider.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Player.EnvironmentElementEntered(m_element);
            return;
        }
        Debug.Log("No player");
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.LogFormat("Exit zone {0}", m_element.name);
        var playerController = collider.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Player.EnvironmentElementExited(m_element);
            return;
        }
        Debug.Log("No player");
    }
}
