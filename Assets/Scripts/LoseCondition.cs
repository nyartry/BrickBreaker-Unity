using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoseCondition : MonoBehaviour
{
    private BricksManager BricksManager;
    private AudioManager audioManager;

	public UnityEvent onBallEnter;
	
	private void Start()
    {
        BricksManager = GameObject.FindObjectOfType<BricksManager>();
        audioManager = AudioManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If while waiting for transition to next level, don't check lose condition
        if (BricksManager.RemainBricks() > 0)
        {
            audioManager.PlayFailingAudio();
			onBallEnter?.Invoke();
        }
    }
}
