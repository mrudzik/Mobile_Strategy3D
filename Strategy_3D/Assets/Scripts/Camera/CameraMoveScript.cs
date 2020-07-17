using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
	float _speed = -10;
	bool _leftMousePressed = false;
	public bool LeftMousePressed { get => _leftMousePressed; }

	[Space]
	public float _xBorderLeft = -12;
	public float _xBorderRight = 7;

	public float _zBorderRight = -12;
	public float _zBorderLeft = 7;


	[Space]
	public float _zoomMin = 2;
	public float _zoomMax = 5;

	// TODO: Optimize this value stuff
	private void Update()
	{
		CheckMouseState();

		if (Input.touchCount == 2)
		{// Touchpad Zoom Control

			// Getting Touches
			Touch touch_0 = Input.GetTouch(0);
			Touch touch_1 = Input.GetTouch(1);
			// Getting Touch Previous Positions
			Vector2 touch_0_PrevPos = touch_0.position - touch_0.deltaPosition;
			Vector2 touch_1_PrevPos = touch_1.position - touch_1.deltaPosition;

			float prevMagnitude = (touch_0_PrevPos - touch_1_PrevPos).magnitude;
			float currentMagnitude = (touch_0.position - touch_1.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;
			ZoomCamera(difference * 0.01f);
		}
		else if (LeftMousePressed)
		{// If Left Mouse pressed we doing movement stuff
			CameraMovement();
		}
		else
			ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
	}



	private void ZoomCamera(float increment)
	{
		if (increment == 0)
			return;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomMin, _zoomMax);
	}


	private void CameraMovement()
	{
		float mouseAxisX = 0;
		float mouseAxisY = 0;

	//	if (Input.touchCount == 1)
	//	{// If moving by touch
	//		Touch touch = Input.GetTouch(0);
	//		mouseAxisX = Mathf.Clamp(touch.deltaPosition.x, -1f, 1f);
	//		mouseAxisY = Mathf.Clamp(touch.deltaPosition.y, -1f, 1f);
	//	}
		//else
		//{// Detecting how far Mouse Moved from previous frame position X
			mouseAxisX = -Input.GetAxis("Mouse X");
			mouseAxisY = -Input.GetAxis("Mouse Y");
	//	}



		if (mouseAxisX != 0 || mouseAxisY != 0)
		{
			// Add to position
			float addX = mouseAxisX * Time.deltaTime * _speed;
			float addZ = mouseAxisY * Time.deltaTime * _speed;
			// New Calculated positions
			float newX = transform.localPosition.x + addX;
			float newY = transform.localPosition.y;
			float newZ = transform.localPosition.z + addZ;

			transform.localPosition = new Vector3(newX, newY, newZ);

			CheckCameraBorders();
		}
	}



	private void CheckMouseState()
	{
		// Detecting Left mouse pressed state
		if (Input.GetMouseButtonDown(0))
			_leftMousePressed = true;
		else if (Input.GetMouseButtonUp(0))
			_leftMousePressed = false;
	}
	private void CheckCameraBorders()
	{
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, _xBorderLeft, _xBorderRight),
			transform.position.y,
			Mathf.Clamp(transform.position.z, _zBorderRight, _zBorderLeft));
	}
}
