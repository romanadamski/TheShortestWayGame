﻿using Assets.ApplicationObjects;
using Assets.Models;
using Assets.IOData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.Controllers;
using Assets.Enums;

namespace Assets.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        GameObject MapSizeInputField;
        GameObject ObstaclesCountInputField;
        GameObject MainMenuPanel;
        GameObject TheGamePanel;
        GameObject SavedMapsDropdown;
        GameObject LoadMapButton;
        GameObject InputMapNameButton;
        GameObject MapNameInputField;
        GameObject InputMapNamePanel;
        GameObject FindShortestWayButton;
        GameObject AlgorithmsDropdown;
        GameObject HelpPanel;
        GameObject CloseMenuButton;
        GameObject ClearPathButton;
        GameObject MessagePanel;

        TMP_InputField MapSizeInputField_InputField;
        TMP_InputField ObstaclesCountInputField_InputField;
        TMP_InputField MapNameInputField_InputField;
        TMP_Dropdown SavedMapsDropdown_Dropdown;
        TMP_Dropdown AlgorithmsDropdown_Dropdown;
        TextMeshProUGUI MessagePanel_MessageText;
        private void Awake()
        {
            GetGameObjects();
            SetMenuActive();
            SetObjects();
        }
        // Start is called before the first frame update
        void Start()
        {
            CheckLoadedMaps();
            SetDropdownDefaultValues();
        }
        public void ShowMessage(string message)
        {
            SetMessagePanelActive();
            MessagePanel_MessageText.text = message;
        }
        public void CloseMessagePanelButton_OnClick()
        {
            MessagePanel_MessageText.text = "";
            MessagePanel.SetActive(false);
        }
        private void SetDropdownDefaultValues()
        {
            SetDropdownValue(SavedMapsDropdown_Dropdown, 0);
            SetDropdownValue(AlgorithmsDropdown_Dropdown, 0);
        }

        private void CheckLoadedMaps()
        {
            if (SavedMapsDropdown_Dropdown.options.Count == 0)
            {
                SetButtonEnable(LoadMapButton, false);
            }
            else
            {
                LoadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            }
        }
        void SetDropdownValue(TMP_Dropdown dropdown, int index)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
        private void GetGameObjects()
        {
            MapSizeInputField = GameObject.Find("MapSizeInputFieldTMP");
            ObstaclesCountInputField = GameObject.Find("ObstaclesCountInputFieldTMP");
            MainMenuPanel = GameObject.Find("MainMenuPanel");
            TheGamePanel = GameObject.Find("TheGamePanel");
            HelpPanel = GameObject.Find("HelpPanel");
            InputMapNamePanel = GameObject.Find("InputMapNamePanel");
            SavedMapsDropdown = GameObject.Find("SavedMapsDropdown");
            LoadMapButton = GameObject.Find("LoadMapButton");
            InputMapNameButton = GameObject.Find("InputMapNameButton");
            MapNameInputField = GameObject.Find("MapNameInputField");
            FindShortestWayButton = GameObject.Find("FindShortestWayButton");
            AlgorithmsDropdown = GameObject.Find("AlgorithmsDropdown");
            CloseMenuButton = GameObject.Find("CloseMenuButton");
            ClearPathButton = GameObject.Find("ClearPathButton");
            MessagePanel = GameObject.Find("MessagePanel");
        }

        private void SetObjects()
        {
            MapSizeInputField_InputField = MapSizeInputField.GetComponent<TMP_InputField>();
            ObstaclesCountInputField_InputField = ObstaclesCountInputField.GetComponent<TMP_InputField>();
            MapNameInputField_InputField = MapNameInputField.GetComponent<TMP_InputField>();
            SavedMapsDropdown_Dropdown = SavedMapsDropdown.GetComponent<TMP_Dropdown>();
            AlgorithmsDropdown_Dropdown = AlgorithmsDropdown.GetComponent<TMP_Dropdown>();
            MessagePanel_MessageText = MessagePanel.GetComponentInChildren<TextMeshProUGUI>();
            MainManager.MapController = new MapController();
            SavedMapsDropdown_Dropdown.onValueChanged.AddListener(delegate
            {
                SavedMapsDropdown_OnValueChanged(SavedMapsDropdown_Dropdown);
            });
            AlgorithmsDropdown_Dropdown.onValueChanged.AddListener(delegate
            {
                AlgorithmsDropdown_OnValueChanged(AlgorithmsDropdown_Dropdown);
            });
            LoadMapsToMapDropdown();
            LoadAlgorithmsToAlgorithimDropdown();
            SetButtonEnable(InputMapNameButton, false);
            SetButtonEnable(FindShortestWayButton, false);
            SetButtonEnable(ClearPathButton, false);
            SetButtonEnable(CloseMenuButton, false);
            MainManager.AlgothitmEnum = (AlgothitmEnum)AlgorithmsDropdown_Dropdown.value;
        }

        private void LoadAlgorithmsToAlgorithimDropdown()
        {
            AlgorithmsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(AlgothitmEnum.DIJKSTRA.ToString()));
            AlgorithmsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(AlgothitmEnum.BELLMAN_FORD.ToString()));
        }

        private void AlgorithmsDropdown_OnValueChanged(TMP_Dropdown algorithmsDropdown_Dropdown)
        {
            MainManager.AlgothitmEnum = (AlgothitmEnum)algorithmsDropdown_Dropdown.value;
        }

        private void LoadMapsToMapDropdown()
        {
            foreach(var map in MainManager.MapController.SavedMaps)
            {
                if(map != null)
                    SavedMapsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(map.Name));
            }
        }
        void SavedMapsDropdown_OnValueChanged(TMP_Dropdown change)
        {
            LoadMapToObject(change.options[change.value].text);
        }

        private void LoadMapToObject(string mapName)
        {
            MainManager.MapController.LoadMapByName(mapName);
            MapSizeInputField_InputField.text = MainManager.MapController.LoadedMap.MapSize.ToString();
            ObstaclesCountInputField_InputField.text = MainManager.MapController.LoadedMap.ObstacleCount.ToString();
        }

        private void SetButtonEnable(GameObject button, bool active)
        {
            button.GetComponent<Button>().enabled = active;
            if(active)
                button.GetComponentInChildren<TMP_Text>().color = new Color(0, 0, 0, 1);
            else
                button.GetComponentInChildren<TMP_Text>().color = new Color(0, 0, 0, 0.3f);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void StartButton_OnClick()
        {
            string message;
            if (!CheckInputFields(out message))
            {
                ShowMessage(message);
                return;
            }

            MainManager.MapController.ActiveMap.MapSize = int.Parse(MapSizeInputField_InputField.text);
            MainManager.MapController.ActiveMap.ObstacleCount = int.Parse(ObstaclesCountInputField_InputField.text);
            MainManager.MapController.GenerateMap();
            SetButtonEnable(InputMapNameButton, true);
            SetButtonEnable(FindShortestWayButton, true);
            SetButtonEnable(CloseMenuButton, true);
            SetButtonEnable(ClearPathButton, false);
            SetGameActive();
            MainManager.CameraManager.SetDefaultCameraLocation(MainManager.MapController.ActiveMap);
        }

        private bool CheckInputFields(out string message)
        {
            message = "";
            bool result = true;
            int mapSize = 0;
            int obstacleCount = 0;
            int.TryParse(MapSizeInputField_InputField.text, out mapSize);
            int.TryParse(ObstaclesCountInputField_InputField.text, out obstacleCount);

            if (string.IsNullOrWhiteSpace(MapSizeInputField_InputField.text))
            {
                message += "Proszę wprowadzić wielkość mapy.\n";
                result = false;
            }
            else if (mapSize < 10)
            {
                message += "Wielkość mapy musi być większa niż 10.\n";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(ObstaclesCountInputField_InputField.text))
            {
                message += "Proszę wprowadzić liczbę przeszkód.\n";
                result = false;
            }
            else if (!string.IsNullOrWhiteSpace(MapSizeInputField_InputField.text) && obstacleCount > mapSize * mapSize)
            {
                message += "Zbyt duża liczba przeszkód w porównaniu do wielkości mapy.\n";
                result = false;
            }
            return result;
        }

        public void MenuButton_OnClick()
        {
            SetMenuActive();
        }
        public void CloseMenu_OnClick()
        {
            SetGameActive();
        }
        public void CloseInputMapNameButton_OnClick()
        {
            MapNameInputField_InputField.text = "";
            SetMenuActive();
        }
        public void LoadMapButton_OnClick()
        {
            LoadMapToScene();
            MainManager.CameraManager.SetDefaultCameraLocation(MainManager.MapController.ActiveMap);
        }

        private void LoadMapToScene()
        {
            LoadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            MainManager.MapController.GenerateLoadedMap();
            SetButtonEnable(InputMapNameButton, true);
            SetButtonEnable(FindShortestWayButton, true);
            SetButtonEnable(CloseMenuButton, true);
            if (MainManager.MapController.IsPathFound)
                SetButtonEnable(ClearPathButton, true);
            else
                SetButtonEnable(ClearPathButton, false);
            SetGameActive();
        }
        public void InputMapNameButton_OnClick()
        {
            SetInputMapNameActive();
        }
        public void SaveMapButton_OnClick()
        {
            string message;
            if(!CheckMapName(MapNameInputField_InputField.text, out message))
            {
                ShowMessage( message);
                return;
            }
            MapObject savedMap = MainManager.MapController.ActiveMap.Clone();
            savedMap.Name = MapNameInputField_InputField.text;
            SaveMap(savedMap);
            MapNameInputField_InputField.text = "";
            SetMenuActive();
            SetMenuAfterSave(savedMap);
            ShowMessage(string.Format("{0} {1}", "Mapa zapisana poprawnie pod nazwą", savedMap.Name));
        }
        public void FindShortestWayButton_OnClick()
        {
            bool isPathFound = MainManager.ExecuteAlgorithm();
            if (isPathFound)
            {
                SetButtonEnable(ClearPathButton, true);
                SetGameActive();
            }
        }
        private bool CheckMapName(string mapName, out string message)
        {
            bool result = true;
            message = "";
            if (string.IsNullOrWhiteSpace(mapName))
            {
                message = "Proszę wprowadzić nazwę mapy.";
                result = false;
            }
            else if (mapName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                message = "Nazwa mapy zawiera niedozwolone znaki.";
                result = false;
            }
            else if (MainManager.MapController.SavedMaps.Find(x => x?.Name == mapName) != null)
            {
                message = "Istnieje już mapa o takiej nazwie.";
                result = false;
            }
            return result;
        }
        public void ClearPath_OnClick()
        {
            MainManager.Algorithm.ClearPath(MainManager.MapController.ActiveMap.FloorElements);
            SetButtonEnable(ClearPathButton, false);
        }
        private void SaveMap(MapObject activeMap)
        {
            IODataManager.Save(activeMap);
            IODataManager.SaveMapName(activeMap.Name);
            MainManager.MapController.SavedMaps.Add(activeMap);
        }

        private void SetMenuAfterSave(MapObject activeMap)
        {
            SavedMapsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(activeMap.Name));
            SetButtonEnable(LoadMapButton, true);
            SetDropdownValue(SavedMapsDropdown_Dropdown, SavedMapsDropdown_Dropdown.options.Count - 1);
        }

        void SetMenuActive()
        {
            MainMenuPanel.SetActive(true);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(false);
            MessagePanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.MAIN_MENU;
        }
        void SetGameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(true);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(false);
            MessagePanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.THE_GAME;
        }
        void SetInputMapNameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(true);
            HelpPanel.SetActive(false);
            MessagePanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.MAP_SAVING;
        }
        void SetHelpActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(true);
            MessagePanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.HELP;
        }
        void SetMessagePanelActive()
        {
            MessagePanel.SetActive(true);
            MainManager.GameMode = GameModeEnum.MESSAGE_PANEL;
        }
        public void ShowHelpPanel_OnClick()
        {
            SetHelpActive();
        }
        public void CloseHelp_OnClick()
        {
            SetMenuActive();
        }
    }
}
