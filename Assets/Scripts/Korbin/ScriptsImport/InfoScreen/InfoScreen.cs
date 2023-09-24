using UnityEngine;

namespace Info
{
	public class InfoScreen : MonoBehaviour
	{
		// protected StatusDisplay _statusDisplay;
		// public virtual void EnableScreen(StatusDisplay statusDisplay)
		// {
		// 	_statusDisplay = statusDisplay;
		// 	gameObject.SetActive(this);
		// }
		
		public virtual void DisableScreen()
		{
			gameObject.SetActive(false);
		}
	}
}