/**
 * UGUI±‡º≠Õÿ’π
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GameUnityFramework.Extensions;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Editor.UGUIExtensions
{
    public class FindPrefabRefer
    {
        [MenuItem("Assets/FindRefer")]
        static void FindReferOfPrefab()
        {
            string rootPath = "D:\\Program Files\\Unity\\Programma\\zfxq\\Assets\\ArtPacks\\Prefabs\\2DGraphics";//\\UIViews";
            string searchTextureName=Selection.activeObject.name;
            List<string> files = GetAllPrefabs(rootPath);
            foreach (string file in files)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.Split("\\zfxq\\").Last());
                if (prefab == null)
                {
                    continue;
                }
                Component[] components = prefab.GetComponentsInChildren<Component>(true);
                foreach (Component component in components)
                {
                    if (component is Renderer)
                    {
                        Material[] materials = ((Renderer)component).sharedMaterials;
                        foreach (Material material in materials)
                        {
                            if (material != null && material.mainTexture != null && material.mainTexture.name == searchTextureName)
                            {
                                Debug.Log(file);
                                break;
                            }
                        }
                    }
                    else if (component is SpriteRenderer)
                    {
                        Sprite sprite = ((SpriteRenderer)component).sprite;
                        if (sprite != null && sprite.texture != null && sprite.texture.name == searchTextureName)
                        {
                            Debug.Log(file);
                            break;
                        }
                    }else if(component is Image)
                    {
                        Image image = ((Image)component);
                        if (image != null && image.mainTexture != null && image.mainTexture.name == searchTextureName)
                        {
                            Debug.Log (file); 
                            break;
                        }
                    }
                }
            }
        }
        public static List<string> GetAllPrefabs(string RootPath)
        {
            List<string> paths = new List<string>();
            paths.AddRange(Directory.GetFiles(RootPath, "*.prefab", SearchOption.TopDirectoryOnly));
            string[] dir = Directory.GetDirectories(RootPath);
            foreach (string path in dir)
            {
                paths.AddRange(GetAllPrefabs(path));
            }
            return paths;
        }
    }
}

