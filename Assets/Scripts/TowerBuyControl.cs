using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefense
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        public void SetTowerAsset(TowerAsset asset) { m_TowerAsset = asset; }

        [SerializeField] private Text m_text;
        [SerializeField] private Button m_button;
        [SerializeField] private Transform buildSite;
        public void SetBuildSite(Transform value)
        { 
            buildSite = value;
        }
        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscride(GoldStatusCheck);
            m_text.text = m_TowerAsset.goldCost.ToString();
            m_button.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }
        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TowerAsset.goldCost != m_button.interactable)
            {
                m_button.interactable = !m_button.interactable;
                m_text.color = m_button.interactable ? Color.white : Color.red;
            }
        }
        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, buildSite);
            BuildSite.HideControls();
        }
        private void OnDestroy()
        {
            TDPlayer.Instance.TextUnSubscrible(GoldStatusCheck);
        }
    }
}