using UnityEngine;
using System.Collections;

namespace LD35
{
    public class Balance : MonoSingleton<Balance>
    {
        [Header("Dogs Properties")]
        public float DogMoveSpeed = 1f;
        public float DogWaitBeforeAttackShepherd = 1f;

        [Header("Man Traits")]
        public float ManSpeed = 15f;
        public float ManScareRadius = 2f;
        public float ManScariness = 0.5f;

        [Header("Wolf Traits")]
        public float WolfSpeed = 45f;
        public float WolfScareRadius = 6f;
        public float WolfScariness = 2f;
    }
}
