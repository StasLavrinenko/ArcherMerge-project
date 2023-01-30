//using CoreHyperCasual;
using MergeSystem.Cells;
using UnityEngine;

namespace MergeSystem
{
    public class StartMonstersCreator : MonoBehaviour
    {
        [Range(0, 9)][SerializeField] private int _level;

        [SerializeField] private MonsterPoolScrObj _monsterPool;
        [SerializeField] private PointsContainer _pointsContainer;

        //[SerializeField] private ScoreData scoreData;

        [SerializeField] private int _startCoins = 100;

        private MonstersVM _monstersVM;
        private void Awake()
        {
            _monstersVM = MonstersVM.Instance;
            if (!_monstersVM.TryCreateMonstersFromSave(_monsterPool))
            {
                CreateMonsters();
                AddStartCoins();
            }
        }

        public void CreateMonsters()
        {
            if (!_monsterPool) return;


            for (int i = 0; i < 2; i++) 
            {
                if (_pointsContainer.TryGetNextFreePoint(out Point point))
                {
                    if (!_monstersVM.TryAddMonster(_level, new Vector2Int(point.X + i, point.Z)))
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (_pointsContainer.TryGetNextFreePoint(point, out Point nextPoint))
                            {
                                if (!_monstersVM.TryAddMonster(_level, new Vector2Int(point.X + i, point.Z)))
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        public void AddStartCoins() 
        {
//            scoreData.Score = _startCoins;
        }
    }
}