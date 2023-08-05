using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense {
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image upgradeIcon;
        [SerializeField] private Text level, cost;
        [SerializeField] private Button buyButton;
        private int costNumber = 0;

        
        public void Initialize()
        {
            upgradeIcon.sprite = asset.sprite;
            nameText.text = asset.Name;

            var savedlevel = Upgrades.GetUpgradeLevel(asset);

            if (savedlevel >= asset.costByLevel.Length)
            {
                level.text = $"Lvl : {savedlevel} (Max)";
                buyButton.interactable = false;
                buyButton.transform.Find("BuyImage").gameObject.SetActive(false);
                buyButton.transform.Find("BuyText").gameObject.SetActive(false);
                cost.text = "X";
                costNumber = int.MaxValue;// самый большой int из возможных
            }
            else
            {
                level.text = $"Lvl: {savedlevel + 1}";
                costNumber = asset.costByLevel[savedlevel];
                cost.text = costNumber.ToString();
            }
        }
        public void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
            Initialize();
        }
    }
}