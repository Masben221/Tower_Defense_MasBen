using UnityEngine;
using TowerDefense;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceShooter
{
    public class ExplodedProjectile : Projectile
    {
        [SerializeField] private float m_ExplosionRadius;
        [SerializeField] private int m_DamageExploded;
        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            FindExplosionTarget();
            base.OnProjectileLifeEnd(col, pos);
        }
        public void FindExplosionTarget()
        {
            Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, m_ExplosionRadius);
            foreach (Collider2D col in potentialTargets)
            {
                Destructible dest = col.transform.root.GetComponent<Destructible>();
                if (col.transform.root.CompareTag("Player") == true) { continue; }
                if (dest != null)
                {
                    dest.ApplyDamage(m_DamageExploded);
                }
            }
        }
        #if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);
        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_ExplosionRadius);
        }
        #endif
    }
}