using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    private static List<Checkpoint> allCheckpoints = new List<Checkpoint>();

    private Animator anim;
    private bool activated = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        allCheckpoints.Add(this);
    }

    private void OnDisable()
    {
        allCheckpoints.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activated)
        {
            // Reset all other checkpoints
            foreach (Checkpoint cp in allCheckpoints)
            {
                if (cp != this && cp.activated)
                    cp.ResetCheckpoint();
            }

            // Activate this one
            activated = true;
            anim.SetBool("isActivated", true);
            RespawnManager.instance.UpdateCheckpoint(transform.position);
        }
    }

    public void ResetCheckpoint()
    {
        activated = false;
        anim.SetBool("isActivated", false);
    }
}