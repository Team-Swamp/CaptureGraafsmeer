using UnityEditor;
using UnityEngine;
using System.IO;
using Framework.GeoLocation;
using Framework.PhoneCamera;
using Framework.ScriptableObjects;
using UI.Canvas.PhotoTaking;

namespace UI.Canvas.PhotoBookSystem
{
    public sealed class PhotoBookWindow : EditorWindow
    {
        private const int MARGIN = 20;
        
        private const string INTERACTABLE = "Assets/Prefabs/Funcinal/Entities/Photo-Interactable.prefab";
        private const string PAGE = "Assets/Prefabs/Funcinal/UI/Canvas/PhotoBook/Page.prefab";
        
        private string newObjectName = "Default Text";
        private string infoA = "Info";
        private Vector2 vectorField = Vector2.zero;
        private Page currentPage;
        private PhotoData currentData;

        [MenuItem("Codename-BDLMTW/PhotoBook Window")]
        public static void ShowWindow() => GetWindow(typeof(PhotoBookWindow));

        private void OnGUI()
        {
            GUILayout.Label("Name for new page:");
            newObjectName = GUILayout.TextField(newObjectName);
            GUILayout.Label("Info for new page:");
            infoA = GUILayout.TextField(infoA);
            vectorField = EditorGUILayout.Vector2Field("Vector2 Field Label", vectorField);
            GUILayout.Space(MARGIN);

            if (newObjectName == string.Empty)
            {
                GUILayout.Space(MARGIN);
                GUILayout.Label("There is a name needed!");
                return;
            }
            
            if (GUILayout.Button("Make new page!"))
                MakeNewPage();
            
            if (GUILayout.Button("Make new interactable!"))
                SpawnInteractable();
        }

        private void MakeNewPage()
        {
            CreatePhotoData();
            
            GameObject parentObject = GameObject.Find("Photobook");
            
            if(!parentObject)
                return;
            
            Debug.Log("parent found");
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PAGE);
            if (prefab != null)
            {
                GameObject newPage = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parentObject.transform);
                newPage.name = newObjectName;

                newPage.transform.localScale = Vector3.one;
                newPage.transform.SetAsFirstSibling();
                newPage.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);

                currentPage = newPage.GetComponent<Page>();
                currentPage.SetData(currentData);
                
                Debug.Log(newPage.transform.position);
            }
            else
            {
                Debug.LogError("Prefab not found at path: " + PAGE);
            }
        }
        
        private void SpawnInteractable()
        {
            Debug.Log("Function executed with string: " + newObjectName);

            GameObject parentObject = GameObject.Find("Photo maker");
            
            if(!parentObject)
                return;
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(INTERACTABLE);
            if (prefab != null)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newObject.name = newObjectName;

                newObject.GetComponent<PhotoInteractable>().ParentPage = currentPage;
                newObject.GetComponent<PhotoInteractable>().SetPhotoTaker(parentObject.GetComponent<PhotoTaker>());
                newObject.GetComponent<PhotoInteractable>().SetPhotoData(currentData);
                
                newObject.GetComponent<CoordinatesTransform>().SetCords(vectorField);
                
                currentPage.SetInteractable(newObject.GetComponent<PhotoInteractable>());

                
                Debug.Log(newObject.transform.position);
            }
            else
                Debug.LogError("ejguheryuigh4erwyuigyo8rw");
        }

        private void CreatePhotoData()
        {
            PhotoData newData = CreateInstance<PhotoData>();
            string directoryPath = "Assets/ScriptableObjects/Photos/";
            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                AssetDatabase.Refresh();
            }

            newData.Title = newObjectName;
            newData.Info = (infoA, null);
            // todo: photodata renders
            
            string path = directoryPath + newObjectName + ".asset";
            
            AssetDatabase.CreateAsset(newData, path);
            AssetDatabase.SaveAssets();

            currentData = newData;
            
            Debug.Log("PhotoData asset created at: " + path);
        }
    }
}