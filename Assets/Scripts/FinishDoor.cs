using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDoor : MonoBehaviour
{
    public GameObject winPanel;
    public float delayBeforeMenu = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        if (player.keysCollected >= player.keysRequired)
        {
            winPanel.SetActive(true);
            Invoke(nameof(ReturnToMenu), delayBeforeMenu);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
