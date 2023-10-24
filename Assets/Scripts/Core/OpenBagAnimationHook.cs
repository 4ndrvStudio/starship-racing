using System.Collections;
using System.Collections.Generic;
using SR.UI;
using UnityEngine;

namespace SR
{
    public class OpenBagAnimationHook : MonoBehaviour
    {
        [SerializeField] private Popup_OpenAnimation popup_OpenAnimation;
        public void OnEndAnimation()
        {
            popup_OpenAnimation.NextState();
        }
    }
}
