using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_UseEffectTotem : CompProperties
    {
        public CompProperties_UseEffectTotem()
        {
            compClass = typeof(CompUseEffect_Totem);
        }

        public TotemTypeDef totemTypeDef;
        public int usedCount = 1;
    }
}
