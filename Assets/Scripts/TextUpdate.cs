using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense 
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Life, Score, Mana }
        
        public UpdateSource source = UpdateSource.Gold;

        private Text m_text;
        void Start()
        {
            m_text = GetComponent<Text>();
            switch (source)
            {
                case UpdateSource.Gold: TDPlayer.Instance.GoldUpdateSubscride(UpdateText);
                break;
                case UpdateSource.Life: TDPlayer.Instance.LifeUpdateSubscride(UpdateText);
                break;
                case UpdateSource.Score: TDPlayer.Instance.ScoreUpdateSubscride(UpdateText);
                break;
                case UpdateSource.Mana: TDPlayer.Instance.ManaUpdateSubscride(UpdateText);
                break;
            }
        }
        private void UpdateText(int value)
        {
            m_text.text = value.ToString();
        }
        private void OnDestroy()
        {
            TDPlayer.Instance.TextUnSubscrible(UpdateText);
        }
    }
}