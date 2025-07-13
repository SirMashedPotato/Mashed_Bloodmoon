using System;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Mashed_Bloodmoon_Mod : Mod
    {
        readonly Mashed_Bloodmoon_ModSettings settings;

        public Mashed_Bloodmoon_Mod(ModContentPack contentPack) : base(contentPack)
        {
            settings = GetSettings<Mashed_Bloodmoon_ModSettings>();
            Log.Message("[Mashed's Bloodmoon] version " + Content.ModMetaData.ModVersion);
        }

        public override string SettingsCategory() => "Mashed_Bloodmoon_ModName".Translate();


        public override void DoSettingsWindowContents(Rect inRect)
        {
            Widgets.DrawMenuSection(inRect);
            Rect mainRect = inRect.ContractedBy(15f);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(mainRect);

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_HuntsmanMoon_EnableCondition".Translate(), ref settings.Mashed_Bloodmoon_HuntsmanMoon_EnableCondition);
            listing_Standard.Gap();

            listing_Standard.Label("Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks".Translate(settings.Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks.min, settings.Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks.max));
            listing_Standard.IntRange(ref settings.Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks, 1, 24);

            listing_Standard.Gap();

            listing_Standard.Label("Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier".Translate(settings.Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier * 100));
            settings.Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier = (float)Math.Round(listing_Standard.Slider(settings.Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier, 0.1f, 5f) * 10) / 10;

            listing_Standard.Label("Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier".Translate(settings.Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier * 100));
            settings.Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier = (float)Math.Round(listing_Standard.Slider(settings.Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier, 0.1f, 5f) * 10) / 10;

            listing_Standard.Label("Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier".Translate(settings.Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier * 100));
            settings.Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier = (float)Math.Round(listing_Standard.Slider(settings.Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier, 0.1f, 5f) * 10) / 10;

            listing_Standard.Gap();
            listing_Standard.GapLine();
            listing_Standard.Gap(24);

            listing_Standard.Label("Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier".Translate(settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier));
            settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier = (int)listing_Standard.Slider(settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier, 1, 10);

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo, "Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_desc".Translate());
            listing_Standard.Gap();

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo);
            listing_Standard.Gap();

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage);
            listing_Standard.Gap();

            if (ModsConfig.IdeologyActive)
            {
                listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo);
                listing_Standard.Gap();

                listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage);
                listing_Standard.Gap();
            }

            listing_Standard.End();

            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width - Window.CloseButSize.x, inRect.y + inRect.height + 2f, Window.CloseButSize.x, Window.CloseButSize.y), "ResetAll".Translate()))
            {
                Mashed_Bloodmoon_ModSettings.ResetSettings();
            }

            base.DoSettingsWindowContents(inRect);
        }
    }
}
