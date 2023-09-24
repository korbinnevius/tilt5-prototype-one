using UnityEngine;
using UnityEngine.Serialization;

namespace Ship
{
	[CreateAssetMenu(fileName = "Damage", menuName = "Ship/Ship Damage Type", order = 0)]
	public class ShipDamageType : ScriptableObject
	{
		//todo: display name
		[FormerlySerializedAs("Sprite")] public Sprite Icon;
		public Color iconColor;
	}
}