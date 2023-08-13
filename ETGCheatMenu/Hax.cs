using UnityEngine;

namespace ETGCheatMenu
{
    internal class Hax : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                PlayerMenu.Instance._visible = !PlayerMenu.Instance._visible;
                StatsMenu.Instance._visible = !StatsMenu.Instance._visible;
                WeaponMenu.Instance._visible = !WeaponMenu.Instance._visible;
                LevelMenu.Instance._visible = !LevelMenu.Instance._visible;
                BreachMenu.Instance._visible = !BreachMenu.Instance._visible;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }
    }
}
