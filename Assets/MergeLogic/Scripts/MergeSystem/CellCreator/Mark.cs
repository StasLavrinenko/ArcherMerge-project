using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeSystem
{
    public class Mark : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _margeMonster;
        [SerializeField] private ParticleSystem _newMonster;

        public ParticleSystem MargeMonster => _margeMonster;
        public ParticleSystem NewMonster => _newMonster;
    }
}
