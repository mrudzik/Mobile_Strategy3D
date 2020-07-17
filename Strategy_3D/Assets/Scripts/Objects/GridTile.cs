using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridTile : MonoBehaviour
{

	#region Inspector_Info

	// 0 Empty Normal
	// 1 Empty in Builder Menu
	// 2 Occupied in Builder Menu
	// 4 Occupied Normal
	[SerializeField] private Material[] _materials;
	[Space]
	[SerializeField] private Renderer _renderer;


	#endregion


	// Grid Position
	private Vector2 _gridPos;
	public Vector2 GridPos { get => _gridPos; }


	private bool _isOccupied = false;
	public bool IsOccupied { get => _isOccupied; }

	// If its occupied we will have reference to occupied object
	private PlacedObject _objectOnGrid = null;
	

	private void Start()
	{
		if (_materials.Length != 4)
		{
			Debug.LogError("Wrong ammount of materials in Tile");
			return;
		}
		_renderer.enabled = true;
		_renderer.sharedMaterial = _materials[0];

	}

	bool _isInitialized = false;
	public void InitializeTile(int posX, int posY)
	{
		if (_isInitialized)
			return; // Protection

		_isInitialized = true;
		_gridPos = new Vector2(posX, posY);
	}

	#region Color
	
	/// <summary>
	/// Should be used to show occupation in normal state.
	/// Grey Colors would be fine
	/// </summary>
	public void SetColorNormal()
	{
		if (_isOccupied) // Shows occupied 
			_renderer.sharedMaterial = _materials[3];
		else
			_renderer.sharedMaterial = _materials[0];
	}
	
	/// <summary>
	/// Should be used to show occupation in Builder Menu.
	/// Bright colors would be fine
	/// </summary>
	public void SetColorOccupation()
	{
		if (_isOccupied)
			_renderer.sharedMaterial = _materials[2];
		else
			_renderer.sharedMaterial = _materials[1];
	}

	#endregion


	public void SetOccupant(PlacedObject occup)
	{
		if (occup == null)
		{// If Setting it to empty
			_objectOnGrid = null;
			_isOccupied = false;
		}
		else
		{// If Setting it to existing object
			_objectOnGrid = occup;
			_isOccupied = true;
		}
		

	}


}
