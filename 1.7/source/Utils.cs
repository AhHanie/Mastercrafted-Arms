using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Mastercrafted_Arms
{
    public static class Utils
    {
        private const int RequiredCraftingLevel = 20;

        // Weapons converted to their unique variant during recipe production. Weak, so a conversion that is
        // never consumed (a mod calling GenRecipe.MakeRecipeProducts without finishing a bill) cannot leak.
        private static readonly ConditionalWeakTable<Thing, object> convertedWeapons = new ConditionalWeakTable<Thing, object>();
        private static readonly object convertedMarker = new object();

        public static ThingDef GetUniqueVariant(ThingDef weaponDef)
        {
            if (weaponDef != null && weaponDef.IsWeapon)
            {
                DefModExtension_UniqueVariant extension = weaponDef.GetModExtension<DefModExtension_UniqueVariant>();
                if (extension?.uniqueWeapon != null)
                {
                    return extension.uniqueWeapon;
                }
            }

            return null;
        }

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

        public static void MarkConvertedWeapon(Thing weapon)
        {
            convertedWeapons.AddOrUpdate(weapon, convertedMarker);
        }

        public static bool TryConsumeConvertedWeapon(Thing weapon)
        {
            return convertedWeapons.Remove(weapon);
        }
    }
}
