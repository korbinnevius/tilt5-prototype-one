// using Timeline;
// using UnityEngine;
//
// namespace Info
// {
// 	public class StatusDisplay : MonoBehaviour
// 	{
// 		//Wrapper class for all children screens - is a state machine and data repo for screens.
//
// 		public GameTimeline Timeline;
// 		private InfoScreen _currentScreen;
//
// 		[Header("Info Screens")]
// 		[SerializeField] private InfoScreen shipEventsScreen;
// 		[SerializeField] private InfoScreen WaitingForPlayers;
//
// 		// [Header("States")]
// 		// [SerializeField] private State _gameplayState;
// 		// [SerializeField] private State _waitingForPlayersState;
// 		void Start()
// 		{
// 			//leave all states/init
// 			shipEventsScreen.DisableScreen();
// 			WaitingForPlayers.DisableScreen();
// 			
// 			//enable default screen
// 			EnableInfoScreen(shipEventsScreen);
// 		}
//
// 		private void OnEnable()
// 		{
// 			_waitingForPlayersState.OnEnterState += () => EnableInfoScreen(WaitingForPlayers);
// 			_gameplayState.OnEnterState += () => EnableInfoScreen(shipEventsScreen);
//
// 		}
//
// 		private void OnDisable()
// 		{
// 			//im not sure how to unsubscribe. copy pasted code doesn't feel right.
// 			_waitingForPlayersState.OnEnterState -= () => EnableInfoScreen(WaitingForPlayers);
// 			_gameplayState.OnEnterState -= () => EnableInfoScreen(shipEventsScreen);
//
// 		}
// 		
//
// 		private void EnableInfoScreen(InfoScreen infoScreen)
// 		{
// 			if (_currentScreen != null)
// 			{
// 				_currentScreen.DisableScreen();
// 			}
// 			infoScreen.EnableScreen(this);
// 			_currentScreen = infoScreen;
// 		}
// 	}
// 	
// 	
// }