using System;
using System.Collections.Generic;
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

        private enum SettingsTab : byte
        {
            HuntsmansMoon,
            Lycanthropy
        }

        private void ReadySettingsTabs()
        {
            tabs.Add(new TabRecord("Mashed_Bloodmoon_HuntsmanMoon_Tab".Translate(), delegate
            {
                curTab = SettingsTab.HuntsmansMoon;
            }, () => curTab == SettingsTab.HuntsmansMoon));

            tabs.Add(new TabRecord("Mashed_Bloodmoon_Lycanthropy_Tab".Translate(), delegate
            {
                curTab = SettingsTab.Lycanthropy;
            }, () => curTab == SettingsTab.Lycanthropy));
        }

        private readonly List<TabRecord> tabs = new List<TabRecord>();
        private static SettingsTab curTab = SettingsTab.HuntsmansMoon;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (tabs.NullOrEmpty())
            {
                ReadySettingsTabs();
            }

            Rect mainRect = inRect;
            mainRect.yMin += 45f;
            Widgets.DrawMenuSection(mainRect);
            TabDrawer.DrawTabs(mainRect, tabs);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(mainRect.ContractedBy(GenUI.Gap));

            switch (curTab)
            {
                case SettingsTab.HuntsmansMoon:
                    DoSettingsTabContents_HuntsmansMoon(listing_Standard);
                    break;

                case SettingsTab.Lycanthropy:
                    DoSettingsTabContents_Lycanthropy(listing_Standard);
                    break;
            }

            listing_Standard.End();

            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width - Window.CloseButSize.x, inRect.y + inRect.height + 2f, Window.CloseButSize.x, Window.CloseButSize.y), "ResetAll".Translate()))
            {
                Mashed_Bloodmoon_ModSettings.ResetSettings();
            }

            base.DoSettingsWindowContents(inRect);
        }
    
        public void DoSettingsTabContents_HuntsmansMoon(Listing_Standard listing_Standard)
        {
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

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints".Translate(), ref settings.Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints);
            listing_Standard.Gap();

            listing_Standard.Gap();
            listing_Standard.GapLine();
        }

        public void DoSettingsTabContents_Lycanthropy(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier".Translate(settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier));
            settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier = (int)listing_Standard.Slider(settings.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier, 1, 10);

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo".Translate(), ref settings.Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo, "Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_desc".Translate());
            listing_Standard.Gap();

            listing_Standard.Label("Mashed_Bloodmoon_Lycanthropy_TextureScalingMode".Translate());

            if (listing_Standard.RadioButton("Mashed_Bloodmoon_Lycanthropy_ScalingMode_BodySize".Translate(), 
                settings.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode == BeastFormScalingMode.BodySize, tooltip: "Mashed_Bloodmoon_Lycanthropy_ScalingMode_BodySizeDesc".Translate()))
            {
                settings.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode = BeastFormScalingMode.BodySize;
            }

            if (listing_Standard.RadioButton("Mashed_Bloodmoon_Lycanthropy_ScalingMode_DrawSize".Translate(),
                settings.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode == BeastFormScalingMode.DrawSize, tooltip: "Mashed_Bloodmoon_Lycanthropy_ScalingMode_DrawSizeDesc".Translate()))
            {
                settings.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode = BeastFormScalingMode.DrawSize;
            }
            listing_Standard.Gap();

            listing_Standard.Gap();
            listing_Standard.GapLine();
            listing_Standard.Gap(24);

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

            listing_Standard.Gap();
            listing_Standard.GapLine();
        }
    }
}
