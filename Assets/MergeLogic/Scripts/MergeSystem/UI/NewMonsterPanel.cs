using UnityEngine;
using UnityEngine.UI;
using MergeSystem;
//using DG.Tweening;
using TMPro;

public class NewMonsterPanel : MonoBehaviour
{
    [SerializeField] private MonsterPoolScrObj _monsterPool;
    [SerializeField] private Transform _spawnPoint;

    private TowerData _monsterData;
    private GameObject _thisPanel;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _strengthText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _nameText;

   // private Tower _activeMonster;
    private int _rangedLevelIndex;
    private int _meleeLevelIndex;
    private MonstersModel _monsterModel;
    private void Awake()
    {
        _monsterModel = MonstersVM.Instance.MonsterModel;
        _monsterModel.OnMonsterMerged += SetPanelSettings;
        Access();
    }

    private void OnDestroy()
    {
        _monsterModel.OnMonsterMerged -= SetPanelSettings;
    }
  

    public void SetPanelSettings(MonsterSave monsterSave)
    {
        TowerData monsterData;
        _monsterPool.TryGetMonsterData(monsterSave.Level, out monsterData);

       // if (_activeMonster)
         //   Destroy(_activeMonster.gameObject);

      //  _activeMonster = Instantiate(monsterData.Monster, _spawnPoint);

      //  DOVirtual.DelayedCall(1f, () => 
     ////   {
     //       if (MeleeMonsterSettings(monsterSave, _meleeLevelIndex)) _meleeLevelIndex++;            
     //       SaveOpenedMonsters();
     //   });
    }

    private bool MeleeMonsterSettings(MonsterSave monsterSave, int savedIndexLevel)
    {
        if(CheckLevel(savedIndexLevel, monsterSave.Level))
        {
            if(_monsterPool.TryGetMonsterData(monsterSave.Level, out  TowerData monsterData))
            _monsterData = monsterData;
///            ChangeMonsterStatsUI(_monsterData.Name, _monsterData.MaxHealth, _monsterData.Damage, monsterSave.Level);
            _thisPanel.SetActive(true);
            return true;
            
        }
        return false;
    }


    private void ChangeMonsterStatsUI(string name, float health, float strength, int level)
    {
        _healthText.text = health.ToString();
        _strengthText.text = strength.ToString();
        _levelText.text = "Level " + (level+1).ToString();
        _nameText.text = name;
        //TODO: Image and name

    }

    private bool CheckLevel(int savedLevel, int level)
    {
        if (savedLevel < level) return true;
        else return false;
    }
    private void Access()
    {
        //_rangedLevelIndex = PlayerPrefs.GetInt(Constants.OPENED_RANGED_MONSTER_LEVEL, 0);
        //_meleeLevelIndex = PlayerPrefs.GetInt(Constants.OPENED_MELEE_MONSTER_LEVEL, 0);
        _thisPanel = this.gameObject;
        _thisPanel.SetActive(false);
    }
    private void SaveOpenedMonsters()
    {
        //PlayerPrefs.SetInt(Constants.OPENED_MELEE_MONSTER_LEVEL, _meleeLevelIndex);
        //PlayerPrefs.SetInt(Constants.OPENED_RANGED_MONSTER_LEVEL, _rangedLevelIndex);
        PlayerPrefs.Save();
    }
}
