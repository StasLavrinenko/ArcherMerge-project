using UnityEngine;
using UnityEngine.UI;
using MergeSystem.Cells;
//using CoreHyperCasual;
//using Gameplay.Core;

namespace MergeSystem.UI
{
    [RequireComponent(typeof(Button))]
    public class ShopButton : MonoBehaviour
    {
        //[SerializeField] private ScoreData scoreData;

        [Range(0, 9)] [SerializeField] private int _level;

        [SerializeField] private MonsterPoolScrObj _monsterPool;
        [SerializeField] private PointsContainer _pointsContainer;
        private Button _buyButton;
        private Text _priceText;

        private int _monsterPrice = 10;
        private MonstersVM _monstersVM;

        private void Awake()
        {
            _buyButton = GetComponent<Button>();
            _buyButton.onClick.AddListener(OnClicked);
            _priceText = _buyButton.gameObject.GetComponentInChildren<Text>();
            _pointsContainer = _pointsContainer ? _pointsContainer : FindObjectOfType<PointsContainer>();
            _monstersVM = MonstersVM.Instance;
           // _monsterPrice = PlayerPrefs.GetInt(Constants.MONEY_FOR_BULD_TOWER, 10);
            _priceText.text = _monsterPrice.ToString();

        }


        private void IncreasePrice()
        {
            _monsterPrice++;
            _priceText.text = _monsterPrice.ToString();
          //  PlayerPrefs.SetInt(Constants.MONEY_FOR_BULD_TOWER, _monsterPrice);
            PlayerPrefs.Save();
        }

        public void OnClicked()
        {
            if (!_monsterPool) 
                return;

           // if (scoreData.Score < _monsterPrice)
               // return;

            if (_pointsContainer.TryGetNextFreePoint(out Point point))
            {
                point.NewMonsterEffect(_level);
            //    scoreData.Score -= _monsterPrice;
                IncreasePrice();
                if (!_monstersVM.TryAddMonster(_level, new Vector2Int(point.X, point.Z)))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (_pointsContainer.TryGetNextFreePoint(point, out Point nextPoint))
                        {
                            if (!_monstersVM.TryAddMonster(_level, new Vector2Int(point.X, point.Z)))
                                continue;
                        }
                    }
                }
            }
        }
    }
}