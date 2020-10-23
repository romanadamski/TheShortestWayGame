using Assets.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Controllers
{
    public class TheGamePanelController : MonoBehaviour, IPointerDownHandler
    {
        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;

        public void OnPointerDown(PointerEventData eventData)
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                MainManager.CameraManager.SetDefaultCameraLocation(MainManager.MapController.ActiveMap);
            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
