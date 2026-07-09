using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("Tutorial Panels")]
    public GameObject panel1; // Controls message
    public GameObject panel2; // Objective message
    public GameObject panel3; // Controls message

    private void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ShowNext()
    {
        panel1.SetActive(false);
        panel3.SetActive(true);
    }

    public void ShowObjective()
    {
        panel3.SetActive(false);
        panel2.SetActive(true);
    }

    public void CloseTutorial()
    {
        panel2.SetActive(false);
        Time.timeScale = 1f;
    }
}
