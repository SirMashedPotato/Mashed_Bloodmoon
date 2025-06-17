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

        /* ==========[GETTERS]========== */

        // Huntsman's Moon
        public static bool HuntsmanMoon_EnableCondition => _instance.Mashed_Bloodmoon_HuntsmanMoon_EnableCondition;

        // Lycanthropy
        public static bool Lycanthropy_EnableOptionsGizmo => _instance.Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo;

        /* ==========[VARIABLES]========== */

        // Huntsman's Moon
        public bool Mashed_Bloodmoon_HuntsmanMoon_EnableCondition = Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def;

        // Lycanthropy
        public bool Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo = Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def;

        /* ==========[DEFAULTS]========== */

        // Huntsman's Moon
        private static readonly bool Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def = true;

        // Lycanthropy
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def = true;

        /* ==========[SAVING]========== */

        public override void ExposeData()
        {
            /* Huntsman's Moon */
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_EnableCondition, "Mashed_Bloodmoon_HuntsmanMoon_EnableCondition", Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def);

            /* Lycanthropy */
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo, "Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo", Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def);

            base.ExposeData();
        }

        /* ==========[RESETTING]========== */
    }
}
