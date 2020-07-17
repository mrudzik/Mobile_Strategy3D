using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopOutMenu_HUD : MonoBehaviour
{
	// This will notify all listeners about self Destruct when it happens
	// And execute external code
	// No need to know about someone else
	private UnityEvent _selfDestruct = new UnityEvent();

	public void SelfDestruct()
	{
		_selfDestruct.Invoke();
	}


	public void AddListener_SelfDestruct(UnityAction listener)
	{
		_selfDestruct.AddListener(listener);
	}
}
