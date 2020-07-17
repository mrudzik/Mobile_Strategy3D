using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopBuildMenu_HUD : PopOutMenu_HUD
{

	UnityEvent _cancelBuildEvent = new UnityEvent();
	UnityEvent _confirmBuildEvent = new UnityEvent();


	#region Buttons

	public void CancelBuild_Button()
	{
		_cancelBuildEvent.Invoke();
		SelfDestruct();
	}

	public void ConfirmBuild_Button()
	{
		_confirmBuildEvent.Invoke();
		SelfDestruct();
	}

	#endregion


	#region Listeners

	public void AddListeners_CancelConfirm(UnityAction cancelListener, UnityAction confirmListener)
	{
		_cancelBuildEvent.AddListener(cancelListener);
		_confirmBuildEvent.AddListener(confirmListener);
	}

	#endregion


}
