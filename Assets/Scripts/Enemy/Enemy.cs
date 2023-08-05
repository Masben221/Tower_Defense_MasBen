using UnityEngine;
using SpaceShooter;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefense
{
    [RequireComponent(typeof(TDPatrolController))]
    [RequireComponent(typeof(Destructible))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType { Base = 0, Magic = 1 }
        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            (int power, TDProjectile.DamageType type, int armor) =>
            { //ArmorType.Base
                switch(type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power - armor,1);
                }
            },
            (int power, TDProjectile.DamageType type, int armor) =>
            { //ArmorType.Magic          

                if (TDProjectile.DamageType.Base == type) armor = armor /2;

                return Mathf.Max(power- armor,1);
            }
        };

        [SerializeField] private int m_damage = 1;
        [SerializeField] private int m_armor = 1;
        [SerializeField] private ArmorType m_ArmorType;

        [SerializeField] private int m_gold = 1;
        [SerializeField] private int m_score = 1;
        [SerializeField] private int m_Mana = 1;
        
        public event Action OnDead;
        private Destructible m_Destructible;
        public event Action OnEnd;

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }
        private void OnDestroy()
        {
            OnEnd?.Invoke();
            OnDead?.Invoke();
            OnDead = null;
        }
        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animatorController;
            GetComponent<SpaceShip>().Use(asset);
            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;
            m_damage = asset.damage;
            m_armor = asset.armor;
            m_ArmorType = asset.armorType;

            m_gold = asset.gold;
            m_score = asset.score;
            m_Mana = asset.mana;
        }
        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_damage);
        }
        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_gold);
        }
        public void GivePlayerScore()
        {
            TDPlayer.Instance.ChangeScore(m_score);
        }
        public void GivePlayerMana()
        {
            TDPlayer.Instance.ChangeMana(m_Mana);
        }

        public void TakeDamage(int damage, TDProjectile.DamageType damageTyte)
        {
            m_Destructible.ApplyDamage(
                ArmorDamageFunctions[(int)m_ArmorType](damage, damageTyte, m_armor));
        }
    }
    /*[CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label("EditorGUICustom");
            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }

    }*/
}