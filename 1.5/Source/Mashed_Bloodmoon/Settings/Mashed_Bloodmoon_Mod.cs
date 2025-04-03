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
    }
}
