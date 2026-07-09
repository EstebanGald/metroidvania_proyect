using UnityEngine;
using TMPro; 

public class KeyUI : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player;
    public TextMeshProUGUI keyText;

    private void Start()
    {
        UpdateKeyText();
    }

    public void UpdateKeyText()
    {
        keyText.text = $"Keys: {player.keysCollected}/{player.keysRequired}";
    }
}
