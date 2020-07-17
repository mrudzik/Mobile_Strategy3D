using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
	void Awake()
	{
		if (!AudioManager.Initialized)
		{
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			AudioManager.Initialize(audioSource);
			DontDestroyOnLoad(gameObject);
		}
		else
		{// Duplicate gameobject, so destroy
			Destroy(gameObject);
		}

	}
}
