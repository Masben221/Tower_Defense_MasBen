using UnityEngine;
using System;
using SpaceShooter;

namespace TowerDefense 
{

    public class Upgrades : MonoSingleton<Upgrades>
    {
        public const string filename = "Upgrades.dat";
        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }
        [SerializeField] private UpgradeSave[] saves;
        private new void Awake()
        {
            base.Awake();
            Saver<UpgradeSave[]>.TryLoad(filename, ref saves);
            foreach (var upgrade in Instance.saves)
            {
                //print(upgrade.level);
                if (FileHandler.HasFile(filename) == false)
                {
                    upgrade.level = 0;
                }
                //print(upgrade.level);
            }
        }
        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.saves)
            {
                if(upgrade.asset == asset)
                {
                    upgrade.level += 1;
                    Saver<UpgradeSave[]>.Save(filename, Instance.saves);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach (var upgrade in Instance.saves)
            {
                for (int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costByLevel[i];
                    //print(upgrade.level);
                }
            }
            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.saves)
            {
                if (upgrade.asset == asset)
                {                    
                    return upgrade.level;
                    //Debug.Log(upgrade.level);
                }
            }
            return 0;
        }
    }
}