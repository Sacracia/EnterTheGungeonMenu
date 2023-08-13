using HarmonyLib;
using UnityEngine;

namespace ETGCheatMenu
{
    public class Loader
    {
        internal static GameObject s_load;
        internal static Harmony s_harmony;

        public static GameObject Load { get { return s_load; } }

        public static void Init()
        {
            s_harmony = new Harmony("etg.mod.sacracia");
            s_load = new GameObject();
            s_load.AddComponent<Hax>();
            s_load.AddComponent<PlayerMenu>();
            s_load.AddComponent<StatsMenu>();
            s_load.AddComponent<WeaponMenu>();
            s_load.AddComponent<LevelMenu>();
            s_load.AddComponent<BreachMenu>();
            Object.DontDestroyOnLoad(s_load);
        }

        public static void Unload()
        {
            _Unload();
        }

        static void _Unload()
        {
            Object.Destroy(s_load);
            s_harmony.UnpatchAll();
        }
    }
}
