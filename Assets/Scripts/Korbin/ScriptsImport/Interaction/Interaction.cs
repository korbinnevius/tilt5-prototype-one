namespace Interaction
{
	public class Interaction
	{
		public string Verb;
		public Interactable Interest;

		public delegate void InteractionDelegate();

		public InteractionDelegate Interact;

		public void Clear()
		{
			Verb = "";
			Interact = null;
			Interest = null;
		}
	};

}