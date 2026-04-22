using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mastercrafted_Arms.Patches
{
    public static class GenRecipePatches
    {
        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        public static class PostProcessProduct
        {
            public static void Postfix(ref Thing __result, Thing product, RecipeDef recipeDef, Pawn worker)
            {
                if (!ShouldConvertToUniqueWeapon(product, worker))
                {
                    return;
                }

                Thing convertedThing = ConvertToUniqueWeapon(product, worker);
                if (convertedThing != null)
                {
                    __result = convertedThing;
                    Toils_RecipePatches.uniqueWeapon = convertedThing;
                }
            }

            private static bool ShouldConvertToUniqueWeapon(Thing product, Pawn worker)
            {
                if (product == null || worker == null || !Toils_RecipePatches.IsLevel20Crafter(worker))
                {
                    return false;
                }

                if (!product.TryGetQuality(out QualityCategory craftedQuality) || craftedQuality < ModSettings.minimumUniqueQuality)
                {
                    return false;
                }

                if (Utils.GetUniqueVariant(product.def) == null)
                {
                    return false;
                }

                return Rand.Chance(ModSettings.craftSuccessChance);
            }

            private static Thing ConvertToUniqueWeapon(Thing product, Pawn worker)
            {
                ThingDef uniqueVariantDef = Utils.GetUniqueVariant(product.def);
                if (uniqueVariantDef == null || !product.TryGetQuality(out QualityCategory craftedQuality))
                {
                    return null;
                }

                Thing uniqueThing = ThingMaker.MakeThing(uniqueVariantDef, product.Stuff);
                uniqueThing.stackCount = product.stackCount;

                if (product.def.useHitPoints && uniqueThing.def.useHitPoints)
                {
                    uniqueThing.HitPoints = product.HitPoints;
                }

                if (product.Faction != null && uniqueThing.def.CanHaveFaction)
                {
                    uniqueThing.SetFactionDirect(product.Faction);
                }

                if (product.questTags != null)
                {
                    uniqueThing.questTags = new List<string>(product.questTags);
                }

                uniqueThing.overrideGraphicIndex = product.overrideGraphicIndex;

                if (product.StyleSourcePrecept != null)
                {
                    uniqueThing.StyleSourcePrecept = product.StyleSourcePrecept;
                }
                else if (product.StyleDef != null)
                {
                    uniqueThing.StyleDef = product.StyleDef;
                }

                CopyColor(product, uniqueThing);
                CopyIngredients(product, uniqueThing);

                uniqueThing.Notify_RecipeProduced(worker);
                uniqueThing.TryGetComp<CompQuality>()?.SetQuality(craftedQuality, ArtGenerationContext.Colony);

                return uniqueThing;
            }

            private static void CopyColor(Thing sourceThing, Thing destinationThing)
            {
                CompColorable sourceComp = sourceThing.TryGetComp<CompColorable>();
                CompColorable destinationComp = destinationThing.TryGetComp<CompColorable>();
                if (sourceComp != null && destinationComp != null && sourceComp.Active)
                {
                    destinationComp.SetColor(sourceComp.Color);
                }
            }

            private static void CopyIngredients(Thing sourceThing, Thing destinationThing)
            {
                CompIngredients sourceComp = sourceThing.TryGetComp<CompIngredients>();
                CompIngredients destinationComp = destinationThing.TryGetComp<CompIngredients>();
                if (sourceComp == null || destinationComp == null)
                {
                    return;
                }

                for (int i = 0; i < sourceComp.ingredients.Count; i++)
                {
                    destinationComp.ingredients.AddUnique(sourceComp.ingredients[i]);
                }
            }
        }
    }
}
