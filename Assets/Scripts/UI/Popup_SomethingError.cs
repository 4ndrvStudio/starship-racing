using System.Collections;
using System.Collections.Generic;
using SR.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SR
{
    public class Popup_SomethingError : UIPopup
    {
        [SerializeField] private Button _btnOk;
        // Start is called before the first frame update
        void Start()
        {
            _btnOk.onClick.AddListener(Hide);
        }

    }
}
