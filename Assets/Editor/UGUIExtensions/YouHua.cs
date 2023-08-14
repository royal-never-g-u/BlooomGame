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
    [MenuItem("Tools/优化/未知")]
    public static void FindComponentInPrefab()
    {

    }
    [MenuItem("Tools/优化/关闭富文本")]
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
    /// 暂时禁用
    /// </summary>
    [MenuItem("Tools/优化/关闭多余射线检测")]
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

    [MenuItem("Tools/表格设置Addressable")]
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

    [MenuItem("Tools/优化/转化Mask")]

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
    [MenuItem("Tools/优化/找组件")]
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
    /// 图片在Asset下的路径
    /// </summary>
    const string ImagePathRoot = "Assets/ArtPacks/Textures";
    /// <summary>
    /// 图集保存路径
    /// </summary>
    const string SpriteAtlasRoot = "Assets/ArtPacks/Atlas";
    /// <summary>
    /// 按照文件夹设置图片的图集名称
    /// </summary>
    [MenuItem("Tools/优化/生成图集")]
    public static void MakeAtlas()
    {
        //先删除就的图集
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
        // 1、添加文件
        DirectoryInfo dir = new DirectoryInfo(path);
        // 这里我使用的是png图片，已经生成Sprite精灵了
        List<FileInfo> files = dir.GetFiles("*.png", SearchOption.TopDirectoryOnly).ToList();
        files.AddRange(dir.GetFiles("*.jpg", SearchOption.TopDirectoryOnly));
        if (files?.Count() > 0)
        {
            foreach (FileInfo file in files)
            {
                //先统一路径表达符号
                var fullPath = file.FullName.Replace("\\", "/");
                var index = fullPath.IndexOf(ImagePathRoot);
                var filePath = fullPath.Substring(index);
                //先把纹理修改为Sprite
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
            //这里创建的是V1的版本
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
    [MenuItem("Tools/优化/设置压缩")]
    static void SetOneImageFormat()
    {
        var childDirects = Directory.GetFiles(ImagePathRoot, "*", SearchOption.AllDirectories);
        foreach (var path in childDirects)
        {
            if (path.EndsWith("jpg") || path.EndsWith("png"))
            {
                //判断图片大小
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture != null)
                {
                    TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                    int textureSize = Mathf.Max(texture.height, texture.width);
                    //Debug.Log(textureSize);
                    int sizeType = FitSize(textureSize);

                    //设置图片压缩格式
                    TextureSetting(importer, sizeType);

                }
                else
                {
                    Debug.Log("Texture2D为null：" + path);
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
        // 取消勾选 "Generte Mip Maps"
        texture.mipmapEnabled = false;
        //设置像素密度
        texture.spritePixelsPerUnit = 100;
        AndroidSetting(texture, size);
        //保存修改
        texture.SaveAndReimport();
    }
    static void AndroidSetting(TextureImporter texture, int size)
    {
        var set = texture.GetPlatformTextureSettings("Android");
        set.overridden = false;

        /*备注--
         ETC：不支持透明通道，图片宽高必须是2的整数次幂
         ETC2：是ETC的扩展，支持透明通道，且图片宽高只要是4的倍数即可
         ASTC是Android和IOS平台下的一种高质量压缩方式，支持Android5.0和iPhone6以上机型
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
            public string name;//图片的名字
            public Vector4 spriteBorder;//图片的包围盒(如果有的话)
            public Vector2 spritePivot;//图片包围盒中的中心轴(如果有的话)
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
        static float matAtlasSize = 2048;//最大图集尺寸
        static float padding = 1;//每两个图片之间用多少像素来隔开
        private static List<SpriteInfo> spriteList = new List<SpriteInfo>();

        [Obsolete]
        static public void Init()
        {
            string assetPath;
            //根据我们的选择来获取选中物体的信息
            Object[] objs = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
            //判断图片命名的合法性
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
            Texture2D[] texs = new Texture2D[objs.Length];//用来保存objs中的物体
            if (texs.Length <= 0)
            {
                Debug.Log("请先选择要合并的小图或小图的目录");
                return;
            }


            for (var i = 0; i < objs.Length; i++)
            {
                texs[i] = objs[i] as Texture2D;
                assetPath = AssetDatabase.GetAssetPath(texs[i]);
                AssetDatabase.ImportAsset(assetPath);//重新把图片导入内存，理论上unity工程中的资源在用到的时候，Unity会自动导入到内存，但有的时候却没有自动导入，为了以防万一，我们手动导入一次
            }

            //得到图片的设置信息
            TextureImporterSettings[] originalSets = GatherSettings(texs);

            //根据我们的需求 设置图片的一些信息.
            for (int i = 0; i < texs.Length; i++)
            {
                SetupTexture(texs[i], true, TextureImporterFormat.RGBA32);
            }
            //最终打成的图集路径，包括名字
            assetPath = "Assets/Atlas.png";
            string outputPath = Application.dataPath + "/../" + assetPath;
            //主要的打图集代码
            PackAndOutputSprites(texs, assetPath, outputPath);
            //打出图集后在Unity选中它
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)));
        }

        //得到图片的设置信息
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
                //如果图片由包围盒的话 记录包围盒信息
                if (imp.textureType == TextureImporterType.Sprite && imp.spriteBorder != Vector4.zero)
                {
                    var spriteInfo = new SpriteInfo(tex.name, imp.spriteBorder, imp.spritePivot, tex.width, tex.height);
                    spriteList.Add(spriteInfo);
                }
            }
            return sets;
        }

        //根据我们的需求 设置图片的一些信息.
        [Obsolete]
        static public void SetupTexture(Texture2D tex, bool isReadable, TextureImporterFormat textureFormat)
        {
            var assetPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            importer.isReadable = isReadable;//图片是否可读取它的内存信息
            importer.textureFormat = textureFormat;//图片的格式
            importer.mipmapEnabled = false;//是否生成mipmap文件
            importer.npotScale = TextureImporterNPOTScale.None;//用于非二次幂纹理的缩放模式
            importer.SaveAndReimport();//刷新图片
        }

        [Obsolete]
        static public void PackAndOutputSprites(Texture2D[] texs, string atlasAssetPath, string outputPath)
        {
            Texture2D atlas = new Texture2D(1, 1);
            Rect[] rs = atlas.PackTextures(texs, (int)padding, (int)matAtlasSize);//添加多个图片到一个图集中,返回值是每个图片在图集(大图片)中的U坐标等信息
                                                                                  // 把图集写入到磁盘文件，最终在磁盘上会有一个图片生成，这个图片包含了很多小图片
            File.WriteAllBytes(outputPath, atlas.EncodeToPNG());
            RefreshAsset(atlasAssetPath);//刷新图片

            //记录图片的名字，只是用于输出日志用;
            StringBuilder names = new StringBuilder();
            //SpriteMetaData结构可以让我们编辑图片的一些信息,想图片的name,包围盒border,在图集中的区域rect等
            SpriteMetaData[] sheet = new SpriteMetaData[rs.Length];
            for (var i = 0; i < sheet.Length; i++)
            {
                SpriteMetaData meta = new SpriteMetaData();
                meta.name = texs[i].name;
                meta.rect = rs[i];//这里的rect记录的是单个图片在图集中的uv坐标值
                                  //因为rect最终需要记录单个图片在大图片图集中所在的区域rect，所以我们做如下的处理
                meta.rect.Set(
                    meta.rect.x * atlas.width,
                    meta.rect.y * atlas.height,
                    meta.rect.width * atlas.width,
                    meta.rect.height * atlas.height
                );
                //如果图片有包围盒信息的话
                var spriteInfo = GetSpriteMetaData(meta.name);
                if (spriteInfo != null)
                {
                    meta.border = spriteInfo.spriteBorder;
                    meta.pivot = spriteInfo.spritePivot;
                }
                sheet[i] = meta;
                //打印日志用
                names.Append(meta.name);
                if (i < sheet.Length - 1)
                    names.Append(",");
            }

            //设置图集的信息
            TextureImporter imp = TextureImporter.GetAtPath(atlasAssetPath) as TextureImporter;
            imp.textureType = TextureImporterType.Sprite;//图集的类型
            imp.textureFormat = TextureImporterFormat.AutomaticCompressed;//图集的格式
            imp.spriteImportMode = SpriteImportMode.Multiple;//Multiple表示我们这个大图片(图集)中包含很多小图片
            imp.mipmapEnabled = false;//是否开启mipmap
            imp.spritesheet = sheet;//设置图集中小图片的信息(每个图片所在的区域rect等)
                                    // 保存并刷新
            imp.SaveAndReimport();
            spriteList.Clear();
            //输出日志
            Debug.Log("Atlas create ok. " + names.ToString());
        }
        //刷新图片
        static public void RefreshAsset(string assetPath)
        {
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(assetPath);
        }
        //得到图片的信息
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
