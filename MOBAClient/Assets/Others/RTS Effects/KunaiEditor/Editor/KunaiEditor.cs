using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Kunai 
{
	public class KunaiEditor : EditorWindow 
	{
		Object obj;
		Object obj2;

		float flt2;

		string path;
		float scaleFloat = 1.00f;
		string notify;
		string tagString;
		
		float originalSpeed;
		float originalSize;
		Vector3 originalScale;

		float xSpeed = 0;
		float ySpeed = 0;
		float zSpeed = 0;

		KunaiRotation kunaiRotation;
		
		GameObject gObj;
		Color particleColor;

		List<GameObject> particlesList = new List<GameObject>();
		List<GameObject> tempList = new List<GameObject>();
		List<float> particleSizes = new List<float>();
		List<float> particleSpeeds = new List<float>();
		List<float> particleGravity = new List<float>();
		List<Vector3> particleScales = new List<Vector3>();
		List<Color> particleColors = new List<Color>();
		
		[MenuItem("Window/Kunai Editor")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow (typeof(KunaiEditor), false, "Kunai Editor");
		}
		
		void Update()
		{
			//Each time the object is changed;
			if (obj2 != obj && obj != null)
			{
				//If the Object has been changed, reset everything before getting new data

				AssetDatabase.Refresh();
				ResetScales ();
				ResetColors ();
				Clear ();
				AssetDatabase.Refresh();
				if(obj != null)
				{
					//If that something is a GameObject
					if (obj is GameObject)
					{
						//Cast it as a GameObject
						gObj = obj as GameObject;

						//If that GameObject has a ParticleSystem
						if (gObj.GetComponent<ParticleSystem>() != null)
						{
							notify = "Particle";
							particleColor = gObj.GetComponent<ParticleSystem>().startColor;
							particlesList.Add (gObj.gameObject);
							particleColors.Add (gObj.GetComponent<ParticleSystem>().startColor);
							particleSizes.Add (gObj.GetComponent<ParticleSystem>().startSize);
							particleSpeeds.Add (gObj.GetComponent<ParticleSystem>().startSpeed);
							particleGravity.Add (gObj.GetComponent<ParticleSystem>().gravityModifier);
							particleScales.Add (gObj.transform.localScale);
						}
						//If that gameObject has children
						if (gObj.transform.childCount > 0)
						{
							GetChildren(gObj);
						}
						if (gObj.transform.childCount == 0 && gObj.GetComponent<ParticleSystem>() == null)
						{
							notify = "Invalid Selection";
						}
					}
					//If the drag n dropped item is a folder
					else if (Directory.Exists(AssetDatabase.GetAssetPath(obj.GetInstanceID())))
					{
						string sAssetFolderPath = AssetDatabase.GetAssetPath(obj);
						string sDataPath  = Application.dataPath;
						string sFolderPath = sDataPath.Substring(0 ,sDataPath.Length-6)+sAssetFolderPath;
						string[] aFilePaths = Directory.GetFiles(sFolderPath);
						foreach (string sFilePath in aFilePaths) 
						{
							string sAssetPath = sFilePath.Substring(sDataPath.Length-6);					
							Object temp =  AssetDatabase.LoadAssetAtPath(sAssetPath,typeof(GameObject));
							tempList.Add (temp as GameObject);
						}
						//Find all GameObjects in folder and put them into an array
						foreach (GameObject temp in tempList)
						{
							if(temp != null)
							{
								//If the GameObject is a particle system put it in an array. Make arrays for size and speeds too
								if (temp.GetComponent<ParticleSystem>() != null)
								{
									particlesList.Add (temp);
									particleColors.Add (temp.GetComponent<ParticleSystem>().startColor);
									particleSizes.Add (temp.GetComponent<ParticleSystem>().startSize);
									particleSpeeds.Add (temp.GetComponent<ParticleSystem>().startSpeed);
									particleGravity.Add (temp.GetComponent<ParticleSystem>().gravityModifier);
									particleScales.Add (temp.transform.localScale);
								}
								if (temp.transform.childCount > 0)
								{
									
									GetChildren(temp);
								}
								if (temp.transform.childCount == 0 && temp.GetComponent<ParticleSystem>() == null)
								{
									notify = "";
								}
							}
						}
					}
				}
				//If nothing is currently in the drag n drop field
				else
				{
					notify = null;
				}			
				obj2 = obj;
			}
			if (flt2 != scaleFloat)
			{
				SetScales ();
				flt2 = scaleFloat;
			}
		}
		
		void OnGUI () 
		{
			GUILayout.Label ("Folder or Prefab", EditorStyles.boldLabel);
			GUILayout.Label ("Drag and drop the prefab or folder you wish to scale.", EditorStyles.wordWrappedMiniLabel);
			obj = (Object)EditorGUILayout.ObjectField(obj, typeof(Object), true,GUILayout.ExpandWidth(false));
			GUILayout.Label (notify);

			scaleFloat = EditorGUILayout.FloatField ("Scale Multiplier", (Mathf.Clamp(scaleFloat, 0.01f, 1000f)),GUILayout.ExpandWidth(false));

			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Set Scale"))
			{
				SetScales ();
			}
			if(GUILayout.Button("Reset Scale"))
			{
				ResetScales ();
			}

			GUILayout.EndHorizontal ();

			GUILayout.Label ("Rotations", EditorStyles.boldLabel);

			xSpeed = EditorGUILayout.FloatField ("X", (Mathf.Clamp(xSpeed, -10000f, 10000f)),GUILayout.ExpandWidth(false));
			ySpeed = EditorGUILayout.FloatField ("Y", (Mathf.Clamp(ySpeed, -10000f, 10000f)),GUILayout.ExpandWidth(false));
			zSpeed = EditorGUILayout.FloatField ("Z", (Mathf.Clamp(zSpeed, -10000f, 10000f)),GUILayout.ExpandWidth(false));
			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Set Rotation"))
			{
				SetRotation ();
			}
			if(GUILayout.Button("Reset Rotation"))
			{
				xSpeed = 0;
				ySpeed = 0;
				zSpeed = 0;
				ResetRotation ();
			}
			GUILayout.EndHorizontal ();

			GUILayout.Label ("Color", EditorStyles.boldLabel);
			GUILayout.Label ("Note: This will not effect Gradients!", EditorStyles.wordWrappedMiniLabel);

			particleColor = EditorGUILayout.ColorField("Start Color", particleColor,GUILayout.ExpandWidth(false));
			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Set Color"))
			{
				SetColors ();
			}
			if(GUILayout.Button("Reset Color"))
			{
				ResetColors ();
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			tagString = EditorGUILayout.TextField ("Copy Suffix", tagString,GUILayout.ExpandWidth(false));
			if(GUILayout.Button("Save Copy"))
			{
				if (particlesList[0].activeInHierarchy)
				{
					Debug.Log ("Can only save copies of a prefab, not active gameObjects.");
				}
				else
					Save ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.Label ("Please use 'Exit' to close window rather than the X button.", EditorStyles.wordWrappedMiniLabel);
			if(GUILayout.Button("Exit"))
			{
				ResetScales ();
				ResetColors ();
				this.Close();
			}
		}
		
		void GetChildren(GameObject parentObj)
		{
			//Check if those children have particleSystems
			foreach (Transform child in parentObj.transform)
			{
				//If they do, put them in a list to scale
				if (child.GetComponent<ParticleSystem>() != null)
				{
					if(!particlesList.Contains (parentObj))
					{
						particlesList.Add (parentObj);
						particleScales.Add (parentObj.transform.localScale);
					}
					particlesList.Add (child.gameObject);
					particleColors.Add (child.GetComponent<ParticleSystem>().startColor);
					particleSizes.Add (child.GetComponent<ParticleSystem>().startSize);
					particleSpeeds.Add (child.GetComponent<ParticleSystem>().startSpeed);
					particleGravity.Add (child.GetComponent<ParticleSystem>().gravityModifier);
					particleScales.Add (child.transform.localScale);
				}
				//If it has children but none of them are particles
				else {notify = "";}
			}
		}
		
		void Clear ()
		{
			tempList.Clear ();
			particlesList.Clear ();
			particleSizes.Clear ();
			particleSpeeds.Clear ();
			particleScales.Clear ();
			particleGravity.Clear ();
		}
		
		void ResetScales ()
		{
			scaleFloat = 1.00f;
			int j = 0;
			for(int i = 0; i < particlesList.Count; i++)
			{
				if(particlesList[i].GetComponent<ParticleSystem> () != null)
				{
					particlesList[i].GetComponent<ParticleSystem>().startSize = particleSizes[j] * scaleFloat;
					particlesList[i].GetComponent<ParticleSystem>().startSpeed = particleSpeeds[j] * scaleFloat;
					particlesList[i].GetComponent<ParticleSystem>().gravityModifier = particleGravity[j] * scaleFloat;
					particlesList[i].transform.localScale = (particleScales[j] * scaleFloat);
					j++;
				}
				else {particlesList[i].transform.localScale = (particleScales[i] * scaleFloat);};
			}
		}

		void SetColors ()
		{
			int j = 0;
			for(int i = 0; i < particlesList.Count; i++)
			{
				if(particlesList[i].GetComponent<ParticleSystem> () != null)
				{
					particlesList[i].GetComponent<ParticleSystem>().startColor = particleColor;
					j++;
				}
			}
		}

		void ResetColors ()
		{
			int j = 0;
			for(int i = 0; i < particlesList.Count; i++)
			{
				if(particlesList[i].GetComponent<ParticleSystem> () != null)
				{
					particlesList[i].GetComponent<ParticleSystem>().startColor = particleColors[j];
					j++;
				}
			}
		}

		void SetScales ()
		{
			int j = 0;
			for(int i = 0; i < particlesList.Count; i++)
			{
				if(particlesList[i].GetComponent<ParticleSystem> () != null)
				{
					particlesList[i].GetComponent<ParticleSystem>().startSize = particleSizes[j] * scaleFloat;
					particlesList[i].GetComponent<ParticleSystem>().startSpeed = particleSpeeds[j] * scaleFloat;
					particlesList[i].GetComponent<ParticleSystem>().gravityModifier = particleGravity[j] * scaleFloat;
					particlesList[i].transform.localScale = (particleScales[j] * scaleFloat);
					j++;
				}
				else {particlesList[i].transform.localScale = (particleScales[i] * scaleFloat);};
			}
		}

		void SetRotation ()
		{
			foreach(Object temp in particlesList)
			{
				GameObject tempGO = temp as GameObject;
				if(tempGO.GetComponent<KunaiRotation>() == null)
				{
					kunaiRotation = tempGO.AddComponent<KunaiRotation>() as KunaiRotation;
				}
				else{kunaiRotation = tempGO.GetComponent<KunaiRotation>();}
				kunaiRotation.xSpeed = xSpeed;
				kunaiRotation.ySpeed = ySpeed;
				kunaiRotation.zSpeed = zSpeed;
			}
		}

		void ResetRotation ()
		{
			foreach(Object temp in particlesList)
			{
				GameObject tempGO = temp as GameObject;
				if(tempGO.GetComponent<KunaiRotation>() != null)
				{
					kunaiRotation = tempGO.GetComponent<KunaiRotation>();
				}
				DestroyImmediate(kunaiRotation, true);
			}
		}

		void Save ()
		{
			if(tagString == null || tagString == "")
			{
				Debug.LogWarning("You must enter a tag to save a copy of your prefab!");
				return;
			}
			foreach(Object temp in particlesList)
			{
				GameObject tempGO = temp as GameObject;
				
				//If this object is a parent
				if(tempGO.transform.root == tempGO.transform && tempGO.transform.childCount > 0)
				{
					string myPath = AssetDatabase.GetAssetPath (temp).Substring(0, AssetDatabase.GetAssetPath (temp).Length - 7);
					Object prefab = PrefabUtility.CreateEmptyPrefab(myPath + tagString + ".prefab");
					PrefabUtility.ReplacePrefab(temp as GameObject, prefab);
				}
				//If this object is just a particle
				if(tempGO.transform.root == tempGO.transform && tempGO.transform.childCount == 0)
				{
					string myPath = AssetDatabase.GetAssetPath (temp).Substring(0, AssetDatabase.GetAssetPath (temp).Length - 7);
					Object prefab = PrefabUtility.CreateEmptyPrefab(myPath + tagString + ".prefab");
					PrefabUtility.ReplacePrefab(temp as GameObject, prefab);
				}
			}
			ResetScales ();
			ResetColors ();
		}

		void OnApplicationQuit() 
		{
			ResetScales ();
			ResetColors ();
		}
	}
}