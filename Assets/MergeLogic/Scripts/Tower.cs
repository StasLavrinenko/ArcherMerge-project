using System;
using UnityEngine;

namespace MergeSystem
{
    public class Tower : MonoBehaviour
    {
        public bool IsDrag { get; private set; }
       // private MonoAttackGun _attackGun;
        public int Level => _level;
        private int _level;

        private void Awake()
        {
            //   _attackGun = GetComponent<MonoAttackGun>();
        }

        public void Initialize(int level)
        {
            _level = level;
        }
        
    }
}