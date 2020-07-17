using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_HUD : MonoBehaviour
{

	#region ShowHide_Grid

	[SerializeField] private Transform _gridHolder;
	
    public void ShowHideGrid()
	{
		_gridHolder.gameObject.SetActive(!_gridHolder.gameObject.activeSelf);
	}

	#endregion


	#region PopOut_Menus
	[Space]
	[SerializeField] private Transform _buttonHolder;
	[SerializeField] private Transform _popMenuHolder;

	[SerializeField] private GameObject _prefabShopMenu;
	[SerializeField] private GameObject _prefabPauseMenu;
	[SerializeField] private GameObject _prefabBuildMenu;
	
	private GameObject _popOutMenu = null;

	private static bool _isMenuOn = false;
	private static bool _isBuilderOn = false;
	public static bool IsMenuOn { get => _isMenuOn; }
	public static bool IsBuilderOn { get => _isBuilderOn; }


	[Space]
	// Object Manager
	[SerializeField] private ObjectManager _objManager;


	#region Start

	private void Start()
	{
		// Just resetting static values to default
		_isMenuOn = false;
		_isBuilderOn = false;
	}

	#endregion


	#region CreateMenu_Buttons
	/// <summary>
	/// Creates Shop Menu
	/// </summary>
	public void ShopMenuButton()
	{
		if (_popOutMenu != null)
		{
			Debug.Log("Pop Menu is already created");
			return;
		}

		CreatePopOutPreparations(_prefabShopMenu);

		// Getting shop menu component cuz it holding all our required code
		var shopMenu = _popOutMenu.GetComponent<PopShopMenu_HUD>();
		// Adding Listener that will create needed object from our button selection
		shopMenu.AddCreationListener(ShopMenuCreateItem_EventHandler);
	}

	public void PauseMenuButton()
	{
		if (_popOutMenu != null)
		{
			Debug.Log("Pop Menu is already created");
			return;
		}

		CreatePopOutPreparations(_prefabPauseMenu);
	}

	#endregion

	private void BuildMenuCreation(GameObject prefab, int size, string name)
	{
		// Standart Pop Menu Creation
		if (_popOutMenu != null)
		{
			Debug.Log("Pop Menu is already created");
			return;
		}
		CreatePopOutPreparations(_prefabBuildMenu);
		// Marking that working builder menu
		_isBuilderOn = true;
		// Showing Grid if hidden
		_gridHolder.gameObject.SetActive(true);
		// Extracting required script component
		var buildMenu = _popOutMenu.GetComponent<PopBuildMenu_HUD>();

		
		// Creating Object
		_objManager.CreateObject(prefab, size, name);
		// Inserting Object Manager Listeners
		_objManager.InsertListenersIn(buildMenu);
	}









	/// <summary>
	/// Prepares for PopOut menu creation
	/// </summary>
	private void CreatePopOutPreparations(GameObject prefabMenu)
	{
		// Hide Main Gameplay Buttons
		_buttonHolder.gameObject.SetActive(false);
		// Stop Time
		Time.timeScale = 0f;
		// This should prevent any camera from movement
		_isMenuOn = true;


		// Creating shop menu from prefab
		_popOutMenu = Instantiate(prefabMenu, _popMenuHolder);
		// Recieving HUD Component
		var shopMenuHUD = _popOutMenu.GetComponent<PopOutMenu_HUD>();
		// When HUD invokes selfDestruct event, we executing this code
		shopMenuHUD.AddListener_SelfDestruct(PopMenuClose_EventHandler);

		AudioManager.Play(AudioClipName.MenuClick);
	}





	#region Listeners


	

	private void ShopMenuCreateItem_EventHandler(GameObject prefab, int size, string name)
	{
		// Closing Menu
		PopMenuClose_EventHandler();
		// Create Build Menu
		BuildMenuCreation(prefab, size, name);
	}

	/// <summary>
	/// Removes any signs of PopOut menu
	/// </summary>
	private void PopMenuClose_EventHandler()
	{
		// Show Main Gameplay Buttons
		_buttonHolder.gameObject.SetActive(true);

		// Destroing and erasing that menu
		Destroy(_popOutMenu);
		_popOutMenu = null;
		// Resume Time
		Time.timeScale = 1f;
		// This should prevent any camera from movement
		_isMenuOn = false;
		// Returning to standart state
		_isBuilderOn = false;
	}


	#endregion

	#endregion




}
