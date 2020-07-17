using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_HUD : MonoBehaviour
{
    public void GameplayButton()
	{
		MenuManager.GoToMenu(MenuNames.Gameplay);
	}

	public void QuitButton()
	{
		MenuManager.QuitGame();
	}
}
