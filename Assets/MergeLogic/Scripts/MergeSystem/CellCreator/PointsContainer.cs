using System.Collections.Generic;
using UnityEngine;

namespace MergeSystem.Cells
{
    public class PointsContainer : MonoBehaviour, ISetPoints
    {
        [SerializeField] private Transform _markPrefab;
        [SerializeField] private Vector3 _markOffset;

        [SerializeField] private Point[,] _points;

        private Renderer _pointRenderer;
        private Color _simpleColor;
        private Color _highliteColor;

        private MonstersVM _monstersVM;

        private void Start()
        {
            _monstersVM = MonstersVM.Instance;
            _simpleColor = Color.white;
            _highliteColor = new Color(0,1,0,1);
        }

        public bool TryGetNextFreePoint(Point point, out Point nextPoint)
        {
            nextPoint = null;
            for (int x = point.X; x < _points.GetLength(0); x++)
            {
                for (int z = point.Z; z < _points.GetLength(1); z++)
                {
                    if (_points[x, z].IsBusy) continue;
                    else
                    {
                        nextPoint = _points[x, z];
                        return true;
                    }
                }
            }
            for (int x = 0; x < point.X; x++)
            {
                for (int z = 0; z < point.Z; z++)
                {
                    if (_points[x, z].IsBusy) continue;
                    else
                    {
                        nextPoint = _points[x, z];
                        return true;
                    }
                }
            }
            return false;
        }
        public bool TryGetNextFreePoint(out Point point)
        {
            point = null;
            for (int x = 0; x < _points.GetLength(0); x++)
            {
                for (int z = 0; z < _points.GetLength(1); z++)
                {
                    if (_points[x, z].IsBusy) continue;
                    else
                    {
                        point = _points[x, z];
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetPoint(Vector2Int pos, out Point point) => TryGetPoint(pos.x, pos.y, out point);
        public bool TryGetPoint(int x, int z, out Point point)
        {
            point = null;
            if (_points.GetLength(0) > x || _points.GetLength(1) > z)
            {
                point = _points[x, z];
            }
            return point != null;
        }

        private Stack<Point> lights;

        public void HighLightPoints(Tower tower, Vector2Int currentId)
        {
            lights = new Stack<Point>();

            for (int x = 0; x < _points.GetLength(0); x++)
            {
                for (int y = 0; y < _points.GetLength(1); y++)
                {
                    if (_monstersVM.GetMonsterSave(new Vector2Int(x, y), out MonsterSave monsterSave))
                        if (monsterSave.Level == tower.Level)
                        {
                            if (currentId == _points[x, y].IndexPos) continue;
                            //lights.Push(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                            //lights.Peek().transform.SetParent(_points[x, y].transform);
                            //lights.Peek().transform.localPosition = new Vector3(0, 2.5f, 0);
                            lights.Push(_points[x, y]);
                            lights.Peek().Render.material.color = _highliteColor;
                            //Debug.Log(x + " " + y + " HighLighted");//TODO: highLight
                        }
                }
            }
        }

        public void OffLights()
        {
            if (lights != null)
            {
                while (lights.Count > 0)
                {
                    lights.Pop().Render.material.color = _simpleColor;                    
                }
                lights = null;
            }
        }

        public void SetPointArray(Point[,] points)
        {
            _points = points;
            //InitializePoints();
        }
        private void InitializePoints()
        {
            if (_points == null) return;

            for (int x = 0; x < _points.GetLength(0); x++)
            {
                for (int z = 0; z < _points.GetLength(1); z++)
                {
                    Transform temp = Instantiate(_markPrefab, _points[x, z].transform);
                    temp.position += _markOffset;
                    _points[x, z].gameObject.AddComponent<BoxCollider>().size = new Vector3(1,1.5f,1);
                    _points[x, z].gameObject.layer = _markPrefab.gameObject.layer;
                }
            }
        }
    }
}