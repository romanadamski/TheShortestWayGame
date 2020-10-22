using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Controllers
{
    public class LinkController : MonoBehaviour, IPointerClickHandler
    {
        TextMeshProUGUI TextMeshProUGUI;
        private void Start()
        {
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(TextMeshProUGUI.text);
        }
        public void SelectLink()
        {
            TextMeshProUGUI.fontStyle = FontStyles.Underline;
            TextMeshProUGUI.color = Color.blue;
        }
        public void DeselectLink()
        {
            TextMeshProUGUI.fontStyle = FontStyles.Normal;
            TextMeshProUGUI.color = Color.white;
        }
    }
}
