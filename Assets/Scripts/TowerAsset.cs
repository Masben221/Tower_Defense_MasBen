using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public int goldCost = 15;
        public Sprite sprite;
        public Sprite GUISprite;
        public Tower m_towerPrefab;
        public TurretProperties m_TowerTurretProperties;

        [SerializeField] private UpgradeAsset requaredUpgrade; //требуемый апгрейд
        [SerializeField] private int requiredUpgradeLevel; //уровень требуеого апгрейда

        public bool IsAvailable() => !requaredUpgrade || requiredUpgradeLevel <= Upgrades.GetUpgradeLevel(requaredUpgrade);

        public TowerAsset[] m_UpgradesTo;
    }
}