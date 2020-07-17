using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used to create and manage objects on grid
/// </summary>
public class ObjectManager : MonoBehaviour
{
	[SerializeField] private Transform _objectHolder;
	[SerializeField] private GridBuilder _gridManager;

	private List<PlacedObject> _placedObjects = new List<PlacedObject>();



	private GameObject _buildObject = null;
	private int _buildSize = 0;
	private string _objName = "";

	List<GridTile> _occupiedTiles = new List<GridTile>();

	private bool _canBuild = false;

	#region CreateDestroy_BuildObject

	/// <summary>
	/// This will create building object
	/// </summary>
	/// <param name="prefab"></param>
	/// <param name="size"></param>
	public void CreateObject(GameObject prefab, int size, string name)
	{
		if (_buildObject != null)
		{
			Debug.Log("Build Object is already existing. Something is wrong!!");
			return;
		}
		// Size Check
		if (size < 1 || size > 3)
		{
			Debug.Log("Wrong Size");
			return;
		}
		
		AudioManager.Play(AudioClipName.ObjectSelected);

		_buildObject = Instantiate(prefab, _objectHolder);
		_buildSize = size;
		_objName = name;

		// This will move build object to closest tile
		MoveBuildObject(Vector2.zero);
	}

	/// <summary>
	/// This will erase building object
	/// </summary>
	private void KillBuildObject()
	{
		Destroy(_buildObject);
		_buildObject = null;
	}

	#endregion


	public void MoveBuildObject(Vector2 aproxNewPos)
	{
		// Find Closest grid
		Vector2 resultPos; // Position where to place object
		GridTile resultGrid;
		_gridManager.FindClosestTile(aproxNewPos, out resultGrid);
		
		
		// Temporary Occupied Tiles
		List<GridTile> occupiedTiles = new List<GridTile>();
		// Adding Selected Tile
		occupiedTiles.Add(resultGrid);
		if (_buildSize > 1)
		{
			// Adding Near Tiles
			_gridManager.FindNearTiles(resultGrid.GridPos, _buildSize, ref occupiedTiles);
			if (occupiedTiles.Count == 0)
			{	// Just dont move it
				return;
			}
		}


		// Clearing occupied Tiles list from previous tiles, from last position
		_occupiedTiles.Clear();
		_occupiedTiles = occupiedTiles;


		// Check thoose tiles for occupation
		_canBuild = true;
		if (_gridManager.IsCurrentTilesOccupied(_occupiedTiles))
		{
			_canBuild = false;
		}


		// Paint all Tiles to Normal
		_gridManager.PaintAllTilesToNormal();
		// Painting thoose tiles for their occupation
		_gridManager.PaintCurrentTilesOccupation(_occupiedTiles);


	
		// Calculate resulting position
		resultPos = new Vector2(resultGrid.transform.position.x, resultGrid.transform.position.z);
		if (_buildSize == 2)
		{
			// current position add half delta
			var tempPos = resultGrid.transform.position - ((resultGrid.transform.position - _occupiedTiles[2].transform.position) / 2);
			resultPos = new Vector2(tempPos.x, tempPos.z);
		}

		// Moving Build Object
		_buildObject.transform.position = new Vector3(
			resultPos.x,
			_buildObject.transform.position.y,
			resultPos.y);
	}

	private int GenerateUniqueID()
	{
		bool loop = true;
		int result = -1;

		while (loop)
		{
			result = Random.Range(1, 2147483647);
			loop = false;
			foreach (PlacedObject placedObj in _placedObjects)
			{ // Check every placed object if they have same ID
				if (placedObj.UniqueID == result)
				{
					loop = true;
				}
			}
		}

		return result;
	}


	#region EventHandlers

	/// <summary>
	/// This will Erase object that we were using in build menu
	/// </summary>
	private void CancelBuild_EventHandler()
	{
		AudioManager.Play(AudioClipName.BuilderCancel);

		KillBuildObject();

		// Paint all Tiles to Normal Color
		_gridManager.PaintAllTilesToNormal();
	}

	/// <summary>
	/// This will Leave Object in build menu
	/// </summary>
	private void ConfirmBuild_EventHandler()
	{
		// Check if Can Build
		if (!_canBuild)
		{// If we cant build object here, then we just cancel this build
			CancelBuild_EventHandler();
			return;
		}

		AudioManager.Play(AudioClipName.BuilderConfirm);

		// Adding to build object PlacedObject component
		_buildObject.AddComponent<PlacedObject>();
		// Initializing PlacedObject component
		var tempPlacedObj = _buildObject.GetComponent<PlacedObject>();
		tempPlacedObj.Initialize(_occupiedTiles[0].GridPos, _occupiedTiles, _buildSize, _objName, GenerateUniqueID());
		tempPlacedObj.ObjectInfo();

		// Move _buildObject to list of existing objects
		_placedObjects.Add(tempPlacedObj);
		// Mark Corresponding Grid tiles
		_gridManager.OccupyTiles(_occupiedTiles, tempPlacedObj);




		// Remove this object from to build slot
		_buildObject = null;
		// Paint all Tiles to Normal Color
		_gridManager.PaintAllTilesToNormal();
	}


	/// <summary>
	/// Inserts needed Listeners in Build Menu
	/// </summary>
	/// <param name="buildMenuScript"></param>
	public void InsertListenersIn(PopBuildMenu_HUD buildMenuScript)
	{
		// Insert Cancel Button listener which will delete spawned object
		// Insert Confirm Button listener which will add spawned object to list of existing objects
		// and mark occupied grid tiles
		buildMenuScript.AddListeners_CancelConfirm(CancelBuild_EventHandler, ConfirmBuild_EventHandler);
	}

	#endregion

}
