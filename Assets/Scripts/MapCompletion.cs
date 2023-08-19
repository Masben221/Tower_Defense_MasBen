using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;
using System;

namespace TowerDefense {
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        public const string filename = "completion.dat";

        [SerializeField] private EpisodeScore[] completionData;       

        private UpgradeShop m_UpgradeShop;//ссылка на UpgradeShop для UpdateMoney

        public int TotalScore { private set; get; }

        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int score;
        }
        
        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref completionData);
            foreach (var episodeScore in completionData)
            {
                TotalScore += episodeScore.score;
                //Debug.Log(TotalScore);
            }           
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                foreach (var item in Instance.completionData)
                { // Сохранение новых очков прохождения
                    if (item.episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.score)
                        {
                            Instance.TotalScore += levelScore - item.score;
                            item.score = levelScore;
                            Saver<EpisodeScore[]>.Save(filename, Instance.completionData);
                           // print(levelScore);
                           //print(Instance.completionData);
                        }

                    }
                }
            }
            else
            {
                //Debug.Log($"Episode complete with score {levelScore}");
            }
        }

        public void ResetData()
        {
            foreach (var episodeScore in completionData)
            {
               episodeScore.score = 0;               
               TotalScore = 0;
            }
        }

        public void UpdateMoney()
        {            
            m_UpgradeShop = FindObjectOfType<UpgradeShop>();
            m_UpgradeShop.money = TotalScore;
            m_UpgradeShop.money -= Upgrades.GetTotalCost();
            m_UpgradeShop.moneyText.text = m_UpgradeShop.money.ToString();
            foreach (var slot in m_UpgradeShop.sales)
            {
                slot.CheckCost(m_UpgradeShop.money);
            }           
        }

        /*public bool TryIndex(int id, out Episode episode, out int score)
       {
           if(id >= 0 && id < completionData.Length)
           {
               episode = completionData[id].episode;
               score = completionData[id].score;
               return true;
           }

           episode = null;
           score = 0;
           return false;
       }*/
        public int GetEpisodeScore(Episode m_episode)
        {
            foreach (var data in completionData)
            {
                if (data.episode == m_episode) { return data.score; }
            }
            return 0;
        }
    }
}



/*public static void SaveEpisodeResult(int levelScore)
{
    if (Instance)
    {
        foreach (var item in Instance.completionData)
        { // Сохранение новых очков прохождения
            if (item.episode == LevelSequenceController.Instance.CurrentEpisode)
            {
                if (levelScore > item.score)
                {
                    Instance.TotalScore += levelScore - item.score;
                    item.score = levelScore;
                    Saver<EpisodeScore[]>.Save(filename, Instance.completionData);
                    print(levelScore);
                    print(Instance.completionData);
                }

            }
        }
    }
    else
    {
        Debug.Log($"Episode complete with score {levelScore}");
    }
}*/