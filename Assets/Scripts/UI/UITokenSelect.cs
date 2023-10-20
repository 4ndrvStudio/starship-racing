using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR.UI
{
    public class UITokenSelect : MonoBehaviour
    {
        public CurrencyType CurrencyType;
        public GameObject ActiveOb;
        public GameObject DeActiveOb;
        public float Price;

        public void Select()
        {
            ActiveOb.SetActive(true);
            DeActiveOb.SetActive(false);
        }
        public void DeSelect()
        {
            ActiveOb.SetActive(false);
            DeActiveOb.SetActive(true);
        }

    }

}
