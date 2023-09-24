namespace Timeline
{
	public class MessageInfo
	{
		//todo enum for message type that controls icons and color of message.
		public string Message;
		public MessageSystem MessageSystem;
		public MessageInfo(string message)
		{
			Message = message;
			//player = null or player index?
		}
	}
}