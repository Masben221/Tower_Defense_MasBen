using System;
using SpaceShooter;
using UnityEngine;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance { get { return Player.Instance as TDPlayer; } }

        //public static Upgrades LevelgoldUpgrade { get => levelgoldUpgrade; set => levelgoldUpgrade = value; }

        public static event Action<int> OnGoldUpdate;
        public static event Action<int> OnLifeUpdate;
        public static event Action<int> OnScoreUpdate;
        public static event Action<int> OnManaUpdate;

        [SerializeField] private int m_gold = 0;
        public int Gold => m_gold;

        [SerializeField] private int m_Mana = 0;
        public int Mana => m_Mana;

        //[SerializeField] private int m_score = 0;
        [SerializeField] private Tower m_TowerPrefab;

        public void GoldUpdateSubscride(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_gold);
        }        

        public void LifeUpdateSubscride(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }
             
        public void ScoreUpdateSubscride(Action<int> act)
        {
            OnScoreUpdate += act;
            act(Instance.Score);
        }   
        
        public void ManaUpdateSubscride(Action<int> act)
        {
            OnManaUpdate += act;
            act(Instance.m_Mana);
        }        

        public void ChangeGold(int change)
        {
            m_gold += change;
            if (OnGoldUpdate != null)
            {
                OnGoldUpdate(m_gold);
            }
        }
        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }

        public void ChangeScore(int change)
        {
            AddScore(change);
            //m_score += change;
            OnScoreUpdate(Score);
        }
        public void ChangeMana(int change)
        {
            m_Mana += change;
            if (OnManaUpdate != null)
            {
                OnManaUpdate(m_Mana);
            }
        }

        // TODO: верим в то, что золота на постройку достаточно
        public void TryBuild(TowerAsset m_TowerAsset, Transform buildSite)
        {
            ChangeGold(-m_TowerAsset.goldCost);
            var tower = Instantiate(m_TowerAsset.m_towerPrefab, buildSite.position, Quaternion.identity);
            //tower.GetComponentInChildren<SpriteRenderer>().sprite = m_TowerAsset.sprite;
            tower.Use(m_TowerAsset);
            Destroy(buildSite.gameObject);
        }

        public void TextUnSubscrible(Action<int> action)
        {
            OnGoldUpdate -= action;
            OnLifeUpdate -= action;
            OnScoreUpdate -= action;
            OnManaUpdate -= action;
        }

        /*public static void GoldUnSubscrible(Action<int> action)
        {
            OnGoldUpdate -= action;
        }*/

        // Апгрейды

        [SerializeField] private UpgradeAsset healthUpgrade;
        [SerializeField] private UpgradeAsset goldUpgrade;
        [SerializeField] private UpgradeAsset damageUpgrade;
        [SerializeField] private UpgradeAsset rateOfFireUpgrade;
        [SerializeField] private UpgradeAsset radiusUpgrade;

        public static int leveldamageUpgrade { get; private set; }
        public static float levelRateOfFireUpgrade { get; private set; }
        public static float levelradiusUpgrade { get; private set; }

        private new void Awake()
        {
            base.Awake();
            
            var levelhealthUpgrade = Upgrades.GetUpgradeLevel(healthUpgrade);
            TakeDamage(-levelhealthUpgrade * 5);

            var levelgoldUpgrade = Upgrades.GetUpgradeLevel(goldUpgrade);
            ChangeGold(m_gold * levelgoldUpgrade);

            leveldamageUpgrade = Upgrades.GetUpgradeLevel(damageUpgrade);

            levelRateOfFireUpgrade = Upgrades.GetUpgradeLevel(rateOfFireUpgrade)/10f;

            levelradiusUpgrade = Upgrades.GetUpgradeLevel(radiusUpgrade)/2f;        
                     
        }
    }
}

/*var enemies = FindObjectsOfType<Enemy>();

float nearestEnemyDistance = Mathf.Infinity;

foreach (var enemy in enemies)
{
    float dist = Vector2.Distance(transform.position, enemy.transform.position);

    if (dist < nearestEnemyDistance)
    {
        m_nearestEnemy = enemy.transform;

        nearestEnemyDistance = dist;
    }
}*/