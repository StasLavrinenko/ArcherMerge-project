using UnityEngine;

namespace MergeSystem
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "Create/Data/MonsterData")]
    public class MonsterDataScrObj : ScriptableObject
    {
        public TowerData TowerData => _monsterData;
        [SerializeField] private TowerData _monsterData;
    }
}



