using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWave : MonoBehaviour
    {
        public static Action<float> OnWavePrepare;
        [Serializable]
        private class Squad
        {
            public EnemyAsset asset;
            public int count;
        }
        [Serializable]
        private class PathGroup
        {
            public Squad[] squads; 
        }
        [SerializeField] private PathGroup[] groups;
        [SerializeField] private float prepareTime = 10f;
        public float GetRemainingTime() { return prepareTime - Time.time; }
        private void Awake()
        {
            enabled = false;
        }
        private event Action OnWaveReady;// событие готовности волны
        public void Prepare(Action spawnEnemies)
        {
            OnWavePrepare?.Invoke(prepareTime); // событие начала подготовки волны
            prepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies; // подписка спавна врагов на событие готовности волны
        }
        private void Update()
        {
            if (Time.time >= prepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke(); // событие готовности волны
            }
        }
        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()// перечисление
        {
            for (int i = 0; i < groups.Length; i++)
            {
                foreach (var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                }
            }
        }
        [SerializeField] private EnemyWave nextWave;
        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies; // отписка от события
            if (nextWave) nextWave.Prepare(spawnEnemies);
            return nextWave;
        }  
    }
}