using Verse;

namespace Mastercrafted_Arms
{
    public static class Utils
    {
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
    }
}
