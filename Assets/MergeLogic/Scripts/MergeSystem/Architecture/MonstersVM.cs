using System;
using UnityEngine;
using MergeSystem;

public class MonstersVM
{
    public event Action<Vector2Int> OnMonsterRemoved;
    public event Action<MonsterSave> OnMonsterAdded;
    

    public static MonstersVM Instance { get { return _instance == null ? new MonstersVM() : _instance; } }
    private static MonstersVM _instance;

    private MonstersModel _monstersModel;
    public MonstersModel MonsterModel => _monstersModel;
    private MonstersVM()
    {
        _monstersModel = new MonstersModel(this);
        _instance = this;
    }

    public MonsterSave[] Monsters => _monstersModel.Monsters;

    public bool GetMonsterSave(Vector2Int pos, out MonsterSave monster) => _monstersModel.GetMonsterSave(pos, out monster);
    public bool CheckMonsterByPos(Vector2Int pos, out int index) => _monstersModel.CheckMonsterByPos(pos, out index);

    public MonsterSave? RemoveMonsterFromSave(Vector2Int pos)
    {
        MonsterSave? temp = _monstersModel.RemoveMonsterFromSave(pos);
        OnMonsterRemoved?.Invoke(pos);
        return temp;
    }

    public bool TryAddMonster(MonsterSave save, MonsterPoolScrObj pool, out Tower obj)
        => TryAddMonster(save.Level, save.PointPos, pool, out obj);
    public bool TryAddMonster(int level, Vector2Int pos, MonsterPoolScrObj pool, out Tower obj)
    {
        obj = null;
        if (_monstersModel.TryAddMonster(level, pos, pool, out obj))
            OnMonsterAdded?.Invoke(new MonsterSave(level, pos));
        return obj != null;
    }

    public bool TryAddMonster(MonsterSave save) => TryAddMonster(save.Level, save.PointPos);
    public bool TryAddMonster(int level, Vector2Int pos)
    {
        if (_monstersModel.TryAddMonster(level, pos))
        {
            if (CheckMonsterByPos(pos, out int index))
                OnMonsterAdded?.Invoke(Monsters[index]);
            
            return true;
        }
        return false;
    }
    public bool TryAutoUpdateTower(MonsterSave save, out MonsterSave newSave)
    {
        if (_monstersModel.TryAutoUpdateTower(save, out newSave))
        {
            OnMonsterAdded?.Invoke(newSave);
            return true;
        }
        return false;
    }

    public bool TryCreateMonstersFromSave(MonsterPoolScrObj data, out Tower[] monsters)
        => _monstersModel.TryCreateMonstersFromSave(data, out monsters);
    
    public bool TryCreateMonstersFromSave(MonsterPoolScrObj data)
        => _monstersModel.TryCreateMonstersFromSave(data);

    public bool TryCreateMonsterFromSave(int index, MonsterPoolScrObj data, out Tower monster)
        => _monstersModel.TryCreateMonsterFromSave(index, data, out monster);
}