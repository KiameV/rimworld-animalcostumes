using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace AnimalCostumes
{
    [StaticConstructorOnStartup]
    partial class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("com.animalcostumes.rimworld.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            /*Log.Message(
                "AnimalCostumes Harmony Patches:" + Environment.NewLine +
                "  Prefix: " + Environment.NewLine +
                "    Pawn_ApparelTracker.Notify_ApparelAdded (Priority.First)" + Environment.NewLine +
                "    Pawn_ApparelTracker.Notify_ApparelRemoved (Priority.First)" + Environment.NewLine +
                "  Postfix:" + Environment.NewLine +
                "    Pawn.GetInspectString");*/
        }

        [HarmonyPatch(typeof(Pawn), "GetInspectString")]
        static class Patch_Pawn_GetInspectString
        {
            static Patch_Pawn_GetInspectString() { }

            public static void Postfix(Pawn __instance, ref string __result)
            {
                if (__instance != null && __instance.apparel != null && __instance.apparel.WornApparel != null)
                {
                    foreach (Apparel a in __instance.apparel.WornApparel)
                    {
                        if (a is AnimalCostume ac)
                        {
                            ac.AddToInspectionString(ref __result);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelAdded")]
        static class Patch_Pawn_ApparelTracker_Notify_ApparelAdded
        {
            [HarmonyPriority(Priority.First)]
            public static void Postfix(Apparel apparel)
            {
                if (apparel is AnimalCostume ac)
                {
                    CompPawnPowered cpp = apparel.TryGetComp<CompPawnPowered>();
                    if (cpp != null)
                    {
                        cpp.SetRemainingCharges();
                    }
                    WorldComp.Add(ac);
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
        static class Patch_Pawn_ApparelTracker_Notify_ApparelRemoved
        {
            [HarmonyPriority(Priority.First)]
            public static void Postfix(Apparel apparel)
            {
                if (apparel is AnimalCostume ac)
                    WorldComp.Remove(ac);
            }
        }

        [HarmonyPatch(typeof(MassUtility), nameof(MassUtility.Capacity))]
        static class Patch_MassUtility_Capacity
        {
            public static void Postfix(ref float __result, Pawn p)
            {
                bool body = false, head = false;
                if (p.RaceProps.Humanlike && p.Faction?.IsPlayerSafe() == true)
                {
                    foreach(Apparel a in p.apparel.WornApparel)
                    {
                        if (a.def is AnimalCostumeDef d)
                        {
                            if (d.CostumeType == CostumeType.Body)
                                body = true;
                            else if (d.CostumeType == CostumeType.Head)
                                head = true;
                        }
                    }
                }
                if (body && head)
                    __result += 5;
            }
        }

        [HarmonyPatch(typeof(CompApparelReloadable), nameof(CompApparelReloadable.UsedOnce))]
        static class Patch_CompReloadable_UsedOnce
        {
            public static bool Prefix(ref CompApparelReloadable __instance)
            {
                if (__instance is CompPawnPowered cpp) {
                    cpp.UsedOnceNew();
                    return false;
                }
                return true;
            }
        }
    }
}
