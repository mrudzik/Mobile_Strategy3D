using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ModelDescribe_HUD : MonoBehaviour
{
	// Prefab
	[Tooltip("Insert your model prefab")]
	[SerializeField] private GameObject _modelPrefab = null;
	// Picture
	[Tooltip("Insert your prepared image for your model")]
	[SerializeField] private Sprite _modelSprite = null;
	// Text
	[Tooltip("Insert your model name")]
	[SerializeField] private string _modelName = "Model";
	// Size
	[Tooltip("Insert your model size that it will ocupate")]
	[SerializeField] private int _modelSize = 1;


	[Space]
	[SerializeField] private Image _shopImage;
	[SerializeField] private Text _shopName;
	[SerializeField] private Text _shopSize;


	// When clicking on button this event is invoked
	UnityEvent<GameObject, int, string> _createEvent = new CreateObjectEvent();


	private bool _isCorrect = false;

	private void Start()
	{
		// Correction Check
		if (_modelPrefab == null
			|| _modelSprite == null)
		{
			Debug.Log("Shop Button is not correct");
			return;
		}
		// TODO: Add Size Check


		// Confirming that this button is correct
		_isCorrect = true;

		// Setting visuals
		
		_shopImage.sprite = _modelSprite;
		_shopName.text = _modelName;
		// Using string interpolation
		_shopSize.text = $"Size: {_modelSize}x{_modelSize}";
	}



	#region EventStuff

	public void CreateThisObject()
	{
		// Protection
		if (!_isCorrect)
			return;

		Debug.Log("Creating Selected Object");
		_createEvent.Invoke(_modelPrefab, _modelSize, _modelName);
	}

	public void AddListener(UnityAction<GameObject, int, string> listener)
	{
		_createEvent.AddListener(listener);
	}


	#endregion

}
