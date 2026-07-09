using UnityEngine;
using UnityEngine.UI; // <-- Required to talk to UI elements like Image!

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Connections")]
    [Tooltip("Drag your Player's Health component here")]
    public Health playerHealth; 

    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("UI Images")]
    [Tooltip("Drag Heart 1, 2, and 3 from the hierarchy into this array")]
    public Image[] heartImages;

    private void Start()
    {
        // Update the UI immediately when the game starts so it matches max health
        UpdateHearts();
    }

    // This method will be triggered by Unity Event
    public void UpdateHearts()
    {
        // Loop through all the UI images
        for (int i = 0; i < heartImages.Length; i++)
        {
            // If the image's index is less than the current health, show a full heart
            if (i < playerHealth.currentHealth)
            {
                heartImages[i].sprite = fullHeart;
            }
            // Otherwise, show an empty heart
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}
