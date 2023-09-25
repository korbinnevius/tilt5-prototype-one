using Interaction;
using TiltFive;
using UnityEngine;
using Input = UnityEngine.Input;
using Player = Ship.Player;

namespace Character_Controller
{
	[RequireComponent(typeof(RBCharacterController))]
	public class CharacterControllerInput : MonoBehaviour
	{
		private RBCharacterController _characterController;
		private PlayerInteractionHandler _interactionHandler;
		private ControllerIndex _controllerIndex = ControllerIndex.Right;
		public PlayerIndex TiltPlayerIndex;
		public GameObject ControllerForward { get; set; }//forward in world space.
		public Vector3 WorldInputDirection { get; set; }

		public TiltFive.Input.WandButton interactButton;
		public TiltFive.Input.WandButton jumpButton;
		private float _trigger;
		[Range(0,1)]public float _triggerThreshold;
		private bool _triggerPressed;
		private Player _player;
		private void Awake()
		{
			_characterController = GetComponent<RBCharacterController>();
			_interactionHandler = GetComponent<PlayerInteractionHandler>();
		}

		public void SetPlayer(Player player)
		{
			_player = player;
		}

		private void Start()
		{
			TiltFive.Wand.TryCheckConnected(out var rightConnected, TiltPlayerIndex, ControllerIndex.Right);
			TiltFive.Wand.TryCheckConnected(out var leftConnected, TiltPlayerIndex, ControllerIndex.Left);

			if (rightConnected)
			{
				_controllerIndex = ControllerIndex.Right;
			}else if (leftConnected)
			{
				//left and not right. todo: just support both at the same time.
				_controllerIndex = ControllerIndex.Left;
			}
		}

		private void Update()
		{
			if (TiltPlayerIndex != PlayerIndex.None)
			{
				//todo remap to forward
				var tFiveInputv2 = TiltFive.Input.GetStickTilt(_controllerIndex, TiltPlayerIndex);
				var tFiveInput = new Vector3(tFiveInputv2.x, 0, tFiveInputv2.y);
				if (ControllerForward != null)
				{
					Vector3 forward = ControllerForward.transform.forward;
					forward = new Vector3(forward.x, 0, forward.z).normalized;
					Vector3 right = ControllerForward.transform.right;
					right = new Vector3(right.x, 0, right.z).normalized;

					tFiveInput = right * tFiveInputv2.x + forward * tFiveInputv2.y;
				}

				WorldInputDirection = tFiveInput;
				_characterController.Move(tFiveInput);
				
				//Interaction
				_trigger = TiltFive.Input.GetTrigger(_controllerIndex, TiltPlayerIndex);
				if (_trigger > _triggerThreshold)
				{
					if (!_triggerPressed)
					{
						_triggerPressed = true;
						_interactionHandler.Interact();
					}
				}
				else
				{
					_triggerPressed = false;
				}

				//jump
				if (TiltFive.Input.GetButtonDown(jumpButton, _controllerIndex, TiltPlayerIndex))
				{
					_characterController.Jump();
				}
				
				//alt
				if (TiltFive.Input.GetButtonDown(interactButton,_controllerIndex,TiltPlayerIndex))
				{
					_interactionHandler.Interact();
				}
			}
			else
			{
				_characterController.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
				if (Input.GetKeyDown(KeyCode.E))
				{
					_interactionHandler.Interact();
				}

				if (Input.GetKeyDown(KeyCode.Space))
				{
					_characterController.Jump();
				}
			}
		}
	}
}