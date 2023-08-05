using UnityEngine;

namespace TowerDefense
{
    public class Rotator : MonoBehaviour
	{
		[SerializeField] private Transform targetTransform;
		[SerializeField] private Vector3 speed;

		void Update()
		{
			targetTransform.Rotate(speed * Time.deltaTime, Space.Self);
		}
	}
}