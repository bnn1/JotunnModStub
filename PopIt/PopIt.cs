// PopIt
// a Valheim mod skeleton using Jötunn
// 
// File:    PopIt.cs
// Project: PopIt

using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using UnityEngine.UIElements;
using static ItemSets;

namespace PopIt
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class PopIt : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.PopIt";
        public const string PluginName = "PopIt";
        public const string PluginVersion = "0.0.1";

        private ButtonConfig PopButton;
        private ConfigEntry<KeyboardShortcut> PopConfig;
        private ConfigEntry<float> PopRadiusConfig;

        public void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            PopConfig = Config.Bind("Hotkey", "Pop pickables", new KeyboardShortcut(KeyCode.L), new ConfigDescription("Press this button to pop pickables"));
            PopRadiusConfig = Config.Bind("Radius", "Pop radius", 3.0f, new ConfigDescription("Radius at which to pop pickables"));
        }

        public void AddInputs()
        {
            PopButton = new ButtonConfig
            {
                Name = "PopShortcut",
                ShortcutConfig = PopConfig,
                RepeatInterval = 1f
            };
            InputManager.Instance.AddButton(PluginGUID, PopButton);
        }

        private void PopPickables()
        {
            Vector3 playerPosition = Player.m_localPlayer.transform.localPosition;
            Collider[] hitCollider = Physics.OverlapSphere(playerPosition, PopRadiusConfig.Value);

            foreach(Collider collider in hitCollider)
            {
                GameObject go = collider.gameObject;
                Interactable componentInParent = go.GetComponentInParent<Interactable>();
                if (componentInParent is Pickable)
                {
                    Pickable pickable = (Pickable)componentInParent;
                    pickable.Interact(Player.m_localPlayer, false, false);
                }
            }


        }

        private void Update()
        {
            if (ZInput.instance != null)
            {
                if (ZInput.GetButtonDown(PopButton.Name))
                {
                    PopPickables();
                }
            }
        }
        private void Awake()
        {
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("PopIt has landed");
            CreateConfigValues();
            AddInputs();

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html
        }
    }
}


