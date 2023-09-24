using Timeline;
using TMPro;
using UnityEngine;

namespace Info.UI
{
	public class BeatCountdownUI : MonoBehaviour
	{
		[SerializeField] private GameTimeline _gameTimeline;
		[SerializeField] private TMP_Text _text;

		private void Update()
		{
			if (_gameTimeline.IsTimelineActive)
			{
				_text.text = Mathf.Ceil(_gameTimeline.CurrentCountdownInBeat).ToString("N0");
			}
			else
			{
				_text.text = "";
			}
		}
	}
}