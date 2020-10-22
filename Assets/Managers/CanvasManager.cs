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
        TMP_InputField MapSizeInputField_InputField;
        TMP_InputField ObstaclesCountInputField_InputField;
        TMP_InputField MapNameInputField_InputField;
        TMP_Dropdown SavedMapsDropdown_Dropdown;
        TMP_Dropdown AlgorithmsDropdown_Dropdown;
        // Start is called before the first frame update
        void Start()
        {
            getGameObjects();
            setObjects();
            setMenuActive();
            checkLoadedMaps();
            setDropdownDefaultValues();
        }

        private void setDropdownDefaultValues()
        {
            setDropdownValue(SavedMapsDropdown_Dropdown, 0);
            setDropdownValue(AlgorithmsDropdown_Dropdown, 0);
        }

        private void checkLoadedMaps()
        {
            if (SavedMapsDropdown_Dropdown.options.Count == 0)
            {
                setButtonEnable(LoadMapButton, false);
            }
            else
            {
                loadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            }
        }
        void setDropdownValue(TMP_Dropdown dropdown, int index)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
        //todo wybor algorytmow
        //todo obiekt przeszkody wymiary
        //todo czasy
        //todo instrukcja
        private void getGameObjects()
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
        }

        private void setObjects()
        {
            MainManager.MapController = new MapController();
            MapSizeInputField_InputField = MapSizeInputField.GetComponent<TMP_InputField>();
            ObstaclesCountInputField_InputField = ObstaclesCountInputField.GetComponent<TMP_InputField>();
            MapNameInputField_InputField = MapNameInputField.GetComponent<TMP_InputField>();
            SavedMapsDropdown_Dropdown = SavedMapsDropdown.GetComponent<TMP_Dropdown>();
            AlgorithmsDropdown_Dropdown = AlgorithmsDropdown.GetComponent<TMP_Dropdown>();
            SavedMapsDropdown_Dropdown.onValueChanged.AddListener(delegate
            {
                SavedMapsDropdown_OnValueChanged(SavedMapsDropdown_Dropdown);
            });
            AlgorithmsDropdown_Dropdown.onValueChanged.AddListener(delegate
            {
                AlgorithmsDropdown_OnValueChanged(AlgorithmsDropdown_Dropdown);
            });
            loadMapsToMapDropdown();
            loadAlgorithmsToAlgorithimDropdown();
            setButtonEnable(InputMapNameButton, false);
            setButtonEnable(FindShortestWayButton, false);
            MainManager.AlgothitmEnum = (AlgothitmEnum)AlgorithmsDropdown_Dropdown.value;
        }

        private void loadAlgorithmsToAlgorithimDropdown()
        {
            AlgorithmsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(AlgothitmEnum.DIJKSTRA.ToString()));
            AlgorithmsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(AlgothitmEnum.BELLMAN_FORD.ToString()));
        }

        private void AlgorithmsDropdown_OnValueChanged(TMP_Dropdown algorithmsDropdown_Dropdown)
        {
            MainManager.AlgothitmEnum = (AlgothitmEnum)algorithmsDropdown_Dropdown.value;
        }

        private void loadMapsToMapDropdown()
        {
            foreach(var map in MainManager.MapController.SavedMaps)
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
            MainManager.MapController.LoadMapByName(mapName);
            MapSizeInputField_InputField.text = MainManager.MapController.LoadedMap.MapSize.ToString();
            ObstaclesCountInputField_InputField.text = MainManager.MapController.LoadedMap.ObstacleCount.ToString();
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

            MainManager.MapController.ActiveMap.MapSize = int.Parse(MapSizeInputField_InputField.text);
            MainManager.MapController.ActiveMap.ObstacleCount = int.Parse(ObstaclesCountInputField_InputField.text);
            MainManager.MapController.GenerateMap();
            setButtonEnable(InputMapNameButton, true);
            setButtonEnable(FindShortestWayButton, true);
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
        public void CloseInputMapNameButton_OnClick()
        {
            MapNameInputField_InputField.text = "";
            setMenuActive();
        }
        public void LoadMapButton_OnClick()
        {
            loadMapToScene();
        }

        private void loadMapToScene()
        {
            loadMapToObject(SavedMapsDropdown_Dropdown.options[SavedMapsDropdown_Dropdown.value].text);
            MainManager.MapController.GenerateLoadedMap();
            setButtonEnable(InputMapNameButton, true);
            setButtonEnable(FindShortestWayButton, true);
            setGameActive();
        }
        public void InputMapNameButton_OnClick()
        {
            setInputMapNameActive();
        }
        public void SaveMapButton_OnClick()
        {
            string message;
            if(!checkMapName(MapNameInputField_InputField.text, out message))
            {
                EditorUtility.DisplayDialog("Błąd zapisywania mapy", message, "OK");
                return;
            }
            MapObject savedMap = MainManager.MapController.ActiveMap.Clone();
            savedMap.Name = MapNameInputField_InputField.text;
            saveMap(savedMap);
            MapNameInputField_InputField.text = "";
            setMenuActive();
            setMenuAfterSave(savedMap);
            EditorUtility.DisplayDialog("Zapisano mapę", string.Format("{0} {1}", "Mapa zapisana poprawnie pod nazwą", savedMap.Name), "OK");
        }
        public void FindShortestWayButton_OnClick()
        {
            MainManager.ExecuteAlgorithm();
        }
        private bool checkMapName(string mapName, out string message)
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
            else if (MainManager.MapController.SavedMaps.Any(x => x.Name == mapName))
            {
                message = "Istnieje już mapa o takiej nazwie.";
                result = false;
            }
            return result;
        }

        private void saveMap(MapObject activeMap)
        {
            IODataManager.Save(activeMap);
            IODataManager.SaveMapName(activeMap.Name);
            MainManager.MapController.SavedMaps.Add(activeMap);
        }

        private void setMenuAfterSave(MapObject activeMap)
        {
            SavedMapsDropdown_Dropdown.options.Add(new TMP_Dropdown.OptionData(activeMap.Name));
            setButtonEnable(LoadMapButton, true);
            setDropdownValue(SavedMapsDropdown_Dropdown, SavedMapsDropdown_Dropdown.options.Count - 1);
        }

        void setMenuActive()
        {
            MainMenuPanel.SetActive(true);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.MAIN_MENU;
        }
        void setGameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(true);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.THE_GAME;
        }
        void setInputMapNameActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(true);
            HelpPanel.SetActive(false);
            MainManager.GameMode = GameModeEnum.MAP_SAVING;
        }
        void setHelpActive()
        {
            MainMenuPanel.SetActive(false);
            TheGamePanel.SetActive(false);
            InputMapNamePanel.SetActive(false);
            HelpPanel.SetActive(true);
            MainManager.GameMode = GameModeEnum.HELP;
        }
        public void ShowHelpPanel_OnClick()
        {
            setHelpActive();
        }
        public void CloseHelp_OnClick()
        {
            setMenuActive();
        }
    }
}
