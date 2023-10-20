using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private List<UIPopup> _listPopup = new List<UIPopup>();

        private void Awake()
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

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

         public void Setup(PopupName popupName, Dictionary<string, object> customProperties = null)
        {
            UIPopup selectedPopup = _listPopup.Find(popup => popup.PopupName == popupName);
            if (selectedPopup != null) selectedPopup.Setup(customProperties);

        }

        public void ShowPopup(PopupName popupName, Dictionary<string, object> customProperties = null)
        {
            UIPopup selectedPopup = _listPopup.Find(popup => popup.PopupName == popupName);
            if (selectedPopup != null) selectedPopup.Show(customProperties);

        }

        public void HidePopup(PopupName popupName)
        {
            UIPopup selectedPopup = _listPopup.Find(popup => popup.PopupName == popupName);
            if (selectedPopup != null) selectedPopup.Hide();
        }


    }

}
