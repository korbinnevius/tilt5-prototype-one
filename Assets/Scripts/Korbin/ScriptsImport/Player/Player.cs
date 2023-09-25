using Character_Controller;
using Interaction;
using TiltFive;
using UnityEngine;

namespace Ship
{
	public class Player : MonoBehaviour
	{
		public Color playerUIColor;

		private RBCharacterController _characterController;
		private PlayerInteractionHandler _playerInteractionHandler;
		public CharacterControllerInput Input => _input;
		public PlayerIndex PlayerIndex => Input.TiltPlayerIndex;//should we own this?

		private CharacterControllerInput _input;

		private void Awake()
		{
			_playerInteractionHandler = GetComponent<PlayerInteractionHandler>();
			_characterController = GetComponent<RBCharacterController>();
			_input = GetComponent<CharacterControllerInput>();

			if (_playerInteractionHandler != null)
			{
				_playerInteractionHandler.SetPlayer(this);
			}
			
			_input.SetPlayer(this);
		}
		
	}
}