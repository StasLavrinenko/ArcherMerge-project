using System.Collections.Generic;
using UnityEngine;
using MergeSystem;
using System;

public class MonstersModel
{
    public MonsterSave[] Monsters => _monsters;
    private MonsterSave[] _monsters;
    private MonsterSave[] _data
    {
        get { return _monstersData.MonsterSaves; }
        set { _monstersData.MonsterSaves = value; }
    }

    
    private MonstersData _monstersData;
    private MonstersVM _monstersVM;
    public event Action<MonsterSave> OnMonsterMerged;

    public MonstersModel(MonstersVM monstersVM)
    {
        _monstersData = new MonstersData();
        _monsters = _data;
        _monstersVM = monstersVM;
    }

    public MonsterSave? RemoveMonsterFromSave(Vector2Int pos)
    {
        if (CheckMonsterByPos(pos, out int index))
        {
            MonsterSave temp = _monsters[index];
            Delete(index);
            return temp;
        }
        return null;
    }

    public bool TryAddMonster(int level, Vector2Int pos, MonsterPoolScrObj pool, out Tower obj)
    {
        obj = null;
        if (!TryAddMonster(level, pos))
            return false;
        if (!TryCreateMonsterFromSave(_monsters.Length - 1, pool, out obj))
            RemoveMonsterFromSave(pos);

        return obj != null;
    }
    public bool TryAddMonster(int level, Vector2Int pos)
    {
        if (CheckMonsterByPos(pos, out int index))
        {
            if (TryMerge(level, pos, _monsters[index], out MonsterSave merged))
            {
                AddMonster(merged.Level, merged.PointPos);
                OnMonsterMerged?.Invoke(merged);
                return true;
            }
            else return false; 
        }
        AddMonster(level, pos);
        return true;
    }

    public bool TryAutoUpdateTower(MonsterSave save, out MonsterSave newSave)
    {
        //return false;
        newSave = new MonsterSave(save.Level + 1, save.PointPos);
        if (!CheckMonsterByPos(save.PointPos, out int index)) return false;
        for (int i = 0; i < _monsters.Length; i++)
        {
            if (_monsters[i].PointPos != save.PointPos && _monsters[i].Level == save.Level)
            {
                _monstersVM.RemoveMonsterFromSave(save.PointPos);
                _monstersVM.RemoveMonsterFromSave(_monsters[i].PointPos);               
                if (newSave.Level < TowerData.MaxLevel) AddMonster(newSave.Level, newSave.PointPos);
                
                return true;
            }            
        }
        return false;
    }
    
    public bool GetMonsterSave(Vector2Int pos, out MonsterSave monster)
    {
        monster = new MonsterSave();
        if (CheckMonsterByPos(pos, out int index))
        {
            monster = _monsters[index];
            return true;
        }
        return false;
    }

    public bool TryCreateMonstersFromSave(MonsterPoolScrObj data, out Tower[] monsters)
    {
        monsters = null;
        if (_monsters == null) return false;
        List<Tower> temp = new List<Tower>();
        for (int i = 0; i < _monsters.Length; i++)
        {
            if (TryCreateMonsterFromSave(i, data, out Tower monster))
                temp.Add(monster);
        }
        monsters = temp.ToArray();
        return true;
    }
    public bool TryCreateMonstersFromSave(MonsterPoolScrObj data)
    {
        if (_monsters == null) return false;
        //List<Monster> temp = new List<Monster>();
        //for (int i = 0; i < _monsters.Length; i++)
        //{
        //    if (TryCreateMonsterFromSave(i, data, out Monster monster))
        //        temp.Add(monster);
        //}
        //monsters = temp.ToArray();
        return true;
    }
    public bool TryCreateMonsterFromSave(int index, MonsterPoolScrObj data, out Tower monster)
    {
        monster = null;
        if (_monsters == null) return false;
        if (_monsters.Length <= index) return false;
        if (data.TryGetMonsterData(_monsters[index].Level, out TowerData monsterData))
        {
            monster = GameObject.Instantiate(monsterData.Monster, default, Quaternion.Euler(0,0,0));
            monster.Initialize(_monsters[index].Level);
            return true;
        }
        return false;
    } 

    public bool CheckMonsterByPos(Vector2Int pos, out int index)
    {
        index = 0;
       
        if (_monsters == null) return false;

        for (int i = 0; i < _monsters.Length; i++)
        {
            if (_monsters[i].PointPos == pos)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    private void CheckMonsterByLevel(int level)
    {
        if (_monsters == null) return;

        for (int i = 0; i < _monsters.Length; i++)
        {
            if (_monsters[i].Level == level)
            {
               // _monsters[i].PointPos
            }
        }
    }


    private void AddMonster(int level, Vector2Int pos)
    {
        MonsterSave[] temp = new MonsterSave[_monsters == null ? 1 : _monsters.Length + 1];
        for (int i = 0; i < temp.Length - 1; i++)
        {
            temp[i] = _monsters[i];
        }
        temp[temp.Length - 1] = new MonsterSave(level, pos);
        _monsters = temp;
        _data = _monsters;
    }

    private bool TryMerge(int level, Vector2Int pos, MonsterSave oldMonster, out MonsterSave merged)
    {
        merged = default(MonsterSave);
        if (level != oldMonster.Level) return false;
        if (level >= TowerData.MaxLevel) return false;

        merged = new MonsterSave(level + 1, pos);
        _monstersVM.RemoveMonsterFromSave(pos);
        return true;
    }

    private void Delete(int index)
    {
        if (_monsters == null) return;

        MonsterSave[] temp = new MonsterSave[_monsters.Length - 1];
        for (int i = 0; i < _monsters.Length; i++)
        {
            if (i == index) continue;
            temp[i >= index ? i - 1 : i] = _monsters[i];
        }
        _monsters = temp;
        _data = _monsters;
    }
}