using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;

    void Start()
    {
        if (PlayerPrefs.HasKey("BestTime"))
        {
            float best = PlayerPrefs.GetFloat("BestTime");
            int minutes = Mathf.FloorToInt(best / 60f);
            int seconds = Mathf.FloorToInt(best % 60f);
            bestTimeText.text = "Mejor tiempo: " + $"{minutes:00}:{seconds:00}";
        }
        else
        {
            bestTimeText.text = "";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
