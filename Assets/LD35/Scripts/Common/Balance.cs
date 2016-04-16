using UnityEngine;
using System.Collections;

namespace LD35
{
    public class Balance : MonoSingleton<Balance>
    {
        [Header("Dogs properties")]
        public float DogMoveSpeed = 1f;
    }
}