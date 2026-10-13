using RimWorld;
using Verse;

namespace Mastercrafted_Arms
{
    public class ModSettings : Verse.ModSettings
    {
        public static float craftSuccessChance = 1f;
        public static QualityCategory minimumUniqueQuality = QualityCategory.Awful;
        public static bool showCustomizationMenu = false;
        public static int maxGeneratedTraits = 6;
        public static int maxSelectionsLow = 1;
        public static int maxSelectionsMid = 2;
        public static int maxSelectionsHigh = 3;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref craftSuccessChance, "craftSuccessChance", 1f);
            Scribe_Values.Look(ref minimumUniqueQuality, "minimumUniqueQuality", QualityCategory.Awful);
            Scribe_Values.Look(ref showCustomizationMenu, "showCustomizationMenu", false);
            Scribe_Values.Look(ref maxGeneratedTraits, "maxGeneratedTraits", 6);
            Scribe_Values.Look(ref maxSelectionsLow, "maxSelectionsLow", 1);
            Scribe_Values.Look(ref maxSelectionsMid, "maxSelectionsMid", 2);
            Scribe_Values.Look(ref maxSelectionsHigh, "maxSelectionsHigh", 3);
        }
    }
}
