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
        public static IntRange HuntsmanMoon_HoursBetweenAttacks => _instance.Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks;
        public static float HuntsmanMoon_RaidPointsMultiplier => _instance.Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier;
        public static float HuntsmanMoon_AmbushPointsMultiplier => _instance.Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier;
        public static float HuntsmanMoon_PackPointsMultiplier => _instance.Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier;
        public static bool HuntsmanMoon_UncapRaidPoints => _instance.Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints;

        // Lycanthropy
        public static bool Lycanthropy_EnableOptionsGizmo => _instance.Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo;
        public static bool EnableConsumeHeartOrder => _instance.Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder;
        public static bool Lycanthropy_EnableConsumeHeartMessage => _instance.Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage;
        public static bool Lycanthropy_PrisonersTransformOnDamage => _instance.Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage;
        public static bool Lycanthropy_SlavesTransformOnDamage => _instance.Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage;
        public static bool Lycanthropy_PrisonersHideGizmo => _instance.Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo;
        public static bool Lycanthropy_SlavesHideGizmo => _instance.Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo;
        public static int Lycanthropy_ConsumedHearMultiplier => _instance.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier;
        public static BeastFormScalingMode Lycanthropy_TextureScalingMode => _instance.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode;


        /* ==========[VARIABLES]========== */

        // Huntsman's Moon
        public bool Mashed_Bloodmoon_HuntsmanMoon_EnableCondition = Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def;
        public IntRange Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks = Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks_def;
        public float Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier_def;
        public float Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier_def;
        public float Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier_def;
        public bool Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints = Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints_def;

        // Lycanthropy
        public bool Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo = Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def;
        public bool Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder = Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder_def;
        public bool Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage = Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage_def;
        public bool Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage = Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage_def;
        public bool Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage = Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage_def;
        public bool Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo = Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo_def;
        public bool Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo = Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo_def;
        public int Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier = Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier_def;
        public BeastFormScalingMode Mashed_Bloodmoon_Lycanthropy_TextureScalingMode = Mashed_Bloodmoon_Lycanthropy_TextureScalingMode_def;

        /* ==========[DEFAULTS]========== */

        // Huntsman's Moon
        private static readonly bool Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def = true;
        private static readonly IntRange Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks_def = new IntRange(4, 7);
        private static readonly float Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier_def = 1f;
        private static readonly float Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier_def = 1f;
        private static readonly float Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier_def = 1f;
        private static readonly bool Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints_def = false;

        // Lycanthropy
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo_def = true;
        private static readonly bool Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo_def = true;
        private static readonly int Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier_def = 1;
        private static readonly BeastFormScalingMode Mashed_Bloodmoon_Lycanthropy_TextureScalingMode_def = BeastFormScalingMode.DrawSize;

        /* ==========[SAVING]========== */

        public override void ExposeData()
        {
            /* Huntsman's Moon */
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_EnableCondition, "Mashed_Bloodmoon_HuntsmanMoon_EnableCondition", Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks, "Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks", Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier, "Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier", Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier, "Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier", Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier, "Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier", Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints, "Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints", Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints_def);

            /* Lycanthropy */
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo, "Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo", Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder, "Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder", Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage, "Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage", Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage, "Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage", Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage, "Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage", Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo, "Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo", Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo, "Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo", Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier, "Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier", Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier_def);
            Scribe_Values.Look(ref Mashed_Bloodmoon_Lycanthropy_TextureScalingMode, "Mashed_Bloodmoon_Lycanthropy_TextureScalingMode", Mashed_Bloodmoon_Lycanthropy_TextureScalingMode_def);

            base.ExposeData();
        }

        /* ==========[RESETTING]========== */

        public static void ResetSettings()
        {
            // Huntsman's Moon
            _instance.Mashed_Bloodmoon_HuntsmanMoon_EnableCondition = Mashed_Bloodmoon_HuntsmanMoon_EnableCondition_def;
            _instance.Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks = Mashed_Bloodmoon_HuntsmanMoon_HoursBetweenAttacks_def;
            _instance.Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_RaidPointsMultiplier_def;
            _instance.Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_AmbushPointsMultiplier_def;
            _instance.Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier = Mashed_Bloodmoon_HuntsmanMoon_PackPointsMultiplier_def;
            _instance.Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints = Mashed_Bloodmoon_HuntsmanMoon_UncapRaidPoints_def;

            // Lycanthropy
            _instance.Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo = Mashed_Bloodmoon_Lycanthropy_EnableOptionsGizmo_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder = Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartOrder_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage = Mashed_Bloodmoon_Lycanthropy_EnableConsumeHeartMessage_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage = Mashed_Bloodmoon_Lycanthropy_PrisonersTransformOnDamage_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage = Mashed_Bloodmoon_Lycanthropy_SlavesTransformOnDamage_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo = Mashed_Bloodmoon_Lycanthropy_PrisonersHideGizmo_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo = Mashed_Bloodmoon_Lycanthropy_SlavesHideGizmo_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier = Mashed_Bloodmoon_Lycanthropy_ConsumedHearMultiplier_def;
            _instance.Mashed_Bloodmoon_Lycanthropy_TextureScalingMode = Mashed_Bloodmoon_Lycanthropy_TextureScalingMode_def;
        }
    }
}
