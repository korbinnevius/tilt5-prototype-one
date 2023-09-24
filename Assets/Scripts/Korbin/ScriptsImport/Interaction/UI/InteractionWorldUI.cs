using TMPro;
using UnityEngine;

namespace Interaction.UI
{
	public class InteractionWorldUI : MonoBehaviour
	{
		private Interaction Interaction => _interactionHandler.CurrentInteraction;
		[SerializeField] private PlayerInteractionHandler _interactionHandler;
		[SerializeField] private TMP_Text _text;
		[SerializeField] private Canvas _UIRenderCanvas;
		[SerializeField] private Vector3 offset;
		private Transform _playerCamera;

		private void Start()
		{
			var index = Mathf.Max((int)_interactionHandler.Player.PlayerIndex - 1,0);
			
			var cam = TiltFive.TiltFiveManager2.Instance.allPlayerSettings[index].glassesSettings.cameraTemplate;
			if (cam != null)
			{
				_playerCamera = cam.transform;
			}
			else
			{
				//set to unity camera.
				var sceneCam = GameObject.FindObjectOfType<Camera>();
				if (sceneCam != null)
				{
					_playerCamera = sceneCam.transform;
				}
			}
		}

		public void Update()
		{
			bool active = Interaction.Interact != null;
			_UIRenderCanvas.gameObject.SetActive(active);
			
			if (active)
			{
				_UIRenderCanvas.transform.position = Interaction.Interest.GetWorldUIPosition();
				_text.color = _interactionHandler.Player.playerUIColor;
				if (_playerCamera != null)
				{
					_UIRenderCanvas.transform.rotation = Quaternion.LookRotation(_text.transform.position-_playerCamera.position, Vector3.up);
				}
				if (Interaction.Verb == "")
				{
					_text.text = "Interact";
				}
				else
				{
					_text.text = Interaction.Verb;
				}
			}
		}
	}
}