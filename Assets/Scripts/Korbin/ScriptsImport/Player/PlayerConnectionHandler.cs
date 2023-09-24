using System.Collections.Generic;
using TiltFive;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Ship
{
	public class PlayerConnectionHandler : MonoBehaviour
	{
		//Searches for input/connections from possible devices. When it finds one, it creates a new player.
		public GameObject PlayerPrefab;
		public Transform spawnLocation;

		private Dictionary<PlayerIndex, GameObject> _players = new Dictionary<PlayerIndex, GameObject>();

		public bool searchForMoreControllers;
		
		private void Awake()
		{
			_players = new Dictionary<PlayerIndex, GameObject>();
		}

		// Update is called once per frame
		void Update()
		{
			//really do this every frame? Should probably just react to input or a button press on 'any' controller.
			if (searchForMoreControllers)
			{
				for (int i = 1; i <= 4; i++)
				{
					var pindex = (PlayerIndex)i; //we can cast an int to an enum. Conveniently, one is 1, two is 2, etc. 
					if (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, pindex))
					{
						if (TiltFive.Player.IsConnected(pindex) && !_players.ContainsKey(pindex))
						{
							SpawnPlayer(pindex);
							//got all expected?
							if (i == 4)
							{
								searchForMoreControllers = false;
							}
						}
					}
				}

				if (Input.GetKeyDown(KeyCode.E) && !_players.ContainsKey(PlayerIndex.None))
				{
					SpawnPlayer(TiltFive.PlayerIndex.None);
				}
			}
		}

		void SpawnPlayer(PlayerIndex playerIndex)
		{
			if (_players.ContainsKey(playerIndex))
			{
				return;
			}
			
			var pla = Instantiate(PlayerPrefab, spawnLocation.position, spawnLocation.rotation);

			//configure input
			var player = pla.GetComponent<Player>();
			player.Input.ControllerForward = pla;

			//todo: yield here for WaitUntilWandConnected
			
			//The controller uses the camera to re-orient input from the axis to be relative.
			//We set it to one of the tracked objects of the wand in order to make it relative to literally how the controller is oriented.
			if (playerIndex != PlayerIndex.None)
			{
				//can be null
				player.Input.ControllerForward = TiltFiveManager2.Instance.allPlayerSettings[(int)playerIndex - 1].rightWandSettings.AimPoint;
				if (player.Input.ControllerForward == null)
				{
					player.Input.ControllerForward = new GameObject();
					player.Input.ControllerForward.name = "P" + playerIndex.ToString()+ " Aim";
					TiltFiveManager2.Instance.allPlayerSettings[(int)playerIndex - 1].rightWandSettings.AimPoint =
						player.Input.ControllerForward;
				}
			}

			if (TiltFive.Player.TryGetFriendlyName(playerIndex, out var friendlyName))
			{
				pla.name = "Player " + playerIndex.ToString()+ " - "+friendlyName;
			}

			//todo: configure visuals

			//add to dictionary
			_players.Add(playerIndex, pla);
			
			//todo: broadcast static action
		}
	}
} // using System.Collections.Generic;
