using Ship;
using TMPro;
using UnityEngine;

namespace Info.UI
{
	public class ShipHealthUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;
		[SerializeField] private ShipInfo _shipInfo;

		private void Update()
		{
			_text.text = _shipInfo.Health.ToString("D");
		}
	}
}