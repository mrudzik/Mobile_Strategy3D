using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopShopMenu_HUD : PopOutMenu_HUD
{

	/// <summary>
	/// This adds Creation Listener to every shop button.
	/// </summary>
	/// <param name="listener"></param>
    public void AddCreationListener(UnityAction<GameObject, int, string> listener)
	{// All this logic can be very slow

		// Find all buttons by tag
		var allButtons = GameObject.FindGameObjectsWithTag("ShopCreateButton");

		// Empty Check
		if (allButtons.Length == 0)
		{
			Debug.Log("Found No Buttons");
			return;
		}
		

		// Do for each this button
		foreach(var button in allButtons)
		{
			// Extracting script component from button
			var tempButtonScript = button.GetComponent<ModelDescribe_HUD>();
			// Adding Creation listener to this component
			tempButtonScript.AddListener(listener);
		}
	}
}
