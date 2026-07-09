using UnityEngine;
public class AbilityNotifier : MonoBehaviour
{
    public GameObject vineGrowPanel;
    public GameObject fireballPanel;
    public GameObject doubleJumpPanel;

    public void ShowVineGrow()
    {
        vineGrowPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowFireball()
    {
        fireballPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowDoubleJump()
    {
        doubleJumpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
