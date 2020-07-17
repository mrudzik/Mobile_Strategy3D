using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{

	private Vector2 _gridPos;
	
	
	private string _objName = "";
	private int _uniqueID = -1;
	public int UniqueID { get => _uniqueID; }

	private int _gridSize = -1;

	private List<GridTile> _occupiedTiles = new List<GridTile>();
	


	bool _isInitialized = false;
    public void Initialize(Vector2 gridPos, List<GridTile> occupiedTiles, int size, string name, int uniqueID)
	{
		if (_isInitialized)
			return; // Protection from double initialization
		_isInitialized = true;

		// Initializing
		_gridPos = gridPos;
		_occupiedTiles = occupiedTiles;
		_gridSize = size;
		_objName = name;
		_uniqueID = uniqueID;

	
	}

	public void ObjectInfo()
	{
		Debug.Log($"Unique ID : {_uniqueID} | Name : {_objName} | Grid Size : {_gridSize}x{_gridSize}");
	}

	private void OnMouseDown()
	{
		ObjectInfo();
	}

}
