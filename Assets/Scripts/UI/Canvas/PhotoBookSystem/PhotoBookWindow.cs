using UnityEditor;
using UnityEngine;

namespace UI.Canvas.PhotoBookSystem
{
    public sealed class PhotoBookWindow : EditorWindow
    {
        private const int MARGIN = 20;
        
        private const string PREFAB = "Assets/Prefabs/Funcinal/Entities/Photo-Interactable.prefab";
        
        private string newObjectName = "Default Text";

        [MenuItem("Codename-BDLMTW/PhotoBook Window")]
        public static void ShowWindow() => GetWindow(typeof(PhotoBookWindow));

        private void OnGUI()
        {
            GUILayout.Label("Name for new page:");
            newObjectName = GUILayout.TextField(newObjectName);
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
            
        }
        
        private void SpawnInteractable()
        {
            Debug.Log("Function executed with string: " + newObjectName);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB);
            
            if (prefab != null)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newObject.name = newObjectName; 
                
                Vector3 spawnPosition = new Vector3(0, 0, 0); // Set the desired spawn position
                newObject.transform.position = spawnPosition;
                Debug.Log(newObject.transform.position);
            }
            else
                Debug.LogError("ejguheryuigh4erwyuigyo8rw");
        }
    }
}