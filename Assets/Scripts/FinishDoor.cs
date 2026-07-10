using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishDoor : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI timeText;
    public float delayBeforeMenu = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        if (player.keysCollected >= player.keysRequired)
        {
            float elapsed = Time.timeSinceLevelLoad;
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            string timeString = $"{minutes:00}:{seconds:00}";

            timeText.text = "Tiempo: " + timeString;

            float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
            if (elapsed < bestTime)
            {
                PlayerPrefs.SetFloat("BestTime", elapsed);
                PlayerPrefs.Save();
                timeText.text += "\n¡Nuevo récord!";
            }

            winPanel.SetActive(true);
            Invoke(nameof(ReturnToMenu), delayBeforeMenu);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
