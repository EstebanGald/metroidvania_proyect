using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class BreakableBridge : MonoBehaviour
{
    [SerializeField] private Tilemap bridgeTilemap; // Reference to the bridge tilemap

    [Header("Bridge Timers")]
    [SerializeField] private float timeBeforeBreak = 4f;
    [SerializeField] private float respawnTime = 4f;
    [Header("Bridge Events")]
    // 2. This will show up in the Inspector so you can plug your animations in!
    public UnityEvent onBridgeStartBreaking; 
    public UnityEvent onBridgeBroken;
    public UnityEvent onBridgeRespawned;
    private bool isBreaking = false; // 3. Prevents the player from triggering the break twice
    private bool playerOnBridge = false; // Flag to track if the player is on the bridge
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isBreaking)
        {
            Debug.Log("Player is on the bridge. Start the animation!");
            playerOnBridge = true;
            isBreaking = true; // Lock it so it only fires once
            
            // 4. Shout to the Inspector that the breaking sequence has begun!
            onBridgeStartBreaking?.Invoke();
            //Start the coroutine to break the bridge after a delay
            StartCoroutine(BreakBridge());
        }
    }

    private IEnumerator BreakBridge()
    {
        yield return new WaitForSeconds(timeBeforeBreak); // Delay before breaking the bridge
        // 5. Shout to the Inspector that the bridge has been broken!
        onBridgeBroken?.Invoke();
        //Respawn logic
        yield return new WaitForSeconds(respawnTime); // Wait while the bridge is gone
        isBreaking = false;
        playerOnBridge = false;

        onBridgeRespawned?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
