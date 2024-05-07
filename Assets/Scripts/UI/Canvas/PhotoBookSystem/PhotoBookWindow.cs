using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

using Framework.GeoLocation;
using Framework.PhoneCamera;
using Framework.ScriptableObjects;
using UI.Canvas.PhotoTaking;

namespace UI.Canvas.PhotoBookSystem
{
    public sealed class PhotoBookWindow : EditorWindow
    {
        private const int MARGIN = 20;

        private const string SPACE = " ";
        private const string INTERACTABLE_PATH = "Assets/Prefabs/Funcinal/Entities/Photo-Interactable.prefab";
        private const string PAGE_PAGE = "Assets/Prefabs/Funcinal/UI/Canvas/PhotoBook/Page.prefab";
        private const string DIRECTORY_PATH = "Assets/ScriptableObjects/Photos/";
        private const string PHOTO_BOOK_NAME = "Photobook";
        private const string PHOTO_MAKER_NAME = "Photo maker";
        private const string CAMERA_PANEL_NAME = "CameraPanel";
        private const string NAME_FIELD = "Name for new page:";
        private const string INFO_FIELD = "Info for new page:";
        private const string CORDS_FIELD = "Vector2 coordinates field:";
        private const string BUTTON_TEXT = "Make new page!";
        private const string SCRIPTABLE_OBJECT_SUFFIX = ".asset";
        private const string INVALID_CHARACTERS = "^[a-zA-Z0-9 ]+$";
        private const string PREFAB_PATH_NOT_FOUND_ERROR = "Prefab not found at path: ";
        private const string NAME_NEEDED_ERROR = "There is a name needed!";
        private const string INVALID_NAME_ERROR = "Invalid input for the name field.";
        private const string PHOTO_BOOK_REFERENCE_WARNING = "You need to reference the page in the photobook pages list!";
        private const string PHOTO_INTERACTABLE_REFERENCE_WARNING = "You need to reference the interactable in the page!";
        
        private static readonly Vector2 WINDOW_SIZE = new (400f, 310f);
        
        private bool _needsRepaint = false;
        private string _newObjectName = "Default Text";
        private string _info = "Info";
        private Vector2 _cords = Vector2.zero;
        private Page _currentPage;
        private PhotoData _currentData;
        private GUIStyle _greenButtonStyle;

        [MenuItem("Codename-BDLMTW/PhotoBook Window")]
        public static void ShowWindow()
        {
            PhotoBookWindow window = GetWindow<PhotoBookWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            if (!EditorApplication.isFocused)
            {
                GUILayout.Space(MARGIN);
                GUILayout.Label(NAME_FIELD + SPACE + _newObjectName);
                GUILayout.Space(MARGIN);
                GUILayout.Label(INFO_FIELD + SPACE + _info);
                GUILayout.Space(MARGIN);
                GUILayout.Label(CORDS_FIELD + SPACE + _cords);
                GUILayout.Space(MARGIN);
                UnRequestRepaint();
                return;
            }
            
            if (_needsRepaint)
            {
                Repaint();
                _needsRepaint = false;
            }
            
            PhotoBookWindow window = GetWindow<PhotoBookWindow>();
            
            window.minSize = WINDOW_SIZE;
            window.maxSize = WINDOW_SIZE;
            
            GUILayout.Space(MARGIN);
            GUILayout.Label(NAME_FIELD);
            _newObjectName = GUILayout.TextField(_newObjectName);
            GUILayout.Space(MARGIN);
            GUILayout.Label(INFO_FIELD);
            _info = EditorGUILayout.TextArea(_info, GUILayout.Height(100));
            GUILayout.Space(MARGIN);
            _cords = EditorGUILayout.Vector2Field(CORDS_FIELD, _cords);
            GUILayout.Space(MARGIN);
            
            if(InvalidInput())
                return;
            
            if (GUILayout.Button(BUTTON_TEXT))
                MakeNewPage();
            
            UnRequestRepaint();
        }

        private void MakeNewPage()
        {
            CreatePhotoData();
            
            GameObject parentObject = GameObject.Find(PHOTO_BOOK_NAME);

            if (!parentObject)
                return;
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PAGE_PAGE);
            if (prefab != null)
            {
                GameObject newPage = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parentObject.transform);
                newPage.name = _newObjectName;

                newPage.transform.localScale = Vector3.one;
                newPage.transform.SetAsFirstSibling();
                newPage.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);

                _currentPage = newPage.GetComponent<Page>();
                _currentPage.SetData(_currentData);
                
                Debug.LogWarning(PHOTO_BOOK_REFERENCE_WARNING);
            }
            else
                Debug.LogError(PREFAB_PATH_NOT_FOUND_ERROR + PAGE_PAGE);
            
            SpawnInteractable();
        }
        
        private void SpawnInteractable()
        {
            GameObject parentObject = GameObject.Find(PHOTO_MAKER_NAME);
            GameObject panel = GameObject.Find(CAMERA_PANEL_NAME);

            if (!parentObject
                || !panel)
                return;
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(INTERACTABLE_PATH);
            if (prefab != null)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newObject.name = _newObjectName;

                newObject.GetComponent<CoordinatesTransform>().SetCords(_cords);
                PhotoInteractable newInteractable = newObject.GetComponent<PhotoInteractable>();
                
                newInteractable.ParentPage = _currentPage;
                newInteractable.SetPhotoTaker(parentObject.GetComponent<PhotoTaker>());
                newInteractable.SetPhotoData(_currentData);
                newInteractable.SetPanel(panel.GetComponent<CameraPanel>());
                
                Debug.LogWarning(PHOTO_INTERACTABLE_REFERENCE_WARNING);
            }
            else
                Debug.LogError(PREFAB_PATH_NOT_FOUND_ERROR + INTERACTABLE_PATH);
        }

        private void CreatePhotoData()
        {
            PhotoData newData = CreateInstance<PhotoData>();
            
            if (!Directory.Exists(DIRECTORY_PATH))
            {
                Directory.CreateDirectory(DIRECTORY_PATH);
                AssetDatabase.Refresh();
            }

            newData.Title = _newObjectName;
            newData.Info = (_info, null);
            // todo: photodata renders
            
            string path = DIRECTORY_PATH + _newObjectName + SCRIPTABLE_OBJECT_SUFFIX;
            
            AssetDatabase.CreateAsset(newData, path);
            AssetDatabase.SaveAssets();

            _currentData = newData;
        }
        
        private void RequestRepaint() => _needsRepaint = true;

        private void UnRequestRepaint() => _needsRepaint = false;

        private bool InvalidInput()
        {
            if (_newObjectName == string.Empty)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUIStyle centeredLabelStyle = new (EditorStyles.boldLabel)
                {
                    normal =
                    {
                        textColor = Color.red
                    }
                };
                GUILayout.Label(NAME_NEEDED_ERROR, centeredLabelStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                RequestRepaint();
                
                return true;
            }
            
            if (!IsValidFileName(_newObjectName))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUIStyle centeredLabelStyle = new (EditorStyles.boldLabel)
                {
                    normal =
                    {
                        textColor = Color.red
                    }
                };
                GUILayout.Label(INVALID_NAME_ERROR, centeredLabelStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                RequestRepaint();
                
                return true;
            }
            
            return false;
        }
        
        private static bool IsValidFileName(string fileName)
        {
            string pattern = INVALID_CHARACTERS;
            Regex regex = new Regex(pattern);
            return regex.IsMatch(fileName);
        }
    }
}