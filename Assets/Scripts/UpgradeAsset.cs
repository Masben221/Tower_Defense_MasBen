using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject
    {
        [Header("Внешний вид Upgrade")]
        public Sprite sprite;        


        [Header("Механика")]
        public string Name;
        public int[] costByLevel = { 3 };

    }
}