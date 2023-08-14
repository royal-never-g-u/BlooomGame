using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.UI;
using GameUnityFramework.Extensions;
using System.Text.RegularExpressions;
using System.Text;
using Object = UnityEngine.Object;
using System.IO;
using UnityEditor.U2D;
using UnityEngine.U2D;
using Editor.UGUIExtensions;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.GUI;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.Graphs;
using UnityEditor.SceneManagement;
using GameUnityFramework.Resource;
using GameGraphics.HomeWorld;
using UnityEditor.TerrainTools;
using GameBaseFramework.Base;

public class YouHua : MonoBehaviour
{
    [MenuItem("Tools/�Ż�/δ֪")]
    public static void FindComponentInPrefab()
    {

    }
    [MenuItem("Tools/�Ż�/�رո��ı�")]
    public static void CloseRichText()
    {
        FindComponent<Text>((i) =>
        {

            Regex myRg = new Regex(@"<.*>.*</.*>");
            string tempStr = myRg.Match(i.text).Value;
            if (tempStr == null || tempStr == "")
            {
                if (i.supportRichText == true)
                {
                    i.supportRichText = false;
                    EditorUtility.SetDirty(i);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        });
        
    }
    /// <summary>
    /// ��ʱ����
    /// </summary>
    [MenuItem("Tools/�Ż�/�رն������߼��")]
    public static void CloseRaycast()
    {
        return;
        FindComponent<Text>((i) =>
        {
            var comps = i.GetComponents<Component>();
            if (comps.Count() == 3)
            {
                if(comps.Where(x=>x.GetType()==typeof(RectTransform)).Count()>=1&& comps.Where(x => x.GetType() == typeof(CanvasRenderer)).Count() >= 1&&comps.Where(x => x.GetType() == typeof(Text)).Count() >= 1)
                {
                    i.raycastTarget = false;
                    EditorUtility.SetDirty(i);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        });
        FindComponent<Image>((i) =>
        {
            var comps = i.GetComponents<Component>();
            if (comps.Count() == 3)
            {
                if (comps.Where(x => x.GetType() == typeof(RectTransform)).Count() >= 1 && comps.Where(x => x.GetType() == typeof(CanvasRenderer)).Count() >= 1 && comps.Where(x => x.GetType() == typeof(Image)).Count() >= 1)
                {
                    if (i.raycastTarget == true)
                    {
                        i.raycastTarget = false;
                        EditorUtility.SetDirty(i);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }
        });
    }

    [MenuItem("Tools/�������Addressable")]
    public static void TableToAddressable()
    {
        var paths = GetAllBytes(Application.dataPath + @"/ArtPacks/Datas/Tables");
        foreach (var path in paths)
        {
            FileInfo fileInfo = new FileInfo(path);
            AssetReference assetReference = new AssetReference(AssetDatabase.GUIDFromAssetPath(path.Replace(Application.dataPath, "Assets")).ToString());
            AddressableAssetGroup assetGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("Datas_Tables");
            if (assetGroup != null)
            {
                AddressableAssetEntry assetEntry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(assetReference.AssetGUID, assetGroup, false, false);
                assetEntry.address = fileInfo.Name.Split('.').First();
                assetEntry.SetLabel("TableData", true, true);
                AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, assetEntry, false, true);
            }
        }
    }
    public static List<string> GetAllBytes(string RootPath)
    {
        List<string> paths = new List<string>();
        paths.AddRange(Directory.GetFiles(RootPath, "*.bytes", SearchOption.TopDirectoryOnly));
        string[] dir = Directory.GetDirectories(RootPath);
        foreach (string path in dir)
        {
            paths.AddRange(GetAllBytes(path));
        }
        return paths;
    }

    [MenuItem("Tools/�Ż�/ת��Mask")]

    public static void ChangeMaskToRect()
    {
        FindComponent<ScrollRect>((i) =>
        {
            Transform viewPort = i.transform.GetChild(0);
            if (viewPort != null && viewPort.GetComponent<Image>() != null)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(viewPort.GetComponent<Image>(),true);
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                };
            }
            if (viewPort != null && viewPort.GetComponent<Mask>() != null)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(viewPort.GetComponent<Mask>(),true);
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                };
                if (viewPort != null && viewPort.GetComponent<RectMask2D>() == null)
                {
                    viewPort.GetOrAddComponent<RectMask2D>();
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        });
        FindComponent<ScrollRectEx>((i) =>
        {
            Transform viewPort = i.transform.GetChild(0);
            if (viewPort != null && viewPort.GetComponent<Image>() != null)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(viewPort.GetComponent<Image>(), true);
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                };
            }
            if (viewPort != null && viewPort.GetComponent<Mask>() != null)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(viewPort.GetComponent<Mask>(), true);
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                };
                if (viewPort != null && viewPort.GetComponent<RectMask2D>() == null)
                {
                    viewPort.GetOrAddComponent<RectMask2D>();
                    EditorUtility.SetDirty(viewPort);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        });
    }
    public static void FindComponent<T>(Action<T> action)
    {
        string[] paths=AssetDatabase.GetAllAssetPaths();
        var gameobjects = paths.Where(x => x.EndsWith("prefab")).Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));
        foreach(var target in gameobjects)
        {
            if (target == null)
            {
                continue;
            }
            T[] com = target.GetComponentsInChildren<T>(true);
            if (com != null && com.Length > 0)
            {
                foreach(var i in com)
                {
                    action(i);
                }
            }
        }
    }
    [MenuItem("Tools/�Ż�/�����")]
    public static void FindOne()
    {
        FindPrefabs<Mask>();
    }
    public static void FindPrefabs<T>()
    {
        string[] paths = AssetDatabase.GetAllAssetPaths();
        var gameobjects = paths.Where(x => x.EndsWith("prefab")).Select(x => x);
        foreach (var target in gameobjects)
        {
            var trueTarget = AssetDatabase.LoadAssetAtPath<GameObject>(target);
            if (trueTarget == null) {
                continue;
            }
            if (trueTarget.GetComponentInChildren<T>(true) != null)
            {
                Debug.Log(target);
                //PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(target));
            }
        }
    }
    /// <summary>
    /// ͼƬ��Asset�µ�·��
    /// </summary>
    const string ImagePathRoot = "Assets/ArtPacks/Textures";
    /// <summary>
    /// ͼ������·��
    /// </summary>
    const string SpriteAtlasRoot = "Assets/ArtPacks/Atlas";
    /// <summary>
    /// �����ļ�������ͼƬ��ͼ������
    /// </summary>
    [MenuItem("Tools/�Ż�/����ͼ��")]
    public static void MakeAtlas()
    {
        //��ɾ���͵�ͼ��
        if (Directory.Exists(SpriteAtlasRoot))
        {
            Directory.Delete(SpriteAtlasRoot, true);
        }
        Directory.CreateDirectory(SpriteAtlasRoot);
        var childDirects = Directory.GetDirectories(ImagePathRoot, "*", SearchOption.AllDirectories);
        foreach (var childDirect in childDirects)
        {
            CreateApriteAtlas(childDirect);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void CreateApriteAtlas(string path)
    {
        SpriteAtlas atlas = new SpriteAtlas();
        SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
        };
        atlas.SetPackingSettings(packSetting);

        SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        atlas.SetTextureSettings(textureSetting);

        TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
        {
            maxTextureSize = 2048,
            format = TextureImporterFormat.Automatic,
            crunchedCompression = true,
            textureCompression = TextureImporterCompression.Compressed,
            compressionQuality = 50,
        };
        atlas.SetPlatformSettings(platformSetting);
        // 1������ļ�
        DirectoryInfo dir = new DirectoryInfo(path);
        // ������ʹ�õ���pngͼƬ���Ѿ�����Sprite������
        List<FileInfo> files = dir.GetFiles("*.png", SearchOption.TopDirectoryOnly).ToList();
        files.AddRange(dir.GetFiles("*.jpg", SearchOption.TopDirectoryOnly));
        if (files?.Count() > 0)
        {
            foreach (FileInfo file in files)
            {
                //��ͳһ·��������
                var fullPath = file.FullName.Replace("\\", "/");
                var index = fullPath.IndexOf(ImagePathRoot);
                var filePath = fullPath.Substring(index);
                //�Ȱ������޸�ΪSprite
                var textureImport = (TextureImporter)AssetImporter.GetAtPath(filePath);
                textureImport.textureType = TextureImporterType.Sprite;
                textureImport.SaveAndReimport();

                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
                if (sprite != null)
                {
                    atlas.Add(new[] { sprite });
                }
            }
            string thisName = "";
            if (dir.Parent.Name != "Textures")
            {
                thisName += dir.Parent.Name + "-";
            }
            thisName += dir.Name;
            //���ﴴ������V1�İ汾
            AssetDatabase.CreateAsset(atlas, $"{Path.Combine(SpriteAtlasRoot, thisName)}.spriteatlas");
        }
    }
    enum FormatType
    {
        RGBA32 = 4,

        ETC_RGB4 = 34,
        ETC2_RGB4 = 45,
        ETC2_RGBA8 = 47,

        ASTC_RGB_5x5 = 49,
        ASTC_RGB_6x6 = 50,
        ASTC_RGB_8x8 = 51,

        ASTC_RGBA_5x5 = 55,
        ASTC_RGBA_6x6 = 56,
        ASTC_RGBA_8x8 = 57,
    }
    static int[] formatSize = new int[] { 32, 64, 128, 256, 512, 1024, 2048 };
    [MenuItem("Tools/�Ż�/����ѹ��")]
    static void SetOneImageFormat()
    {
        var childDirects = Directory.GetFiles(ImagePathRoot, "*", SearchOption.AllDirectories);
        foreach (var path in childDirects)
        {
            if (path.EndsWith("jpg") || path.EndsWith("png"))
            {
                //�ж�ͼƬ��С
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture != null)
                {
                    TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                    int textureSize = Mathf.Max(texture.height, texture.width);
                    //Debug.Log(textureSize);
                    int sizeType = FitSize(textureSize);

                    //����ͼƬѹ����ʽ
                    TextureSetting(importer, sizeType);

                }
                else
                {
                    Debug.Log("Texture2DΪnull��" + path);
                }
            }
        }
    }
    static int FitSize(int picValue)
    {
        foreach (var one in formatSize)
        {
            if (picValue <= one)
            {
                return one;
            }
        }
        return 1024;
    }
    static void TextureSetting(TextureImporter texture, int size = 2048)
    {
        // ȡ����ѡ "Generte Mip Maps"
        texture.mipmapEnabled = false;
        //���������ܶ�
        texture.spritePixelsPerUnit = 100;
        AndroidSetting(texture, size);
        //�����޸�
        texture.SaveAndReimport();
    }
    static void AndroidSetting(TextureImporter texture, int size)
    {
        var set = texture.GetPlatformTextureSettings("Android");
        set.overridden = false;

        /*��ע--
         ETC����֧��͸��ͨ����ͼƬ��߱�����2����������
         ETC2����ETC����չ��֧��͸��ͨ������ͼƬ���ֻҪ��4�ı�������
         ASTC��Android��IOSƽ̨�µ�һ�ָ�����ѹ����ʽ��֧��Android5.0��iPhone6���ϻ���
        */
        set.format = TextureImporterFormat.ETC2_RGB4;
        set.maxTextureSize = size;
        set.compressionQuality = 100;
        texture.SetPlatformTextureSettings(set);
    }
    public class AtlasCreate
    {
        public class TextureImporterSettings
        {
            public bool isReadable;
            public TextureImporterFormat textureImporterFormat;
            public TextureImporterSettings(bool isReadable, TextureImporterFormat textureImporterFormat)
            {
                this.isReadable = isReadable;
                this.textureImporterFormat = textureImporterFormat;
            }
        }
        public class SpriteInfo
        {
            public string name;//ͼƬ������
            public Vector4 spriteBorder;//ͼƬ�İ�Χ��(����еĻ�)
            public Vector2 spritePivot;//ͼƬ��Χ���е�������(����еĻ�)
            public float width;
            public float height;

            public SpriteInfo(string name, Vector4 border, Vector2 pivot, float w, float h)
            {
                this.name = name;
                spriteBorder = border;
                spritePivot = pivot;
                width = w;
                height = h;
            }
        }
        static float matAtlasSize = 2048;//���ͼ���ߴ�
        static float padding = 1;//ÿ����ͼƬ֮���ö�������������
        private static List<SpriteInfo> spriteList = new List<SpriteInfo>();

        [Obsolete]
        static public void Init()
        {
            string assetPath;
            //�������ǵ�ѡ������ȡѡ���������Ϣ
            Object[] objs = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
            //�ж�ͼƬ�����ĺϷ���
            for (int i = 0; i < objs.Length; i++)
            {
                Object obj = objs[i];
                if (obj.name.StartsWith(" ") || obj.name.EndsWith(" "))
                {
                    string newName = obj.name.TrimStart(' ').TrimEnd(' ');
                    Debug.Log(string.Format("rename texture'name old name : {0}, new name {1}", obj.name, newName));
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
                }
            }
            Texture2D[] texs = new Texture2D[objs.Length];//��������objs�е�����
            if (texs.Length <= 0)
            {
                Debug.Log("����ѡ��Ҫ�ϲ���Сͼ��Сͼ��Ŀ¼");
                return;
            }


            for (var i = 0; i < objs.Length; i++)
            {
                texs[i] = objs[i] as Texture2D;
                assetPath = AssetDatabase.GetAssetPath(texs[i]);
                AssetDatabase.ImportAsset(assetPath);//���°�ͼƬ�����ڴ棬������unity�����е���Դ���õ���ʱ��Unity���Զ����뵽�ڴ棬���е�ʱ��ȴû���Զ����룬Ϊ���Է���һ�������ֶ�����һ��
            }

            //�õ�ͼƬ��������Ϣ
            TextureImporterSettings[] originalSets = GatherSettings(texs);

            //�������ǵ����� ����ͼƬ��һЩ��Ϣ.
            for (int i = 0; i < texs.Length; i++)
            {
                SetupTexture(texs[i], true, TextureImporterFormat.RGBA32);
            }
            //���մ�ɵ�ͼ��·������������
            assetPath = "Assets/Atlas.png";
            string outputPath = Application.dataPath + "/../" + assetPath;
            //��Ҫ�Ĵ�ͼ������
            PackAndOutputSprites(texs, assetPath, outputPath);
            //���ͼ������Unityѡ����
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)));
        }

        //�õ�ͼƬ��������Ϣ
        [Obsolete]
        static public TextureImporterSettings[] GatherSettings(Texture2D[] texs)
        {
            TextureImporterSettings[] sets = new TextureImporterSettings[texs.Length];
            for (var i = 0; i < texs.Length; i++)
            {
                var tex = texs[i];
                var assetPath = AssetDatabase.GetAssetPath(tex);
                TextureImporter imp = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                sets[i] = new TextureImporterSettings(imp.isReadable, imp.textureFormat);
                //���ͼƬ�ɰ�Χ�еĻ� ��¼��Χ����Ϣ
                if (imp.textureType == TextureImporterType.Sprite && imp.spriteBorder != Vector4.zero)
                {
                    var spriteInfo = new SpriteInfo(tex.name, imp.spriteBorder, imp.spritePivot, tex.width, tex.height);
                    spriteList.Add(spriteInfo);
                }
            }
            return sets;
        }

        //�������ǵ����� ����ͼƬ��һЩ��Ϣ.
        [Obsolete]
        static public void SetupTexture(Texture2D tex, bool isReadable, TextureImporterFormat textureFormat)
        {
            var assetPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            importer.isReadable = isReadable;//ͼƬ�Ƿ�ɶ�ȡ�����ڴ���Ϣ
            importer.textureFormat = textureFormat;//ͼƬ�ĸ�ʽ
            importer.mipmapEnabled = false;//�Ƿ�����mipmap�ļ�
            importer.npotScale = TextureImporterNPOTScale.None;//���ڷǶ��������������ģʽ
            importer.SaveAndReimport();//ˢ��ͼƬ
        }

        [Obsolete]
        static public void PackAndOutputSprites(Texture2D[] texs, string atlasAssetPath, string outputPath)
        {
            Texture2D atlas = new Texture2D(1, 1);
            Rect[] rs = atlas.PackTextures(texs, (int)padding, (int)matAtlasSize);//��Ӷ��ͼƬ��һ��ͼ����,����ֵ��ÿ��ͼƬ��ͼ��(��ͼƬ)�е�U�������Ϣ
                                                                                  // ��ͼ��д�뵽�����ļ��������ڴ����ϻ���һ��ͼƬ���ɣ����ͼƬ�����˺ܶ�СͼƬ
            File.WriteAllBytes(outputPath, atlas.EncodeToPNG());
            RefreshAsset(atlasAssetPath);//ˢ��ͼƬ

            //��¼ͼƬ�����֣�ֻ�����������־��;
            StringBuilder names = new StringBuilder();
            //SpriteMetaData�ṹ���������Ǳ༭ͼƬ��һЩ��Ϣ,��ͼƬ��name,��Χ��border,��ͼ���е�����rect��
            SpriteMetaData[] sheet = new SpriteMetaData[rs.Length];
            for (var i = 0; i < sheet.Length; i++)
            {
                SpriteMetaData meta = new SpriteMetaData();
                meta.name = texs[i].name;
                meta.rect = rs[i];//�����rect��¼���ǵ���ͼƬ��ͼ���е�uv����ֵ
                                  //��Ϊrect������Ҫ��¼����ͼƬ�ڴ�ͼƬͼ�������ڵ�����rect���������������µĴ���
                meta.rect.Set(
                    meta.rect.x * atlas.width,
                    meta.rect.y * atlas.height,
                    meta.rect.width * atlas.width,
                    meta.rect.height * atlas.height
                );
                //���ͼƬ�а�Χ����Ϣ�Ļ�
                var spriteInfo = GetSpriteMetaData(meta.name);
                if (spriteInfo != null)
                {
                    meta.border = spriteInfo.spriteBorder;
                    meta.pivot = spriteInfo.spritePivot;
                }
                sheet[i] = meta;
                //��ӡ��־��
                names.Append(meta.name);
                if (i < sheet.Length - 1)
                    names.Append(",");
            }

            //����ͼ������Ϣ
            TextureImporter imp = TextureImporter.GetAtPath(atlasAssetPath) as TextureImporter;
            imp.textureType = TextureImporterType.Sprite;//ͼ��������
            imp.textureFormat = TextureImporterFormat.AutomaticCompressed;//ͼ���ĸ�ʽ
            imp.spriteImportMode = SpriteImportMode.Multiple;//Multiple��ʾ���������ͼƬ(ͼ��)�а����ܶ�СͼƬ
            imp.mipmapEnabled = false;//�Ƿ���mipmap
            imp.spritesheet = sheet;//����ͼ����СͼƬ����Ϣ(ÿ��ͼƬ���ڵ�����rect��)
                                    // ���沢ˢ��
            imp.SaveAndReimport();
            spriteList.Clear();
            //�����־
            Debug.Log("Atlas create ok. " + names.ToString());
        }
        //ˢ��ͼƬ
        static public void RefreshAsset(string assetPath)
        {
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(assetPath);
        }
        //�õ�ͼƬ����Ϣ
        static public SpriteInfo GetSpriteMetaData(string texName)
        {
            for (int i = 0; i < spriteList.Count; i++)
            {
                if (spriteList[i].name == texName)
                {
                    return spriteList[i];
                }
            }
            //Debug.Log("Can not find texture metadata : " + texName);
            return null;
        }
    }
}
