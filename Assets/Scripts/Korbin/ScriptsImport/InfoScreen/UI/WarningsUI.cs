using Timeline;
using UnityEngine;
using UnityEngine.UI;

namespace Info.UI
{
	public class WarningsUI : MonoBehaviour
	{
		[SerializeField] private GameTimeline _timeline;
		[SerializeField] private Sector _sector;
		[SerializeField] private Image _image;

		private void OnEnable()
		{
			_timeline.OnNewBeat += OnBeat;
			//todo: diable image with OnTimelineOver
			OnBeat();
		}

		private void OnBeat()
		{
			if (_timeline.IsTimelineActive && _timeline.TryGetShipEventInSector(_sector,0, out var shipEvent))
			{
				_image.enabled = true;
				//todo: will we always want the damage icon? uh?
				_image.sprite = shipEvent.DamageType.Icon;//sprite from here, from damage type?
				_image.color = shipEvent.DamageType.iconColor;
			}
			else
			{
				_image.enabled = false;
			}
		}

		private void OnDisable()
		{
			_timeline.OnNewBeat -= OnBeat;

		}
	}
}