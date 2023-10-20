using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR.UI
{
    public class Popup_OpenAnimation : UIPopup
    {
        [SerializeField] private GameObject _purple;
        [SerializeField] private GameObject _red;
        [SerializeField] private GameObject _gold;


        public override void Show(Dictionary<string, object> customProperties)
        {
            base.Show(customProperties);
            string bagType = customProperties["BagType"].ToString();
            switch (bagType) {
                case "1": 
                    DisableAllBag();
                    _purple.gameObject.SetActive(true); 
                    break;
                case "2": 
                    DisableAllBag();
                    _red.gameObject.SetActive(true); 
                    break;
                case "3": 
                    DisableAllBag();
                    _gold.gameObject.SetActive(true); 
                    break;
            }
        }
        
        public void NextState() {
            Hide();
        }

        private void DisableAllBag() {
            _purple.gameObject.SetActive(false);
            _red.gameObject.SetActive(false);
            _gold.gameObject.SetActive(false);  
        }
    }

}
