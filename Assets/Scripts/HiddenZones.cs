using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenZones : MonoBehaviour
{
    [SerializeField] private Tilemap hiddenTilemap; // Reference to the hidden tilemap
    private bool playerInZone = false; // Flag to track if the player is in the zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            // Hide tilemap when the player enters the zone
            hiddenTilemap.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            // Reveal tilemap when the player exits the zone
            hiddenTilemap.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
