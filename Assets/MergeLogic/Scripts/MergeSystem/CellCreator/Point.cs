using UnityEngine;

namespace MergeSystem.Cells
{
    public class Point : MonoBehaviour
    {
        [SerializeField] private Point _forwardPoint;
        [SerializeField] private Point _backPoint;
        [SerializeField] private Point _leftPoint;
        [SerializeField] private Point _rightPoint;
        [SerializeField] private int _x;
        [SerializeField] private int _z;

        public Vector2Int IndexPos => _indexPos;
        private Vector2Int _indexPos;

        public SpriteRenderer Render => _renderer;
        private SpriteRenderer _renderer;
        // private ParticleSystem _margeMonster;
        // private ParticleSystem _newMonster;

        public int _containingTowerLevel = -1;


        public bool IsBusy { get; set; }

        private void Awake()
        {
            MonstersVM.Instance.MonsterModel.OnMonsterMerged += PlayParticle;
            _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }


        private void OnDisable()
        {
            MonstersVM.Instance.MonsterModel.OnMonsterMerged -= PlayParticle;
        }
        public void Initialize(Point forward, Point back, Point right, Point left, int x, int z)
        {
            _forwardPoint = forward;
            _backPoint = back;
            _rightPoint = right;
            _leftPoint = left;
            _x = x;
            _z = z;
            _indexPos = new Vector2Int(_x, _z);
            // var mark = GetComponentInChildren<Mark>();
            //  _margeMonster = mark.MargeMonster;
            // _newMonster = mark.NewMonster;
        }
        //MonstersVM.Instance.MonsterModel
        public void PlayParticle(MonsterSave save)
        {
            if(save.PointPos.x == _x && save.PointPos.y == _z)
            {
                _containingTowerLevel = save.Level;
                //_margeMonster.Play();
            }
        }

        public void NewMonsterEffect(int level) 
        {
            _containingTowerLevel = level;
            //  _newMonster.Play();
        }

        public void ResetLevel()
        {
            _containingTowerLevel = -1;
        }

        public int X => _x;
        public int Z => _z;
        public bool TryGetForwardPoint(out Point point) { return point = _forwardPoint; }
        public bool TryGetBackPoint(out Point point) { return point = _backPoint; }
        public bool TryGetLeftPoint(out Point point) { return point = _leftPoint; }
        public bool TryGetRightPoint(out Point point) { return point = _rightPoint; }
    }
}