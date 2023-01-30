using UnityEngine;

namespace MergeSystem.Cells
{
    public class PointGenerator : MonoBehaviour
    {
        [SerializeField] private int _xCount;
        [SerializeField] private int _zCount;
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Vector3 _pointsOffset;
        [SerializeField] private Transform _parentForPoints;
        [SerializeField] private Transform[] _SetPoints;
        [SerializeField] private bool _addHalf;
        [SerializeField] private ParticleSystem _particle;

        private static Point[,] _doublePoints;

        public void Awake()
        {
            DoubleArrayGenerator();
            InitilizePoints();
        }

        private void DoubleArrayGenerator()
        {
            DeletePoints();

            int maxCount = _xCount * _zCount;
            _doublePoints = new Point[_xCount, _zCount];

            for (int x = 0; x < _xCount; x++)
            {
                for (int z = 0; z < _zCount; z++)
                {
                    Transform temp = Instantiate(_cellPrefab).transform;
                    temp.parent = _parentForPoints ? _parentForPoints : transform;
                    temp.localPosition = Vector3.zero;

                    Vector3 position = new Vector3((-_xCount * _pointsOffset.x) / 2f, 0, (-_zCount * _pointsOffset.z) / 2f);
                    position.x += (x * _pointsOffset.x);
                    position.y += _pointsOffset.y;
                    position.z += (z * _pointsOffset.z);
                    temp.gameObject.AddComponent<BoxCollider>().size = new Vector3(2, 1.5f, 2);


                    if (_addHalf)
                    {
                        position.x += _pointsOffset.x / 2f;
                        position.z += _pointsOffset.z / 2f;
                    }

                    temp.localPosition = position;

                    _doublePoints[x, z] = temp.GetComponent<Point>();

                }
            }
            for (int i = 0; i < _SetPoints.Length; i++)
            {
                if (_SetPoints[i].TryGetComponent(out ISetPoints points))
                    points.SetPointArray(_doublePoints);
            }
        }



        private void InitilizePoints()
        {
            if (_doublePoints != null)
            {
                for (int x = 0; x < _doublePoints.GetLength(0); x++)
                {
                    for (int y = 0; y < _doublePoints.GetLength(1); y++)
                    {
                        if (_doublePoints[x, y])
                            _doublePoints[x, y]
                                .Initialize(
                                    y + 1 < _doublePoints.GetLength(1) ? _doublePoints[x, y + 1] : null,
                                    y - 1 >= 0 ? _doublePoints[x, y - 1] : null,
                                    x + 1 < _doublePoints.GetLength(0) ? _doublePoints[x + 1, y] : null,
                                    x - 1 >= 0 ? _doublePoints[x - 1, y] : null,
                                    x,
                                    y
                                );
                    }
                }
            }
        }
        public void DeletePoints()
        {
            if (_doublePoints != null)
            {
                for (int x = 0; x < _doublePoints.GetLength(0); x++)
                {
                    for (int y = 0; y < _doublePoints.GetLength(1); y++)
                    {
                        if (_doublePoints[x, y]) DestroyImmediate(_doublePoints[x, y].gameObject);
                    }
                }
                _doublePoints = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_doublePoints != null)
            {
                for (int x = 0; x < _doublePoints.GetLength(0); x++)
                {
                    for (int y = 0; y < _doublePoints.GetLength(1); y++)
                    {
                        if (_doublePoints[x, y])
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawSphere(_doublePoints[x, y].transform.position, 0.2f);
                        }
                    }
                }
            }
        }


    }
    public interface ISetPoints
    {
        public void SetPointArray(Point[,] points);
    }
}