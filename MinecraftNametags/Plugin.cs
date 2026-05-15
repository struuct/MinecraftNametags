using System.Linq;
using System.Reflection;
using BepInEx;
using MinecraftNametags.Behaviours;
using UnityEngine;

namespace MinecraftNametags;

[BepInPlugin(Constants.Guid, Constants.ModName, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin? Instance;
    public AssetBundle? Bundle;

    private void Awake()
    {
        Instance = this;

        Bundle = AssetBundleUtility.LoadBundle(
            Assembly.GetExecutingAssembly(),
            "MinecraftNametags.Resources.minecraftnametags"
        );

        GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
    }

    public void OnPlayerSpawned()
    {
        foreach (var rigObject in FindObjectsByType<GameObject>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None
                 ).Where(x => x.name == "Gorilla Player Networked(Clone)"))
        {
            rigObject.AddComponent<Nametag>();
        }
    }
}