using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this);
            } else
            {
                Instance = this; 
            }

        }

        private void OnDestroy()
        {
            if(Instance == this)
                 Instance = null;
        }


    }

}
