using UnityEngine;
using SpaceShooter;
using System.Collections.Generic;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        //[SerializeField] private TowerAsset[] m_TowerAssets;

        private List<TowerBuyControl> m_ActiveControl;

        private RectTransform m_RectTransform;

        #region Unity Events
        private void Awake()
        {            
            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
            m_RectTransform = GetComponent<RectTransform>();
        }
        public void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }
        #endregion
        private void MoveToBuildSite(BuildSite buildSite)
        {
            if (buildSite)
            {                
                var position = Camera.main.WorldToScreenPoint(buildSite.transform.root.position);
                m_RectTransform.anchoredPosition = position;
                gameObject.SetActive(true);

                m_ActiveControl = new List<TowerBuyControl>();

                foreach (var asset in buildSite.buildableTowers)
                {
                    if (asset.IsAvailable())
                    {
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);

                        newControl.SetTowerAsset(asset);
                    }
                }
                if (m_ActiveControl.Count > 0)
                {
                    gameObject.SetActive(true);
                    var angle = 360 / m_ActiveControl.Count;
                    for (int i = 0; i < m_ActiveControl.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.up * 100);
                        m_ActiveControl[i].transform.position += offset;
                    }
                    foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                    {
                        tbc.SetBuildSite(buildSite.transform.root);
                    }
                }
            }
            else
            {
                if (m_ActiveControl != null)
                {
                    foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                    m_ActiveControl.Clear();
                    gameObject.SetActive(false);
                }                
            }            
        }
    }
}