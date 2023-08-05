using UnityEngine;
using SpaceShooter;

namespace TowerDefense {
    public class TDLevelController : LevelController
    {
        private int levelScore = 3;
        private new void Start()
        {
            base.Start();
            TDPlayer.Instance.OnPlayerDead += () =>
            {
                StopLevelActivity();
                LevelResultController.Instance.Show(false);
            };
            m_EventLevelCompleted.AddListener(() =>
            {
                StopLevelActivity();
                if (m_ReferenceTime <= Time.time)
                {
                    levelScore -= 1;
                }
                MapCompletion.SaveEpisodeResult(levelScore);
            });
            void LifeScoreChange(int _)
            {
                levelScore -= 1;
                TDPlayer.OnLifeUpdate -= LifeScoreChange;
            }
            TDPlayer.OnLifeUpdate += LifeScoreChange;
        }
        private void StopLevelActivity()
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            void DisableAll<T>() where T: MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }
                       
            DisableAll<EnemyWave>();            
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<BuildSite>();
            DisableAll<Abilities>();
            DisableAll<NextWaveGUI>();
        }
    }
}