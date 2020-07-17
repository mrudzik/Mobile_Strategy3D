using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public enum MenuNames
{
	MainMenu,
	Gameplay,
	PauseMenu,
	ShopMenu
}

public static class MenuManager
{

	public static void GoToMenu(MenuNames menuName)
	{
		switch (menuName)
		{
			case MenuNames.MainMenu:
				SceneManager.LoadScene("MainMenu");
				break;
			case MenuNames.Gameplay:
				SceneManager.LoadScene("Gameplay");
				break;
			case MenuNames.PauseMenu:
				//Object.Instantiate(Resources.Load("PauseMenu"));
				break;
			case MenuNames.ShopMenu:
				//Object.Instantiate(Resources.Load("ShopMenu"));
				break;
		}
	}

	public static void QuitGame()
	{
		Application.Quit();
	}

}
