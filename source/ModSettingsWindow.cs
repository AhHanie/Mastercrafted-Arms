using RimWorld;
using UnityEngine;
using Verse;

namespace Mastercrafted_Arms
{
    public static class ModSettingsWindow
    {
        public static void Draw(Rect parent)
        {
            Rect innerRect = new Rect(parent);
            innerRect.x += 5f;
            innerRect.width -= 35f;
            innerRect.y += 10f;
            innerRect.height -= 20f;

            Listing_Standard list = new Listing_Standard(GameFont.Small);
            Widgets.DrawMenuSection(parent);
            list.Begin(innerRect);
            list.Gap(6f);

            float labelWidth = innerRect.width * 0.55f;

            Rect craftRow = list.GetRect(22f);
            Rect craftLabelRect = new Rect(craftRow.x, craftRow.y, labelWidth, craftRow.height);
            Rect craftSliderRect = new Rect(craftLabelRect.xMax + 10f, craftRow.y, craftRow.width - labelWidth - 10f, craftRow.height);
            Widgets.Label(craftLabelRect, "MastercraftedArms.Settings.CraftChance.Label".Translate());
            ModSettings.craftSuccessChance = Widgets.HorizontalSlider(
                craftSliderRect,
                ModSettings.craftSuccessChance,
                0f,
                1f,
                false,
                ModSettings.craftSuccessChance.ToStringPercent(),
                null,
                null,
                0.01f
            );

            list.Gap(6f);
            list.Label("MastercraftedArms.Settings.LevelRequirement.Label".Translate());

            list.Gap(10f);
            Rect minQualityRow = list.GetRect(22f);
            Rect minQualityLabelRect = new Rect(minQualityRow.x, minQualityRow.y, labelWidth, minQualityRow.height);
            Rect minQualitySliderRect = new Rect(minQualityLabelRect.xMax + 10f, minQualityRow.y, minQualityRow.width - labelWidth - 10f, minQualityRow.height);
            Widgets.Label(minQualityLabelRect, "MastercraftedArms.Settings.MinimumQuality.Label".Translate());
            ModSettings.minimumUniqueQuality = (QualityCategory)Mathf.RoundToInt(
                Widgets.HorizontalSlider(
                    minQualitySliderRect,
                    (float)ModSettings.minimumUniqueQuality,
                    0f,
                    6f,
                    false,
                    ModSettings.minimumUniqueQuality.GetLabel().CapitalizeFirst(),
                    null,
                    null,
                    1f
                )
            );

            list.Gap(6f);
            list.CheckboxLabeled(
                "MastercraftedArms.Settings.ShowWeaponCustomizationMenu.Label".Translate(),
                ref ModSettings.showCustomizationMenu,
                "MastercraftedArms.Settings.ShowWeaponCustomizationMenu.Tooltip".Translate()
            );

            list.Gap(6f);
            list.Label("MastercraftedArms.Settings.CustomizationMenuNote.Label".Translate());

            list.Gap(10f);
            Rect maxGeneratedRow = list.GetRect(22f);
            Rect maxGeneratedLabelRect = new Rect(maxGeneratedRow.x, maxGeneratedRow.y, labelWidth, maxGeneratedRow.height);
            Rect maxGeneratedSliderRect = new Rect(maxGeneratedLabelRect.xMax + 10f, maxGeneratedRow.y, maxGeneratedRow.width - labelWidth - 10f, maxGeneratedRow.height);
            if (Mouse.IsOver(maxGeneratedLabelRect))
            {
                Widgets.DrawHighlight(maxGeneratedLabelRect);
            }

            Widgets.Label(maxGeneratedLabelRect, "MastercraftedArms.Settings.MaxGeneratedTraits.Label".Translate());
            TooltipHandler.TipRegion(maxGeneratedLabelRect, "MastercraftedArms.Settings.MaxGeneratedTraits.Tooltip".Translate());
            ModSettings.maxGeneratedTraits = Mathf.RoundToInt(
                Widgets.HorizontalSlider(maxGeneratedSliderRect, ModSettings.maxGeneratedTraits, 3f, 20f, false, ModSettings.maxGeneratedTraits.ToString(), null, null, 1f)
            );

            list.Gap(24f);
            list.Label("MastercraftedArms.Settings.MaxTraitSelections.Header".Translate());

            Rect maxLowRow = list.GetRect(22f);
            Rect maxLowLabelRect = new Rect(maxLowRow.x, maxLowRow.y, labelWidth, maxLowRow.height);
            Rect maxLowSliderRect = new Rect(maxLowLabelRect.xMax + 10f, maxLowRow.y, maxLowRow.width - labelWidth - 10f, maxLowRow.height);
            Widgets.Label(maxLowLabelRect, "MastercraftedArms.Settings.MaxTraitSelections.Low.Label".Translate());
            ModSettings.maxSelectionsLow = Mathf.RoundToInt(
                Widgets.HorizontalSlider(maxLowSliderRect, ModSettings.maxSelectionsLow, 1f, 5f, false, ModSettings.maxSelectionsLow.ToString(), null, null, 1f)
            );

            list.Gap(6f);
            Rect maxMidRow = list.GetRect(22f);
            Rect maxMidLabelRect = new Rect(maxMidRow.x, maxMidRow.y, labelWidth, maxMidRow.height);
            Rect maxMidSliderRect = new Rect(maxMidLabelRect.xMax + 10f, maxMidRow.y, maxMidRow.width - labelWidth - 10f, maxMidRow.height);
            Widgets.Label(maxMidLabelRect, "MastercraftedArms.Settings.MaxTraitSelections.Mid.Label".Translate());
            ModSettings.maxSelectionsMid = Mathf.RoundToInt(
                Widgets.HorizontalSlider(maxMidSliderRect, ModSettings.maxSelectionsMid, 1f, 7f, false, ModSettings.maxSelectionsMid.ToString(), null, null, 1f)
            );

            list.Gap(6f);
            Rect maxHighRow = list.GetRect(22f);
            Rect maxHighLabelRect = new Rect(maxHighRow.x, maxHighRow.y, labelWidth, maxHighRow.height);
            Rect maxHighSliderRect = new Rect(maxHighLabelRect.xMax + 10f, maxHighRow.y, maxHighRow.width - labelWidth - 10f, maxHighRow.height);
            Widgets.Label(maxHighLabelRect, "MastercraftedArms.Settings.MaxTraitSelections.High.Label".Translate());
            ModSettings.maxSelectionsHigh = Mathf.RoundToInt(
                Widgets.HorizontalSlider(maxHighSliderRect, ModSettings.maxSelectionsHigh, 1f, 9f, false, ModSettings.maxSelectionsHigh.ToString(), null, null, 1f)
            );

            list.End();
        }
    }
}
