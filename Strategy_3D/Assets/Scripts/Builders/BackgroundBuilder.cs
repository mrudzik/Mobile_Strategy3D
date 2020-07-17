using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBuilder : MonoBehaviour
{
	//[SerializeField] private GameObject _planePrefab;
	[SerializeField] private GameObject[] _decorativeModelPrefabs;

	[SerializeField] private float _innerSquare = 10;
	[SerializeField] private float _outerSquare = 20;

	[SerializeField] private Transform _decorativeHolder;

	[Tooltip("Spawn probability in percents. Type int from 0 to 100")]
	[SerializeField] private float _spawnProbability = 30;


	private List<GameObject> _decorativeList = null;

	private void Start()
	{
		BuildBackgroundDecorations();
	}

	private void BuildBackgroundDecorations()
	{
		if (_decorativeList != null)
		{
			// TODO: Destroy all objects in that list
			// Temporary protection
			Debug.Log("Trying to build on existing decorative background");
			return;
		}
		_decorativeList = new List<GameObject>();


		// The goal is to run thru all integer coords and randomly place decorative objects
		// Exception is inside inner square
		for (int xCoord = -((int)_outerSquare); xCoord < _outerSquare; xCoord++)
		{
			for (int yCoord = -((int)_outerSquare); yCoord < _outerSquare; yCoord++)
			{
				if (-_innerSquare < xCoord && xCoord < _innerSquare
					&& -_innerSquare < yCoord && yCoord < _innerSquare)
				{// We are inside Inner Square
					// Just Skipping to next coord
					continue;
				}
				if (Random.Range(0, 100) > _spawnProbability)
				{// Probability Check
					// Skipping
					continue;
				}

				float noiseX = Random.Range(-1.0f, 1.0f);
				float noiseY = Random.Range(-1.0f, 1.0f);
				// Creating Decorative Object
				GameObject tempObject = Instantiate(
					_decorativeModelPrefabs[Random.Range(0, _decorativeModelPrefabs.Length)],// Prefab
					new Vector3(xCoord + noiseX, 0.0f, yCoord + noiseY), // Position
					new Quaternion(),
					_decorativeHolder); // Holder
				// Rotating Object via Eulers
				tempObject.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
				// Scale Object
				tempObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

				_decorativeList.Add(tempObject);

			}
		}

		





	}
}
