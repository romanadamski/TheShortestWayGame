using Assets.ApplicationObjects;
using Assets.Models;
using Assets.SavedMaps;
using Packages.Rider.Editor.Util;
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

        MapController MapManager;
        TMP_InputField MapSizeInputField_InputField;
        TMP_InputField ObstaclesCountInputField_InputField;
        TMP_InputField MapNameInputField_InputField;
        TMP_Dropdown SavedMapsDropdown_Dropdown;
        public GameMode GameMode;
        // Start is called before the first frame update
        void Start()
        {
            getGameObjects();
            setObjects();
            setMenuActive();
            checkLoadedMaps();
        }

        private void checkLoadedMaps()
        {
            if (SavedMapsDropdown_Dropdown.options.Count == 0)
            {
                setButtonEnable(LoadMapButton, false);
            }
            else
            {
                setDropdownMapNamesValue(0);
                loadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            }
        }
        void setDropdownMapNamesValue(int index)
        {
            SavedMapsDropdown_Dropdown.value = index;
            SavedMapsDropdown_Dropdown.RefreshShownValue();
        }
        //todo wybor algorytmow
        //todo obiekt przeszkody wymiary
        private void getGameObjects()
        {
            MapSizeInputField = GameObject.Find("MapSizeInputFieldTMP");
            ObstaclesCountInputField = GameObject.Find("ObstaclesCountInputFieldTMP");
            MainMenuPanel = GameObject.Find("MainMenuPanel");
            TheGamePanel = GameObject.Find("TheGamePanel");
            SavedMapsDropdown = GameObject.Find("SavedMapsDropdown");
            LoadMapButton = GameObject.Find("LoadMapButton");
            InputMapNameButton = GameObject.Find("InputMapNameButton");
            MapNameInputField = GameObject.Find("MapNameInputField");
            InputMapNamePanel = GameObject.Find("InputMapNamePanel");
        }
        private void setObjects()
        {
            MapManager = new MapController();
            MapSizeInputField_InputField = MapSizeInputField.GetComponent<TMP_InputField>();
            ObstaclesCountInputField_InputField = ObstaclesCountInputField.GetComponent<TMP_InputField>();
            SavedMapsDropdown_Dropdown = SavedMapsDropdown.GetComponent<TMP_Dropdown>();
            MapNameInputField_InputField = MapNameInputField.GetComponent<TMP_InputField>();
            SavedMapsDropdown_Dropdown.onValueChanged.AddListener(delegate
            {
                SavedMapsDropdown_OnValueChanged(SavedMapsDropdown_Dropdown);
            });
            loadMapsToDropdown();
            setButtonEnable(InputMapNameButton, false);
        }

        private void loadMapsToDropdown()
        {
            foreach(var map in MapManager.SavedMaps)
            {
                if(map != null)
                    SavedMapsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(map.Name));
            }
        }
        void SavedMapsDropdown_OnValueChanged(TMP_Dropdown change)
        {
            loadMapToObject(change.options[change.value].text);
        }

        private void loadMapToObject(string mapName)
        {
            MapManager.LoadMapByName(mapName);
            MapSizeInputField_InputField.text = MapManager.LoadedMap.MapSize.ToString();
            ObstaclesCountInputField_InputField.text = MapManager.LoadedMap.ObstacleCount.ToString();
        }

        private void setButtonEnable(GameObject button, bool active)
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
                EditorUtility.DisplayDialog("Błąd wczytywania mapy", message, "OK");
                return;
            }
            
            MapManager.ActiveMap.MapSize = int.Parse(MapSizeInputField_InputField.text);
            MapManager.ActiveMap.ObstacleCount = int.Parse(ObstaclesCountInputField_InputField.text);
            MapManager.GenerateMap();
            setButtonEnable(InputMapNameButton, true);
            setGameActive();
        }

        private bool CheckInputFields(out string message)
        {
            message = "";
            bool result = true;
            if (string.IsNullOrWhiteSpace(MapSizeInputField_InputField.text))
            {
                message += "Proszę wprowadzić wielkość mapy.\n";
                result = false;
            }
            else if (int.Parse(MapSizeInputField_InputField.text) < 10)
            {
                message += "Wielkość mapy musi być większa niż 10.\n";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(ObstaclesCountInputField_InputField.text))
            {
                message += "Proszę wprowadzić ilość przeszkód.\n";
                result = false;
            }
            return result;
        }

        public void MenuButton_OnClick()
        {
            setMenuActive();
        }
        public void CloseMenu_OnClick()
        {
            setGameActive();
        }
        public void LoadMapButton_OnClick()
        {
            loadMapToScene();
        }

        private void loadMapToScene()
        {
            loadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            MapManager.GenerateLoadedMap();
            setGameActive();
        }
        public void InputMapNameButton_OnClick()
        {
            setInputMapNameActive();
        }
        public void SaveMapButton_OnClick()
        {
            checkMapName(MapNameInputField_InputField.text);
            MapObject savedMap = MapManager.ActiveMap.Clone();
            savedMap.Name = MapNameInputField_InputField.text;
            saveMap(savedMap);
            setMenuActive();
            setMenuAfterSave(savedMap);
        }
        private bool checkMapName(string mapName)
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(mapName))
            {
                result = false;
            }
            else if (mapName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                result = false;
            }
            else if (MapManager.SavedMaps.Any(x => x.Name == mapName))
            {
                result = false;
            }
            return result;
        }

        private void saveMap(MapObject activeMap)
        {
            IODataManager.Save(activeMap);
            IODataManager.SaveMapName(activeMap.Name);
            MapManager.SavedMaps.Add(activeMap);
        }

        private void setMenuAfterSave(MapObject activeMap)
        {
            SavedMapsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(activeMap.Name));
            setButtonEnable(LoadMapButton, true);
            setDropdownMapNamesValue(SavedMapsDropdown_Dropdown.options.Count - 1);
        }

        void setMenuActive()
        {
            MainMenuPanel.SetActive(true);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(false);
            GameMode = GameMode.MAIN_MENU;
        }
        void setGameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(true);
            InputMapNamePanel.SetActive(false);
            GameMode = GameMode.THE_GAME;
        }
        void setInputMapNameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(true);
            GameMode = GameMode.MAP_SAVING;
        }
    }
}
