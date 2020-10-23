using Assets.ApplicationObjects;
using Assets.Enums;
using Assets.Managers;
using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    float speed = 5;
    float startCameraLocationY;
    float startCameraLocationZ;

    float startCameraRotationX;
    float startCameraRotationY;
    float startCameraRotationZ;
    GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.Find("MainCamera");
        startCameraLocationY = MainCamera.transform.localPosition.y;
        startCameraLocationZ = MainCamera.transform.localPosition.z;

        startCameraRotationX = MainCamera.transform.rotation.eulerAngles.x;
        startCameraRotationY = MainCamera.transform.rotation.eulerAngles.y;
        startCameraRotationZ = MainCamera.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(MainManager.GameMode == GameModeEnum.THE_GAME)
        {
            if (Input.GetMouseButton(0))
            {
                MainCamera.transform.Translate(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            }
            else if (Input.GetMouseButton(1))
            {
                MainCamera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
                var X = MainCamera.transform.rotation.eulerAngles.x;
                var Y = MainCamera.transform.rotation.eulerAngles.y;
                MainCamera.transform.rotation = Quaternion.Euler(X, Y, 0);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                MainCamera.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * speed);
            }
        }
    }
    public void SetDefaultCameraLocation(MapObject mapObject)
    {
        MainCamera.transform.localPosition = new Vector3(mapObject.MapSize / 2, startCameraLocationY, mapObject.MapSize);
        MainCamera.transform.localRotation = Quaternion.Euler(new Vector3(startCameraRotationX, startCameraRotationY, startCameraRotationZ));
    }
}
