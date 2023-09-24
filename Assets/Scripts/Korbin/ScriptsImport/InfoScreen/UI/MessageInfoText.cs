using Timeline;
using TMPro;
using UnityEngine;

namespace Info.UI
{
	
	public class MessageInfoText : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;
		public void Init(MessageInfo message)
		{
			_text.text = message.Message;
		}
	}
}