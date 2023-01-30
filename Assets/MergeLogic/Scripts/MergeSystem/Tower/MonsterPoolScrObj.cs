using System.Collections.Generic;
using UnityEngine;

namespace MergeSystem
{
    [CreateAssetMenu(fileName = "MontersPool", menuName = "Create/Data/MontersPool")]
    public class MonsterPoolScrObj : ScriptableObject
    {
        [SerializeField] private TowerPack[] _monsterPacks;

        public bool TryGetMonsterData(int level, out TowerData monsterData)
        {
            monsterData = null;
            if (_monsterPacks != null)
            {
                for (int i = 0; i < _monsterPacks.Length; i++)
                {
                    if (_monsterPacks[i].MonsterDataScrObjects == null) continue;
                    {
                        if (_monsterPacks[i].MonsterDataScrObjects.Length > level) monsterData = _monsterPacks[i].MonsterDataScrObjects[level].TowerData;
                        break;
                    }
                }

            }
            return monsterData != null;
        }
        /*
        private void InitializeMonstersMap()
        {
            _monstersMap = new Dictionary<MonsterTypes, MonsterDataScrObj[]>();

            for (int i = 0; i < _monsterPacks.Length; i++)
            {
                if (_monsterPacks[i].MonsterDataScrObjects == null) continue;
                _monstersMap.Add(_monsterPacks[i].MonsterType, _monsterPacks[i].MonsterDataScrObjects);

                //for (int j = 0; j < _monsterPacks[i].MonsterDataScrObjects.Length; j++)
                //{
                    // _monsterPacks[i].MonsterDataScrObjects[j].MonsterData.Initialize(j, _monsterPacks[i].MonsterType);
                //}

            }
        }
        */
        [System.Serializable]
        private struct TowerPack
        {
            public MonsterDataScrObj[] MonsterDataScrObjects => _monsterDataScrObjects;
            [Tooltip("Index must be equal the Level of the Monster.")]
            [SerializeField] private MonsterDataScrObj[] _monsterDataScrObjects;
        }
    }
}