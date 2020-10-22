using Assets.ApplicationObjects;
using Assets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SavedMaps
{
    public static class IODataManager
    {
		public static string extension = "map";
		public static string mainFileName = string.Format("{0}.{1}", "map_names", extension);

		
		public static void CreateMainFile()
        {
			try
			{
				if (!File.Exists(mainFileName))
                {
					Stream stream = File.Open(mainFileName, FileMode.Create);
					stream.Close();
				}
			}
			catch (Exception e)
			{

			}
		}
		public static List<string> GetMapsNames()
        {
			try
			{
				return File.ReadAllLines(mainFileName).ToList();
			}
			catch (Exception e)
			{
				return null;
			}
		}
		public static void SaveMapName(string mapName)
		{
			try
			{
				if (!File.Exists(mainFileName))
				{
					CreateMainFile();
				}
				using (StreamWriter sw = File.AppendText(mainFileName))
				{
					sw.WriteLine(mapName);
				}
			}
			catch (Exception e)
			{

			}
		}
		public static List<MapObject> LoadMapsByMapNames(List<string> mapNames)
        {
			List<MapObject> mapObjects = new List<MapObject>();
			foreach(var mapName in mapNames)
            {
				mapObjects.Add(Load(mapName));
			}
			return mapObjects;
		}
		public static MapObject Load(string filePath)
		{
			try
			{
				MapModel data = new MapModel();
				filePath = string.Format("{0}.{1}", filePath, extension);
				using (StreamReader sr = new StreamReader(filePath))
				{
					string mapModelJson = sr.ReadToEnd();
					data = JsonUtility.FromJson<MapModel>(mapModelJson);
				}
				MapObject mapObject = Mapper.ConvertMapModelToMapObject(data);
                return mapObject;
			}
			catch (Exception e)
            {
                return null;
			}
		}
		public static void Save(MapObject data)
		{
			try
			{
				MapModel mapModel = Mapper.ConvertMapObjectToMapModel(data);
				string fileName = string.Format("{0}.{1}", mapModel.Name, extension);
				using(StreamWriter sw = new StreamWriter(fileName))
                {
					string mapModelJson = JsonUtility.ToJson(mapModel);
					sw.Write(mapModelJson);
				}
			}
			catch (Exception e)
			{

			}
		}
	}
}
