using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace MinecraftNametags;

// entirely from https://github.com/GorillaTagModdingHub/GorillaLibrary
public static class AssetBundleUtility
{
    public static AssetBundle LoadBundle(Assembly assembly, string path)
    {
        Stream stream = assembly.GetManifestResourceStream(path);

        if (stream == null)
            throw new System.Exception("AssetBundle stream was null: " + path);

        AssetBundle bundle = AssetBundle.LoadFromStream(stream);
        stream.Close();

        return bundle;
    }

    public static async Task<AssetBundle> LoadBundleAsync(Assembly assembly, string path)
    {
        Stream stream = assembly.GetManifestResourceStream(path);

        if (stream == null)
            throw new System.Exception("AssetBundle stream was null: " + path);

        AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);

        while (!request.isDone)
            await Task.Yield();

        stream.Close();

        return request.assetBundle;
    }

    public static async Task<T> LoadAssetAsync<T>(AssetBundle bundle, string name) where T : Object
    {
        AssetBundleRequest request = bundle.LoadAssetAsync<T>(name);

        while (!request.isDone)
            await Task.Yield();

        return request.asset as T;
    }

    public static async Task<T[]> LoadAssetsWithSubAssetsAsync<T>(AssetBundle bundle, string name) where T : Object
    {
        AssetBundleRequest request = bundle.LoadAssetWithSubAssetsAsync<T>(name);

        while (!request.isDone)
            await Task.Yield();

        return request.allAssets.Cast<T>().ToArray();
    }
}