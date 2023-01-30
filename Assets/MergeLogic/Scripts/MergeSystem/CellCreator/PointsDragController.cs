using UnityEngine;

namespace MergeSystem.Cells
{
    [RequireComponent(typeof(PointsContainer))]
    [RequireComponent(typeof(MonstersContainer))]
    public class PointsDragController : MonoBehaviour
    {        
        [Header("Dependencies")]
        [SerializeField] private MonsterPoolScrObj _monsterPool;
        [SerializeField] private LayerMask _monsterLayer;
        [SerializeField] private LayerMask _pointLayer;
        [SerializeField] private LayerMask _floorLayer;

        // Hide Dependencies //
        private MonstersVM _monstersVM;
        private PointsContainer _pointsContainer;

        // Privates //
        private Point _selectedPoint;
        private Tower _cursor;
        private bool _isDrag;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _monstersVM = MonstersVM.Instance;
            _pointsContainer = GetComponent<PointsContainer>();
            //_monsterLayer = LayerMask.GetMask("Monster");
            //_pointLayer = LayerMask.GetMask("Point");
            //_floorLayer = LayerMask.GetMask("Floor");
            if (!_monsterPool) Debug.LogError("Add MonsterPoolScrObj value to PointDragController! SROCHNA!");
        }

        private void Update()
        {
            if (Input.touchCount < 1) return;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) Touch();
            if (touch.phase == TouchPhase.Moved) Drag();        
            if (touch.phase == TouchPhase.Ended) EndTouch();
        }

        private void Touch() 
        {
            if (!_cursor)
            {
                Select();
            }
            else
            {
                FinishMove();
            }
        }

        private void Drag()
        {
            _isDrag = true; 
            if (_cursor)
            {
                CursorMove();
                //_cursor.StopAttack();
            }

        }

        private void EndTouch()
        {
            //if(_cursor)_cursor.StartAttack();

            if (_isDrag && _cursor) FinishMove();
            
            if (_selectedPoint != null) _selectedPoint.ResetLevel();
        }        

        private void Select()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _monsterLayer))
            {
                if (hit.transform.TryGetComponent(out _cursor) && hit.transform.parent.TryGetComponent(out _selectedPoint))
                {
                    _pointsContainer.HighLightPoints(_cursor, _selectedPoint.IndexPos);
                    CreateCursorTower(_selectedPoint.IndexPos, _selectedPoint.transform.position);
                }
            }
        }
        private void CreateCursorTower(Vector2Int id, Vector3 position)
        {
            if (_monstersVM.CheckMonsterByPos(id, out int index))
            {
                _monstersVM.TryCreateMonsterFromSave(index, _monsterPool, out _cursor);
                _cursor.transform.position = position;
                _monstersVM.RemoveMonsterFromSave(id);
            }
            else
            {
                _selectedPoint = null;
            }
         
        }
        
        public void CursorMove()
        {           
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _floorLayer))
                _cursor.transform.position = Vector3.Lerp(_cursor.transform.position, hit.point, Time.deltaTime * 100);
        }
        private void FinishMove()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _pointLayer))  // Hit Check.// Try Get Point.
            {
                if (hit.transform.TryGetComponent(out Point point))
                {
                    if (_monstersVM.TryAddMonster(new MonsterSave(_cursor.Level, point.IndexPos)))
                    {                    
                        if (point.IndexPos == _selectedPoint.IndexPos)
                            _monstersVM.TryAutoUpdateTower(new MonsterSave(_cursor.Level, point.IndexPos), out MonsterSave newSave);
                    }
                    else Back();
                }
                else Back();
            }
            else Back();

            Clear();
        }

        private void Back() => _monstersVM.TryAddMonster(_cursor.Level, _selectedPoint.IndexPos);

        private void Clear()
        {
            _pointsContainer.OffLights();
            Destroy(_cursor.gameObject);
            _cursor = null;
            _selectedPoint = null;
            _isDrag = false;
        }       
    }

    /*
    namespace MergeSystem.Cells.Old
    {
        
        public class PointsDragController : Monoehaviour
        {
            private Vector3 _mousePosition;
            [SerializeField] private Camera _camera;
            [SerializeField] private MonsterPoolScrObj _monsterPool;
            [SerializeField] private LayerMask _monsterLayer;
            [SerializeField] private LayerMask _pointLayer;
            [SerializeField] private LayerMask _floorLayer;

            private MonstersVM _monstersVM;
            private Point _oldPoint;
            private Point _selectedPoint;
            private PointsContainer _pointsContainer;

            //  private MonstersContainer _monstersContainer;
            // private PointsContainer _pointsContainer;

            private void Awake()
            {
                // _pointsContainer = GetComponent<PointsContainer>();
                // _monstersContainer = GetComponent<MonstersContainer>();
                _camera = Camera.main;
                _pointsContainer = GetComponent<PointsContainer>();
                _floorLayer = LayerMask.GetMask("Floor");
                _monsterLayer = LayerMask.GetMask("Monster");
                _pointLayer = LayerMask.GetMask("Point");
                _monstersVM = MonstersVM.Instance;
                if (!_monsterPool) Debug.LogError("Add MonsterPoolScrObj value to PointDragController! SROCHNA!");
            }

            private Tower _oldUnit;
            private Tower unitToDrag;
            private bool _isDrag;

            private void Update()
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        MouseDown();
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        if (!_isDrag) StartDrag();
                        else MouseDrag();
                    }
                    if (touch.phase == TouchPhase.Ended) MouseUp();
                }
            }

            private void MouseDown()
            {
                if (!_oldPoint) Select();
                else if (unitToDrag) Finish(unitToDrag);
            }

            private void Select()
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _monsterLayer))
                {
                    if (hit.transform.TryGetComponent(out _oldUnit))
                    {
                        if (hit.transform.parent.TryGetComponent(out _oldPoint))
                        {
                            _selectedPoint = _oldPoint;
                            _pointsContainer.HighLightPoints(_oldUnit);

                            Vector2Int pos = new Vector2Int(_oldPoint.X, _oldPoint.Z);
                            if (_monstersVM.CheckMonsterByPos(pos, out int index))
                            {
                                _monstersVM.TryCreateMonsterFromSave(index, _monsterPool, out unitToDrag);
                                unitToDrag.transform.position = _oldUnit.transform.position;
                                _monstersVM.RemoveMonsterFromSave(pos);
                                Debug.Log(unitToDrag.Level);
                            }
                            else
                            {
                                _oldPoint = null;
                                _selectedPoint = null;
                            }
                        }

                    }

                }
            }

            private void StartDrag()
            {
                _isDrag = true;
            }


            private void MouseDrag()
            {
                if (_isDrag) Mouse();
            }

            private void MouseUp()
            {
                if (_isDrag) Finish(unitToDrag);
                if (_selectedPoint != null) _selectedPoint.ResetLevel();
                _isDrag = false;

                _selectedPoint = null;
            }

            private void Finish(Tower unit)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _pointLayer))
                {
                    SetMonster(hit.transform.GetComponent<Point>(), unit);
                }
                else
                {
                    _monstersVM.TryAddMonster(unit.Level, new Vector2Int(_oldPoint.X, _oldPoint.Z));
                }
                _oldUnit = null;
                _oldPoint = null;
                unitToDrag = null;
                Destroy(unit.gameObject);
            }


            private void SetMonster(Point newPoint, Tower newUnit)
            {
                Debug.Log("Merge" + newUnit.Level);
                if (_monstersVM.TryAddMonster(newUnit.Level, new Vector2Int(newPoint.X, newPoint.Z)))
                {
                    Vector2Int pos = new Vector2Int(_oldPoint.X, _oldPoint.Z);
                    _monstersVM.RemoveMonsterFromSave(pos);
                }
                else _monstersVM.TryAddMonster(newUnit.Level, new Vector2Int(_oldPoint.X, _oldPoint.Z));
            }

            public void Mouse()
            {
                _selectedPoint = null;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _floorLayer))
                {
                    _mousePosition = hit.point;
                    unitToDrag.transform.position = Vector3.Lerp(unitToDrag.transform.position,
                       _mousePosition,
                        Time.deltaTime * 100);
                }
            }

        }
    }
        */
}