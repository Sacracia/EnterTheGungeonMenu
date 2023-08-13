using UnityEngine;

namespace ETGCheatMenu
{
    internal class PlayerMenu : MonoBehaviour
    {
        internal bool _visible = true;
        Rect _window = new Rect(10f, 10f, 160f, 100f);
        bool _godmode = false;
        bool _invulnerable = false;
        bool _ghost = false;
        bool _teleport = false;
        bool _colision = false;
        bool _invisible = false;

        static PlayerMenu s_instance;

        public static PlayerMenu Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = (PlayerMenu)UnityEngine.Object.FindObjectOfType(typeof(PlayerMenu));
                }
                if (s_instance == null)
                {
                    s_instance = Loader.Load.AddComponent<PlayerMenu>();
                }
                return s_instance;
            }
        }

        public bool Ghost
        {
            set
            {
                PlayerController localPlayer = GameManager.Instance?.PrimaryPlayer;
                if (localPlayer != null)
                {
                    if (value)
                    {
                        typeof(PlayerController).GetMethod("BecomeGhost", System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Instance).Invoke(localPlayer, new object[0]);
                    }
                    else
                    {
                        localPlayer.IsGhost = false;
                    }
                }
            }
        }


        void DrawElements()
        {
            bool flag = GUILayout.Toggle(_godmode, "Godmode", new GUILayoutOption[0]);
            if (flag != _godmode)
            {
                _godmode = flag;
                Patches.ApplyPatch(typeof(HealthHaver), "ApplyDamageDirectional", () => Patches.ApplyDamageDirectional(null), flag, true);
            }
            flag = GUILayout.Toggle(_invulnerable, "Invulnerable", new GUILayoutOption[0]);
            if (flag != _invulnerable)
            {
                _invulnerable = flag;
                HealthHaver healthHaver = GameManager.Instance?.PrimaryPlayer?.healthHaver;
                if (!flag && healthHaver != null && !healthHaver.IsVulnerable)
                {
                    healthHaver.IsVulnerable = true;
                }
            }
            flag = GUILayout.Toggle(_ghost, "Ghost", new GUILayoutOption[0]);
            if (flag != _ghost)
            {
                _ghost = flag;
                this.Ghost = flag;
            }
            _teleport = GUILayout.Toggle(_teleport, "Teleport (F2)", new GUILayoutOption[0]);
            flag = GUILayout.Toggle(_colision, "No collision", new GUILayoutOption[0]);
            if (flag != _colision)
            {
                _colision = flag;
                PlayerController localPlayer = GameManager.Instance?.PrimaryPlayer;
                if (localPlayer != null)
                {
                    SpeculativeRigidbody specRigidbody = localPlayer.specRigidbody;
                    specRigidbody.PrimaryPixelCollider.Enabled = !flag;
                }
            }
            flag = GUILayout.Toggle(_invisible, "Invisible", new GUILayoutOption[0]);
            if (flag != _invisible)
            {
                _invisible = flag;
                SpeculativeRigidbody temp = null;
                Patches.ApplyPatchGetter(typeof(AIActor), "TargetRigidbody", () => Patches.TargetRigidbody(ref temp), flag, true);
            }
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        public void OnGUI()
        {
            if (_visible)
            {
                _window = GUILayout.Window(0, _window, OnWindow, "Player", new GUILayoutOption[0]);
            }
        }

        private void TeleportToCrosshair()
        {
            PlayerController localPlayer = GameManager.Instance.PrimaryPlayer;
            if (localPlayer != null)
            {
                Vector3 v3 = (Vector3)typeof(PlayerController).GetMethod("DetermineAimPointInWorld", System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance).Invoke(localPlayer, new object[0]);
                if (v3 != null)
                {
                    localPlayer.specRigidbody.Position = new Position(v3.XY());
                }
            }
        }

        public void Update()
        {
            PlayerController localPlayer = GameManager.Instance?.PrimaryPlayer;
            if (_invulnerable && localPlayer.healthHaver.IsVulnerable)
            {
                localPlayer.healthHaver.IsVulnerable = false;
            }
            if (Input.GetKeyDown(KeyCode.F2) && _teleport)
            {
                TeleportToCrosshair();
            }
        }
    }
}
