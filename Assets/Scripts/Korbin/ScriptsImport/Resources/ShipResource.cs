using UnityEngine;

namespace Resources
{
	[CreateAssetMenu(fileName = "Res", menuName = "Ship/Resource", order = 0)]
	public class ShipResource : ScriptableObject
	{
		[SerializeField] private ResourceElement _resourcePrefab;

		public ResourceElement InstantiateElementGameObject(Vector3 position, Transform parent = null, int resourceAmount = 1)
		{
			//create different prefabs depending on amount of resoureces;
			var resource = Instantiate(_resourcePrefab,position,_resourcePrefab.transform.rotation,parent);
			resource.SetAmount(resourceAmount);
			if (resource.Resource != this)
			{
				Debug.LogWarning("Resource {this} has improperly setup prefab?");
				resource.SetResource(this);
			}

			return resource;
		}
	}
}