using SpaceShooter;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public class TDPatrolController : AIController
    {
        private Path m_path;
        private int pathIndex;
        [SerializeField] private UnityEvent OnEndPath;

        internal void SetPath(Path newPath)
        {
            m_path = newPath;
            pathIndex = 0;
            SetPatrolBehaviour(m_path[pathIndex]);
        }
        protected override void GetNewPoint()
        {
            pathIndex++;
            if (m_path.Lenght > pathIndex) { SetPatrolBehaviour(m_path[pathIndex]); }
            else { OnEndPath.Invoke(); Destroy(gameObject); }
        }
    }
}