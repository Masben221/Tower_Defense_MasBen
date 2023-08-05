using SpaceShooter;
using UnityEngine;

namespace TowerDefense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea startArea;
        public CircleArea StartArea { get { return startArea; } }
        [SerializeField] private AIPointPatrol[] points;
        public int Lenght { get => points.Length; }
        public AIPointPatrol this[int i] { get => points[i]; }

#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            foreach (var point in points) { Gizmos.DrawSphere(point.transform.position, point.Radius); }
        }
#endif
    }
}