using System;
using UnityEngine;

namespace TowerDefense {
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;

        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWave;
        
        private int activeEnemyCount = 0;
        public event Action OnAllWavesDead;

        private void RecordEnemyDead()
        { 
            if (--activeEnemyCount == 0)
            {
                ForceNextWave();
            }
        }
        private void Start()
        {
            currentWave.Prepare(SpawnEnemies);
        }

        internal void ForceNextWave()
        {
            if (currentWave)
            {
                TDPlayer.Instance.ChangeGold((int)currentWave.GetRemainingTime());
                SpawnEnemies();
            }
            
            else

            {
                if (activeEnemyCount == 0) { OnAllWavesDead?.Invoke(); }
            }
        }
        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in currentWave.EnumerateSquads())
            {
                if (pathIndex < paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab, 
                                            paths[pathIndex].StartArea.RandomInsideZone, 
                                            Quaternion.identity);
                        e.OnEnd += RecordEnemyDead;
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(paths[pathIndex]);
                        activeEnemyCount += 1;
                        OnEnemySpawn?.Invoke(e);
                    }
                }
                else
                {
                    //Debug.LogWarning($"Invalid pathIndex in {name}");
                }
                
            }
            currentWave = currentWave.PrepareNext(SpawnEnemies);
        }
    }
}