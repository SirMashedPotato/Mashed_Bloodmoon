using Verse;

namespace Mashed_Bloodmoon
{
    public class Mashed_Bloodmoon_ModSettings : ModSettings
    {
        public static Mashed_Bloodmoon_ModSettings Instance => _instance;

        private static Mashed_Bloodmoon_ModSettings _instance;

        public Mashed_Bloodmoon_ModSettings()
        {
            _instance = this;
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }
    }
}
