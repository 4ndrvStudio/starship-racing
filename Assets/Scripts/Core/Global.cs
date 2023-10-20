using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class Global : MonoBehaviour
    {
        public static Global Instance;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        void Start() => DontDestroyOnLoad(gameObject);
    }
}
