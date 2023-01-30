using UnityEngine;

public class MonstersData 
{
    private const string SAVE_FILE_NAME = "Monsters";
    private SaveSystem<SavePack> _saveSystem;

    public MonstersData()
    {
        _saveSystem = new SaveSystem<SavePack>(SAVE_FILE_NAME);
    }

    public MonsterSave[] MonsterSaves
    {
        get 
        {           
            _saveSystem.TryLoad(out SavePack value); 
            return value.Save; 
        }
        set
        {
            SavePack pack = new SavePack(value);
            _saveSystem.Save(pack);
        }
    }
}

[System.Serializable]
public struct SavePack
{
    public SavePack(MonsterSave[] save)
    {
        _save = save;
    }

    public MonsterSave[] Save => _save;
    [SerializeField] private MonsterSave[] _save;
}

[System.Serializable]
public struct MonsterSave
{
    public MonsterSave(int level, Vector2Int pos)
    {
        _level = level;
        _pointPos = pos;
       
    }

    public int Level => _level;
    [SerializeField] private int _level;

    public Vector2Int PointPos => _pointPos;
    [SerializeField] private Vector2Int _pointPos;
}