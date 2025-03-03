using RimWorld;

namespace Mashed_Bloodmoon
{
    public class StorytellerCompProperties_CategoryMTBCustom : StorytellerCompProperties
    {
        public StorytellerCompProperties_CategoryMTBCustom()
        {
            compClass = typeof(StorytellerComp_CategoryMTBCustom);
        }

        public float mtbDays = -1f;
        public IncidentCategoryDef category;
    }
}
