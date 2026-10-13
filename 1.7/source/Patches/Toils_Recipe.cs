using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.AI;

namespace Mastercrafted_Arms.Patches
{
    public static class Toils_RecipePatches
    {
        // Infix on the one RecordsUtility.Notify_BillDone call inside the init action of
        // Toils_Recipe.FinishRecipeAndStartStoringProduct. Vanilla calls it exactly once, with the final product list,
        // so this replaces wrapping the toil's initAction delegate and sharing the converted weapon through a static field.
        [HarmonyPatch]
        public static class FinishRecipeAndStartStoringProduct
        {
            private static MethodBase initActionLambda;

            public static bool Prepare()
            {
                if (initActionLambda == null)
                {
                    MethodInfo toilMethod = AccessTools.Method(typeof(Toils_Recipe), nameof(Toils_Recipe.FinishRecipeAndStartStoringProduct));
                    MethodInfo notifyBillDone = AccessTools.Method(typeof(RecordsUtility), nameof(RecordsUtility.Notify_BillDone));
                    if (toilMethod != null && notifyBillDone != null)
                    {
                        initActionLambda = AccessTools.Lambdas(toilMethod).FirstOrDefault(lambda => CallsMethod(lambda, notifyBillDone));
                    }
                }

                if (initActionLambda == null)
                {
                    Log.Warning("[Mastercrafted Arms] Could not find the recipe-finished action in Toils_Recipe.FinishRecipeAndStartStoringProduct; the weapon customization menu is disabled.");
                    return false;
                }

                return true;
            }

            public static MethodBase TargetMethod()
            {
                return initActionLambda;
            }

            [HarmonyPostfix]
            [HarmonyInfix(typeof(RecordsUtility), nameof(RecordsUtility.Notify_BillDone))]
            public static void Postfix(List<Thing> products)
            {
                if (products == null)
                {
                    return;
                }

                for (int i = 0; i < products.Count; i++)
                {
                    Thing product = products[i];
                    if (product == null || !Utils.TryConsumeConvertedWeapon(product) || !ModSettings.showCustomizationMenu)
                    {
                        continue;
                    }

                    CompUniqueWeapon comp = product.TryGetComp<CompUniqueWeapon>();
                    if (comp != null)
                    {
                        comp.TraitsListForReading.Clear();
                        Find.WindowStack.Add(new Dialog_UniqueWeaponCustomization(product));
                    }
                }
            }

            private static bool CallsMethod(MethodBase body, MethodBase callee)
            {
                return PatchProcessor.ReadMethodBody(body).Any(instruction =>
                    instruction.Value is MethodBase called
                    && called.Module == callee.Module
                    && called.MetadataToken == callee.MetadataToken);
            }
        }
    }
}
