using TowerDefense;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Турелька корабля. Требует аудио источник для выдачи спецэффекта при стрельбе.
    /// Требует на верхенм уровне скрипт SpaceShip для вычитания патронов и энергии.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// Тип турели, первичный или вторичный.
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;
        /// <summary>
        /// Текущие патроны в турели.
        /// </summary>
        [SerializeField] private TurretProperties m_TurretProperties;

        [SerializeField] private float m_VolumeSFX = 1f;

        [SerializeField] private AudioSource m_AudioSource;

        /// <summary>
        /// Таймер повторного выстрела.
        /// </summary>
        private float m_RefireTimer;

        /// <summary>
        /// Стрелять можем? 
        /// </summary>
        public bool CanFire => m_RefireTimer <= 0;

        /// <summary>
        /// Кешированная ссылка на родительский шип.
        /// </summary>
        private SpaceShip m_Ship;

        #region Unity events

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0) { m_RefireTimer -= Time.deltaTime; }
            else if (Mode == TurretMode.Auto) { Fire(); }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Метод стрельбы турелью. 
        /// </summary>
        public void Fire()
        {
            if (m_RefireTimer > 0)
                return;

            if (m_TurretProperties == null)
                return;
            if (m_Ship)
            {
                // кушаем энергию
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                    return;

                // кушаем патроны
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }

            // инстанцируем прожектайл который уже сам полетит.
            var projectile = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<TDProjectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            // добавление к дамагу апгрейд если есть

            projectile.Damage += TDPlayer.leveldamageUpgrade;

            // метод выставления данных прожектайлу о том кто стрелял для избавления от попаданий в самого себя
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;
            m_RefireTimer -= TDPlayer.levelRateOfFireUpgrade; //скорострельность с учетом апгрейда
            //print("RateOfFire " + m_RefireTimer);

            {
                //playSFX
                m_AudioSource.PlayOneShot(m_TurretProperties.LaunchSFX, m_VolumeSFX);
            }
        }

        /// <summary>
        /// Установка свойств турели. Будет использовано в дальнейшем для паверапки.
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode)
                return;

            m_TurretProperties = props;
            m_RefireTimer = 0;
        }
        #endregion       
    }
}