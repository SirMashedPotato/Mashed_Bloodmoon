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
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);

            listing_Standard.CheckboxLabeled("Mashed_Bloodmoon_HuntsmanMoon_EnableCondition".Translate(), ref settings.Mashed_Bloodmoon_HuntsmanMoon_EnableCondition);

            listing_Standard.End();

            base.DoSettingsWindowContents(inRect);
        }
    }
}
