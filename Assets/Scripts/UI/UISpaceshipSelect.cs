using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class UISpaceshipSelect : MonoBehaviour
    {
        public int TypeSelect;
        public GameObject ActiveOb;
        public GameObject DeActiveOb;

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
