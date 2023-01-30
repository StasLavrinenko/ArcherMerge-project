using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MergeSystem.Cells;
using UnityEditor;

namespace MergeSystem
{
    [RequireComponent(typeof(PointsContainer))]
    public class MonstersContainer : MonoBehaviour
    {
        private PointsContainer _pointsContainer;
        private Dictionary<Point, Tower> _monsterMap;
        [SerializeField] private MonsterPoolScrObj _monsterPool;

        private MonstersVM _monstersVM;

        public Dictionary<Point, Tower> MonsterMap => _monsterMap;

        private void Awake()
        {
            _pointsContainer = GetComponent<PointsContainer>();
            _monsterMap = new Dictionary<Point, Tower>();
            _monstersVM = MonstersVM.Instance;

            _monstersVM.OnMonsterAdded += AddMonster;
            _monstersVM.OnMonsterRemoved += DeleteMonster;
        }

        private void Start() => CreateFromSave();

        private void OnDestroy()
        {
            _monstersVM.OnMonsterAdded -= AddMonster;
            _monstersVM.OnMonsterRemoved -= DeleteMonster;
        }
#if UNITY_EDITOR
        #region Comment when builing
        /// <summary>
        /// Comment Below when you are making Build
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                DeleteSaveSystem();
            }
        }

        private void DeleteSaveSystem()
        {
            if (Directory.Exists(Application.persistentDataPath + "/Saves/"))
            {
                Directory.Delete(Application.persistentDataPath + "/Saves/", true);
                FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/Saves/");
            }
        }
        #endregion
#endif

        private void AddMonster(MonsterSave save)
        {
            if (_pointsContainer.TryGetPoint(save.PointPos, out Point point))
            {
                TryCreateMonster(point, save);
            }
            else Debug.LogError("Point Container does not have point with id " + save.PointPos);
        }

        private void DeleteMonster(Vector2Int pos)
        {
            if (_pointsContainer.TryGetPoint(pos, out Point point))
            {
                if (_monsterMap.ContainsKey(point))
                {
                    Debug.Log(_monsterMap[point] + " DELETED");
                    Destroy(_monsterMap[point].gameObject);
                    _monsterMap.Remove(point);
                    point.IsBusy = false;
                }
            }
            else Debug.LogError("Point Container does not have point with id " + pos);
        }

        private void CreateFromSave()
        {
            if (_monstersVM.TryCreateMonstersFromSave(_monsterPool, out Tower[] monsters))
            {
                for (int i = 0; i < monsters.Length; i++)
                {
                    if (_pointsContainer.TryGetPoint(_monstersVM.Monsters[i].PointPos, out Point point))
                    {
                        _monsterMap.Add(point, monsters[i]);
                        monsters[i].transform.parent = point.transform;
                        monsters[i].transform.localPosition = Vector3.zero;
                        point.IsBusy = true;
                        point._containingTowerLevel = monsters[i].Level;
                    }
                }
            }
        }

        public bool TryCreateMonster(Point point, MonsterSave save)
        {

            if (_monsterPool.TryGetMonsterData(save.Level, out TowerData data))
            {
                Debug.Log(save.PointPos + "Created");
                Tower temp = Instantiate(data.Monster, point.transform.position, Quaternion.Euler(0, 0, 0));
                temp.transform.parent = point.transform;
                temp.Initialize(save.Level);
                _monsterMap.Add(point, temp);
                point.IsBusy = true;
                return true;
            }
            else
            {
                Debug.LogError("Monster Pool does not have monster of these " + save.Level);
                return false;
            }
        }
    }
}