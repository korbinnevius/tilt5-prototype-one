using Info.UI;
using Timeline;
using UnityEngine;

namespace Info
{
	public class ShipMessagesInfoScreen : InfoScreen
	{
		private MessageSystem _messageSystem;
		private MessageInfo _mostRecentMessage;
		[SerializeField] private int messagesToDisplay = 10;
		[SerializeField] private GameObject _messagePrefab;
		[SerializeField] private Transform _messageParent;
		private void OnEnable()
		{
			MessageSystem.OnNewMessage += OnNewMessage;
		}
		private void OnDisable()
		{
			MessageSystem.OnNewMessage -= OnNewMessage;
		}

		// public override void EnableScreen(StatusDisplay statusDisplay)
		// {
		// 	base.EnableScreen(statusDisplay);
		// 	RedrawMessages();
		// }

		private void OnNewMessage(MessageInfo message)
		{
			_messageSystem = message.MessageSystem;
			_mostRecentMessage = message;
			RedrawMessages();
		}

		private void ClearMessageUIChildren()
		{
			foreach(Transform child in _messageParent)
			{
				Destroy(child.gameObject);
			}
		}
		private void RedrawMessages()
		{
			ClearMessageUIChildren();
			if (_messageSystem == null)
			{
				//no first message yet.
				return;
			}
			foreach (var message in _messageSystem.GetRecentMessages(messagesToDisplay))
			{
				Instantiate(_messagePrefab, _messageParent).GetComponent<MessageInfoText>().Init(message);
			}
		}
	}
}