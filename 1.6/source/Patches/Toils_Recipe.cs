using HarmonyLib;
using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace Mastercrafted_Arms.Patches
{
    public static class Toils_RecipePatches
    {
        private const int RequiredCraftingLevel = 20;
        public static Thing uniqueWeapon;

        public static bool IsLevel20Crafter(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }

            int craftingLevel = pawn.RaceProps.IsMechanoid
                ? pawn.RaceProps.mechFixedSkillLevel
                : pawn.skills?.GetSkill(SkillDefOf.Crafting)?.Level ?? -1;

            return craftingLevel >= RequiredCraftingLevel;
        }

        [HarmonyPatch(typeof(Toils_Recipe), "FinishRecipeAndStartStoringProduct")]
        public static class FinishRecipeAndStartStoringProduct
        {
            public static void Postfix(ref Toil __result)
            {
                Toil originalToil = __result;
                Action originalInitAction = __result.initAction;
                __result.initAction = delegate
                {
                    uniqueWeapon = null;
                    originalInitAction();

                    if (uniqueWeapon != null && ModSettings.showCustomizationMenu)
                    {
                        CompUniqueWeapon comp = uniqueWeapon.TryGetComp<CompUniqueWeapon>();
                        if (comp != null)
                        {
                            comp.TraitsListForReading.Clear();
                            Find.WindowStack.Add(new Dialog_UniqueWeaponCustomization(uniqueWeapon));
                        }
                    }

                    uniqueWeapon = null;
                };
            }
        }
    }
}
