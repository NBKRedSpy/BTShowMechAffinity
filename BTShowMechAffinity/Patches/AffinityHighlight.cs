using BattleTech;
using BattleTech.Data;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using Harmony;
using MechAffinity;
using MechAffinity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace BTShowMechAffinity.Patches
{
    /// <summary>
    /// When a mech is clicked in the Contract Loadout screen, the pilots that have an affinity 
    /// are highlighted.
    /// </summary>
    [HarmonyPatch(typeof(LanceConfiguratorPanel), nameof(LanceConfiguratorPanel.OnButtonClicked))]
    public class AffinityHighlight
    {

        public static FieldInfo CurrentRosterFieldInfo { get; set; }
        public static FieldInfo RosterFieldInfo { get; set; }
        public static FieldInfo LoadoutSlotsFieldInfo { get; set; }
        public static FieldInfo RosterSlotsFieldInfo { get; set; }
        public static FieldInfo PilotTypeFieldInfo { get; set; }
        public static MethodInfo GetPrefIdMethod { get; set; }
        public static MethodInfo BarracksRosterListApplySort { get; set; }

        /// <summary>
        /// True if the mech affinity is enabled and loaded.
        /// </summary>
        public static bool MechAffinityIsLoaded { get; set; }

        public static void Postfix(LanceConfiguratorPanel __instance, IMechLabDraggableItem item)
        {
            try
            {
                if (InitReflectionTypes() == false) return; //exit if the MechAffinity mod is not loaded


                MechDef selectedMech = item.MechDef;
                if (selectedMech == null)
                {
                    return;
                }

                Dictionary<string, SGBarracksRosterSlot> currentRoster =
                    (Dictionary<string, SGBarracksRosterSlot>)CurrentRosterFieldInfo.GetValue(__instance.pilotListWidget);

                SimGameState simulation = UnityGameInstance.BattleTechGame.Simulation;


                List<SGBarracksRosterSlot> allPilotSlots = new List<SGBarracksRosterSlot>(currentRoster.Values);

                //Add any lance loadout slots to simplify the loop below.
                LanceLoadoutSlot[] loadoutSlots = (LanceLoadoutSlot[])LoadoutSlotsFieldInfo.GetValue(__instance);

                //Mech pilot assignment
                allPilotSlots.AddRange(loadoutSlots.Where(x => x.SelectedPilot?.Pilot != null).Select(x => x.SelectedPilot));

                foreach (SGBarracksRosterSlot rosterSlot in allPilotSlots)
                {
                    var pilotColor = (UIColorRefTracker)PilotTypeFieldInfo.GetValue(rosterSlot);

                    int deploymentCount;

                    deploymentCount = GetMechDeploymentCount(rosterSlot.Pilot, selectedMech);

                    SetDeploymentCountColor(pilotColor, simulation, deploymentCount);
                }

                //Sort the Barracks Roster List on the right side of the loadout screen.
                List<SGBarracksRosterSlot> list = new List<SGBarracksRosterSlot>(currentRoster.Values);

                //Order by deployment.  The UI seems to insert in reverse order.
                list = list.OrderBy(x => GetMechDeploymentCount(x.Pilot, selectedMech))
                    .ThenByDescending(x => x.Pilot?.Description?.DisplayName)
                    .ToList();

                BarracksRosterListApplySort.Invoke(__instance.pilotListWidget, new object[] { list });
                __instance.pilotListWidget.ForceRefreshImmediate();

            }
            catch (Exception ex)
            {
                Logger.Log(ex);

            }
        }

        /// <summary>
        /// Initializes the reflection info used in this patch.
        /// </summary>
        /// <returns>False if the MechAffinity mod is not loaded.</returns>
        public static bool InitReflectionTypes()
        {
            if (CurrentRosterFieldInfo != null) return true;

            GetPrefIdMethod = AccessTools.Method(typeof(PilotAffinityManager), "getPrefabId",
                new Type[] { typeof(MechDef), typeof(EIdType) });

            //Check if the Mech Affinity mod was loaded.
            if (GetPrefIdMethod == null) return false;

            CurrentRosterFieldInfo = AccessTools.Field(typeof(SGBarracksRosterList), "currentRoster");
            PilotTypeFieldInfo = AccessTools.Field(typeof(SGBarracksRosterSlot), "pilotTypeBackground");
            LoadoutSlotsFieldInfo = AccessTools.Field(typeof(LanceConfiguratorPanel), "loadoutSlots");
            BarracksRosterListApplySort = AccessTools.Method(typeof(SGBarracksRosterList), "ApplySort",
                new Type[]
                {
                    typeof(List<SGBarracksRosterSlot>)
                });


            return true;
        }

        public static int GetMechDeploymentCount(Pilot pilot, MechDef mechDef)
        {
            //todo:  Optimize method infos and check if the mechaffinity assembly is even loaded.

            InitReflectionTypes();

            string mechPrefabid;

            mechPrefabid = (string)GetPrefIdMethod.Invoke(PilotAffinityManager.Instance, new object[] { mechDef, EIdType.PrefabId });

            int deploymentCount;

            deploymentCount = PilotAffinityManager.Instance.getDeploymentCountWithMech(pilot, mechPrefabid);

            return deploymentCount;

        }
        public static void SetDeploymentCountColor(UIColorRefTracker pilotColor, SimGameState simulation, int deploymentCount)
        {
            if (deploymentCount == 0)
            {
                pilotColor.OverrideWithColor(Color.black);
            }
            else if (deploymentCount < 10)
            {
                pilotColor.OverrideWithColor(Color.grey);
            }
            else if (deploymentCount < 20)
            {
                pilotColor.OverrideWithColor(new Color(48 / 255f, 146 / 255f, 49 / 255f)); //Soft Green
            }
            else if (deploymentCount < 30)
            {
                pilotColor.OverrideWithColor(new Color(77 / 255f, 81 / 255f, 248 / 255f));    //Softer blue
            }
            else if (deploymentCount < 50)
            {
                pilotColor.OverrideWithColor(new Color(282 / 255f, 59 / 255f, 73 / 255f)); //Purple
            }
            else
            {
                pilotColor.OverrideWithColor(new Color(242 / 255f, 175 / 255f, 27 / 255f));    //Orange-gold
            }
        }
    }




}


