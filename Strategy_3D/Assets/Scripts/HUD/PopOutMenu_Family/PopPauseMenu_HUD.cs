using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopPauseMenu_HUD : PopOutMenu_HUD
{
    public void Quit_Button()
	{
		MenuManager.GoToMenu(MenuNames.MainMenu);
	}
}
