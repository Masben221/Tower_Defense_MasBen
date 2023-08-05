using UnityEngine;
using UnityEngine.UI;
using System;

namespace TowerDefense {
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] public int money;
        [SerializeField] public Text moneyText;
        [SerializeField] public BuyUpgrade[] sales;
        private void Start()
        {
            var m = FindObjectOfType<MapCompletion>();            

            foreach (var slot in sales)
            {
                slot.Initialize();
                slot.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(m.UpdateMoney);
            }           
          
           m.UpdateMoney();           
        }
        /*public void UpdateMoney()
        {
            money = MapCompletion.Instance.TotalScore;
            money -= Upgrades.GetTotalCost();
            moneyText.text = money.ToString();
            foreach (var slot in sales)
            {
                slot.CheckCost(money);
            }
        }*/
    }
}