using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class TDProjectile : Projectile
    {
        public enum DamageType { Base, Magic}
        [SerializeField] private DamageType m_DamageType;
        //[SerializeField] private Sound m_ShootSound = Sound.Arrow;
        //[SerializeField] private Sound m_HitSound = Sound.ArrowHit;

        [SerializeField] private float m_ExplosionRadius;
        [SerializeField] private int m_DamageExploded;

        private void Start()
        {
            //m_ShootSound.Play();
        }
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
                Enemy enemy = col.transform.root.GetComponent<Enemy>();
                
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage, m_DamageType);
                }
            }
        }

        protected override void OnHit(RaycastHit2D hit)
        {
            //m_HitSound.Play();

            var enemy = hit.collider.transform.root.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage, m_DamageType);
            }
        }
    }
}