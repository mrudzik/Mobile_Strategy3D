using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBuilder : MonoBehaviour
{
	[SerializeField] ObjectManager _objManager;
	[SerializeField] ScrollAndPitch _cameraController;


	// Update is called once per frame
	void LateUpdate()
    {
		// Required for proper clicking functionality of Builder Menu
		if (Gameplay_HUD.IsBuilderOn)
		{
#if UNITY_EDITOR
			
			if (Input.GetMouseButtonDown(0))
			{
				CatchPlaneMouseClick();
			}

#endif

#if UNITY_IOS || UNITY_ANDROID
			
			if (Input.touchCount >= 1)
			{
				CatchPlaneTouch();
			}

#endif
		}
#if UNITY_EDITOR
		if (!Gameplay_HUD.IsMenuOn)
		{
			
		}
#endif
	}


#if UNITY_EDITOR
	private void CatchPlaneMouseClick()
	{
		// Getting Mouse Position
		var mousePos = Input.mousePosition;
		//Debug.Log("Mouse Pos X:" + mousePos.x + " Y:" + mousePos.y);
		// Checking if Click position is to high
		// Hardcoded Check
		if (mousePos.y > 600) // 720px Y screen size - 120px Builder Menu Panel Height
		{// If to high dont move anything
			return;
		}


		// Shooting Ray to plane, to recieve plane position of click
		var ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out var hit))
		{
			var planePos = hit.point;
			//Debug.Log("Builder Raycast hit " + hit.collider.gameObject.name);
			//Debug.Log("Plane Pos X:" + planePos.x + " Z:" + planePos.z);
			_objManager.MoveBuildObject(new Vector2(planePos.x, planePos.z));
		}
		// _cameraController.PlanePosition(new Vector2(mousePos.x, mousePos.y));


		
	}

#endif

#if UNITY_IOS || UNITY_ANDROID
	private void CatchPlaneTouch()
	{
		// Getting Touch
		Touch touch1 = Input.GetTouch(0);

		// Checking if touch position is to high
		// Hardcoded Check
		if (touch1.position.y > 600) // 720px Y screen size - 120px Builder Menu Panel Height
		{// If to high dont move anything
			return;
		}

		// Shooting Ray to plane, to recieve plane position of touch
		var planePos = _cameraController.PlanePosition(touch1.position);
		_objManager.MoveBuildObject(new Vector2(planePos.x, planePos.z));
	}

#endif





}
