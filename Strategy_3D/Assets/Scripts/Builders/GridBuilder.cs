using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
	[SerializeField] private GameObject _prefabGridTile;
	[SerializeField] private Transform _gridHolder;
	[SerializeField] private int _tileLineAmmount;
	[SerializeField] private float _tileSize;
	[SerializeField] private float _planeSize;


	// 2d List
	private List<List<GridTile>> _gridList = null;


	private void Start()
	{
		BuildGrid();
	}


	public void BuildGrid()
	{
		if (_gridList != null)
		{// If such grid already exists
		 // TODO: Destroy all objects inside
		 // Temporary protection
			Debug.Log("Trying to build on existing grid");
			return;
		}

		// Calculation variables

		// Edge Position in X and Z coordinates
		Vector2 startEdgePos = new Vector2(
			transform.position.x - (_planeSize / 2),
			transform.position.y - (_planeSize / 2));
		float tileShift = _planeSize / _tileLineAmmount;
		Vector2 startPos = new Vector2(
			startEdgePos.x + tileShift / 2,
			startEdgePos.y + tileShift / 2);


		// Create new grid

		// Creating X Coordinate Plane
		_gridList = new List<List<GridTile>>();
		for (int xNum = 0; xNum < _tileLineAmmount; xNum++)
		{// Moving on X coordinate
			// Creating Y Coordinate Plane for each X number
			_gridList.Add(new List<GridTile>());
			for (int yNum = 0; yNum < _tileLineAmmount; yNum++)
			{// Moving on Y coordinate
			 // Creating Tile for X and Y Coordinate
				GameObject tempGameObject = Instantiate(_prefabGridTile, _gridHolder);
				var tempObject = tempGameObject.GetComponent<GridTile>();

				// Moving Tile to corresponding position
				tempObject.transform.position = new Vector3(
					startPos.x + (xNum * tileShift),
					tempObject.transform.position.y,
					startPos.y + (yNum * tileShift));
				// Doing needed Tile Size
				tempObject.transform.localScale = new Vector3(_tileSize, 1, _tileSize);
				// Initializing Tile
				tempObject.InitializeTile(xNum, yNum);

				// Adding Tile to our grid List
				_gridList[xNum].Add(tempObject);
			}

		}

	}









	/// <summary>
	/// This will show closest grid position
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="resultGridPos"></param>
	public void FindClosestTile(Vector2 pos, out GridTile resultTile)
	//out Vector2 resultGridPos, 
	{
		float shortestDist = -1;
		//resultGridPos = Vector2.zero;
		resultTile = null;

		// Check all tiles
		for (int xNum = 0; xNum < _gridList.Count; xNum++)
		{
			for (int yNum = 0; yNum < _gridList[xNum].Count; yNum++)
			{
				// Getting Tile real position
				Vector3 tempPos3d = _gridList[xNum][yNum].transform.position;
				// Converting that real position to more comfortable for us 2d position
				Vector2 tempPos2d = new Vector2(tempPos3d.x, tempPos3d.z);

				// Calculating distance from our pos to tile
				var dist = (pos - tempPos2d).magnitude;

				if (shortestDist < 0 || shortestDist > dist)
				{// If we found more shorter distance to tile than previous
				 // Set that tile as our result
					shortestDist = dist;
				//	resultGridPos = tempPos2d;
					resultTile = _gridList[xNum][yNum];
				}
			}
		}
	}

	public void FindNearTiles(Vector2 gridPos, int size, ref List<GridTile> resultGrids)
	{
		int posX = (int)gridPos.x;
		int posY = (int)gridPos.y;


		try
		{ // It Should Catch an Exception when we try to add non existing Tile
			switch (size)
			{
				case 2: // 1 2
						// x 3

					// 1 = X && Y + 1
					InsideGridCheck(posX,		posY + 1,	ref resultGrids);
					// 2 = X + 1 && Y + 1
					InsideGridCheck(posX + 1,	posY + 1,	ref resultGrids);
					// 3 = X + 1 && Y
					InsideGridCheck(posX + 1,	posY,		ref resultGrids);
					break;
				case 3: // 1 2 3
						// 4 x 5
						// 6 7 8

					// 1 = X - 1	&& Y + 1
					InsideGridCheck(posX - 1,	posY + 1,	ref resultGrids);
					// 2 = X		&& Y + 1
					InsideGridCheck(posX,		posY + 1,	ref resultGrids);
					// 3 = X + 1	&& Y + 1
					InsideGridCheck(posX + 1,	posY + 1,	ref resultGrids);
					// 4 = X - 1	&& Y
					InsideGridCheck(posX - 1,	posY,		ref resultGrids);
					// 5 = X + 1	&& Y
					InsideGridCheck(posX + 1,	posY,		ref resultGrids);
					// 6 = X - 1	&& Y - 1
					InsideGridCheck(posX - 1,	posY - 1,	ref resultGrids);
					// 7 = X		&& Y - 1
					InsideGridCheck(posX,		posY - 1,	ref resultGrids);
					//8 = X + 1		&& Y - 1
					InsideGridCheck(posX + 1,	posY - 1,	ref resultGrids);
					break;
			}
		}
		catch
		{ // This will clear any results if error occurs
			resultGrids.Clear();
		}

		// If we Avoided Catch block than we are all Happy
	
		
	}

	private void InsideGridCheck(int x, int y, ref List<GridTile> resultGrids)
	{
		// Should Throw Exception if we miss
		resultGrids.Add(_gridList[x][y]);
		// But no worries we will catch it
	}






	#region Color

	public void PaintAllTilesToNormal()
	{
		for (int xNum = 0; xNum < _gridList.Count; xNum++)
		{
			for (int yNum = 0; yNum < _gridList[xNum].Count; yNum++)
			{
				_gridList[xNum][yNum].SetColorNormal();
			}
		}
	}

	public void PaintCurrentTilesOccupation(List<GridTile> tileList)
	{
		foreach (GridTile tile in tileList)
		{
			tile.SetColorOccupation();
		}		
	}


	#endregion

	public bool IsCurrentTilesOccupied(List<GridTile> currentTiles)
	{
		foreach (var tile in currentTiles)
		{
			if (tile.IsOccupied)
				return true;
		}
		return false;
	}

	public void OccupyTiles(List<GridTile> currentTiles, PlacedObject placedObj)
	{
		foreach (var tile in currentTiles)
		{
			tile.SetOccupant(placedObj);
		}
	}

}
