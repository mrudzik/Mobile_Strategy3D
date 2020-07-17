using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPitch : MonoBehaviour
{


	public Camera camera;
	// Not a mesh its only object for calculations
	protected Plane plane;

	private void Awake()
	{
		if (camera == null)
			camera = Camera.main;
	}

#if UNITY_IOS || UNITY_ANDROID

	private void LateUpdate()
	{
		if (!Gameplay_HUD.IsMenuOn)
			MoveCameraByTouch();
		
			
	}





	private void MoveCameraByTouch()
	{
		// Update Plane
		if (Input.touchCount >= 1)
		{
			plane.SetNormalAndPosition(transform.up, transform.position);
		}

		var Delta1 = Vector3.zero;
		var Delta2 = Vector3.zero;

		// Scroll
		if (Input.touchCount >= 1)
		{
			Touch touch1 = Input.GetTouch(0);
			Delta1 = PlanePositionDelta(touch1);
			// If there was a move
			if (touch1.phase == TouchPhase.Moved)
			{// Move Camera
				camera.transform.Translate(Delta1, Space.World);
				CheckCameraBorders();
			}
		}


		// Pitch
		if (Input.touchCount >= 2)
		{
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);

			// Starting two finger positions
			var pos1Now = PlanePosition(touch1.position);
			var pos2Now = PlanePosition(touch2.position);
			// Previous two finger positions
			var pos1Before = PlanePosition(touch1.position - touch1.deltaPosition);
			var pos2Before = PlanePosition(touch2.position - touch2.deltaPosition);

			// Calculate Zoom
			var zoom = Vector3.Distance(pos1Now, pos2Now) / Vector3.Distance(pos1Before, pos2Before);
			// Zoom Edge Cases Protection
			if (zoom == 0 || zoom > 10)
				return;

			// Move Camera amount the mid ray
			camera.transform.position = Vector3.LerpUnclamped(pos1Now, camera.transform.position, 1 / zoom);
			CheckCameraBorders();
		}
	}

#endif

	/// <summary>
	/// Gives you delta how far you moved from previous plane position
	/// touch Just finger data
	/// </summary>
	/// <param name="touch"></param>
	/// <returns></returns>

	protected Vector3 PlanePositionDelta(Touch touch)
	{
		// Not Moved from previous frame
		if (touch.phase != TouchPhase.Moved)
			return Vector3.zero;

		// Delta
		var rayBefore = camera.ScreenPointToRay(touch.position - touch.deltaPosition);
		var rayNow = camera.ScreenPointToRay(touch.position);
		if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
			return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

		// Not on Plane
		return Vector3.zero;
	}



	/// <summary>
	/// Gives you Coordinates where on plane you have touched
	/// screenPos - finger position on screen
	/// </summary>
	/// <param name="screenPos"></param>
	/// <returns></returns>
	public Vector3 PlanePosition(Vector2 screenPos)
	{
		// Position
		var rayNow = camera.ScreenPointToRay(screenPos);
		if (plane.Raycast(rayNow, out var enterNow))
			return rayNow.GetPoint(enterNow); // Distance to ray and plane collision

		return Vector3.zero;
	}



	[Space]
	public float _xBorderBackLeft = -15;
	public float _xBorderForwardRight = 5;

	public float _zBorderBackRight = -15;
	public float _zBorderForwardLeft = 5;

	public float _yBorderUp = 6;
	public float _yBorderDown = 1;

	/// <summary>
	/// Checks Camera Global position
	/// </summary>
	private void CheckCameraBorders()
	{
		camera.transform.position = new Vector3(
			Mathf.Clamp(camera.transform.position.x, _xBorderBackLeft, _xBorderForwardRight),
			Mathf.Clamp(camera.transform.position.y, _yBorderDown, _yBorderUp),
			Mathf.Clamp(camera.transform.position.z, _zBorderBackRight, _zBorderForwardLeft));
	}


}
