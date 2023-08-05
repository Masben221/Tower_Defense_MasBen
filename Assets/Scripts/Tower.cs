using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense {
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius = 4f;
        private Turret[] turrets;
        private Destructible target = null;
        private Vector2 m_targetVector;
        private Projectile projectileTower;

        public void Use(TowerAsset asset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = asset.sprite;
            turrets = GetComponentsInChildren<Turret>();

            foreach (var turret in turrets)
            {
                turret.AssignLoadout(asset.m_TowerTurretProperties);
            }

            GetComponentInChildren<BuildSite>().SetBuildableTowers(asset.m_UpgradesTo);
        }

        private void Start()
        {
            turrets = GetComponentsInChildren<Turret>();
            m_Radius += TDPlayer.levelradiusUpgrade; // ������ ����������� ���� � ������ ��������
        }
        private void Update()
        {
            if (target) 
            {
                m_targetVector = target.transform.position - transform.position;
                //m_targetVector = Vector2.MoveTowards(transform.position,
                //target.transform.position, Time.deltaTime * turret.ProjectileSpeed);
                
                if (m_targetVector.magnitude <= m_Radius)
                { 
                    foreach (var turret in turrets)
                    {
                        //m_targetVector = Vector2.MoveTowards(transform.position, target.transform.position, 
                        //    Time.deltaTime * this.ProjectileSpeed);
                        turret.transform.up = m_targetVector;
                        turret.Fire();
                    }
                }
                else 
                { 
                    target = null; 
                }
            }
            else 
            {
                var entherCollider = (Physics2D.OverlapCircle(transform.position, m_Radius));
                if (entherCollider)
                {
                    target = entherCollider.transform.root.GetComponent<Destructible>();
                }
            }
        }
#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
#endif
    }
}