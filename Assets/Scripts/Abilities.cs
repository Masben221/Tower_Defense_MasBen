using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SpaceShooter;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace TowerDefense
{
    public class Abilities : MonoSingleton<Abilities>
    {
        public static int levelfireUpgrade { get; private set; }
        public static int leveltimeUpgrade { get; private set; }  

        private void Start()
        {
            levelfireUpgrade = Upgrades.GetUpgradeLevel(fireUpgrade); // using upgrades in the game
            if (levelfireUpgrade == 0)
            {
                Instance.FireButton.gameObject.SetActive(false);
            }

            leveltimeUpgrade = Upgrades.GetUpgradeLevel(timeUpgrade); // using upgrades in the game
            if (leveltimeUpgrade == 0)
            {
                Instance.TimeButton.gameObject.SetActive(false);
                //Instance.TimeButton.interactable = false;
            }

            m_FireAbility.CostToCheck = m_FireAbility.Cost * levelfireUpgrade;
            m_FireAbility.CostText.text = m_FireAbility.CostToCheck.ToString();

            m_TimeAbility.CostToCheck = m_TimeAbility.Cost * leveltimeUpgrade;
            m_TimeAbility.CostText.text = m_TimeAbility.CostToCheck.ToString();

            TDPlayer.Instance.GoldUpdateSubscride(CheckUpgradesGold);
            TDPlayer.Instance.ManaUpdateSubscride(CheckUpgradesMana);

            m_TimeAbility.IsCooledDown = true;
            CheckUpgradesMana(1);
        }
        
        [Header("Параметры FireAbility")]
        [SerializeField] private Button FireButton;
        [SerializeField] private UpgradeAsset fireUpgrade;
        [SerializeField] private FireAbility m_FireAbility;        

        [Serializable]
        public class FireAbility 
        {
            [SerializeField] private int m_Cost = 5;
            public int Cost { get => m_Cost; set => m_Cost = m_Cost = value; }

            private int m_CostToCheck;
            public int CostToCheck { get => m_CostToCheck; set => m_CostToCheck = value; }

            [SerializeField] private Text m_CostText;
            public Text CostText { get => m_CostText; }

            [SerializeField] private int m_Damage = 5;
            private int Damage; 
            [SerializeField] private int m_RadiusCircle = 1;

            [SerializeField] private GameObject m_ParticleFire;

            public void Use()
            {
                Damage = m_Damage * levelfireUpgrade;
                //print("Damage = " + Damage);
                m_ParticleFire.SetActive(false);

                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);
                    m_ParticleFire.transform.position = position;
                    m_ParticleFire.SetActive(true);

                    foreach (var collider in Physics2D.OverlapCircleAll(position, m_RadiusCircle)) // взрыв
                    {                        
                        if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                             enemy.TakeDamage(Damage, TDProjectile.DamageType.Magic);
                        }
                            /*if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                            {
                                enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Base);
                            }*/
                    }
                });                              
            }
        }

        [Header("Параметры TimeAbility")]
        [SerializeField] private Button TimeButton;
        [SerializeField] private UpgradeAsset timeUpgrade;
        [SerializeField] private TimeAbility m_TimeAbility;

        [Serializable]
        public class TimeAbility 
        {           
            [SerializeField] private float m_Duration = 5f;
            [SerializeField] private int m_Cost = 10;
            public int Cost { get => m_Cost; set => m_Cost = m_Cost = value; }

            private int m_CostToCheck;
            public int CostToCheck { get => m_CostToCheck; set => m_CostToCheck = value; }

            [SerializeField] private Text m_CostText;
            public Text CostText { get => m_CostText; }

            [SerializeField] private float m_Cooldown = 15f;

            public bool IsCooledDown;

            public void Use()
            {
                //print("Duration = " + m_Duration * leveltimeUpgrade);

                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }

                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.HalfMaxLinearVelocity();

                EnemyWaveManager.OnEnemySpawn += Slow;

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration * leveltimeUpgrade);//время работы замедления с учетом уровня апгрейда
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                        ship.RestoreMaxLinearVelocity();

                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }

                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    Instance.TimeButton.interactable = false;
                    IsCooledDown = false;
                    yield return new WaitForSeconds(m_Cooldown);
                    IsCooledDown = true;
                    Instance.TimeButton.interactable = true;
                }
                Instance.StartCoroutine(TimeAbilityButton());
            }
        }

        private void CheckUpgradesGold(int gold)
        {
            if (TDPlayer.Instance.Gold >= m_FireAbility.CostToCheck)
            {
                Instance.FireButton.interactable = true;
            }
            else 
            {
                Instance.FireButton.interactable = false;
            }
        }
        private void CheckUpgradesMana(int mana)
        {
            //print(m_TimeAbility.IsCooledDown);
            if (TDPlayer.Instance.Mana >= m_TimeAbility.CostToCheck && m_TimeAbility.IsCooledDown == true)
            {
                Instance.TimeButton.interactable = true;
            }
            else
            {
                Instance.TimeButton.interactable = false;
            }
        }
        private void OnDestroy()
        {
            TDPlayer.Instance.TextUnSubscrible(CheckUpgradesGold);
            TDPlayer.Instance.TextUnSubscrible(CheckUpgradesMana);
        }

        public void UseFireAbility()
        {
            if (TDPlayer.Instance.Gold >= m_FireAbility.CostToCheck) 
            {
                TDPlayer.Instance.ChangeGold(-m_FireAbility.CostToCheck);
                m_FireAbility.Use();
            } 
        }

        public void UseTimeAbility() 
        {
            if (TDPlayer.Instance.Mana >= m_TimeAbility.CostToCheck)
            {
                TDPlayer.Instance.ChangeMana(-m_TimeAbility.CostToCheck);
                m_TimeAbility.Use();
            }
        }
    }
}
