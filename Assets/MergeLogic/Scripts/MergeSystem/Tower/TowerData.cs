using UnityEngine;


namespace MergeSystem
{
    [System.Serializable]
    public class TowerData
    {
        public Tower Monster => _monster;
        [SerializeField] private Tower _monster;

        public static int MaxLevel = 9;
    }
}