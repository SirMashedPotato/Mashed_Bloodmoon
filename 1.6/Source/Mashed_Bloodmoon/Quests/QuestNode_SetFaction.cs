using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Mashed_Bloodmoon
{
    public class QuestNode_SetFaction : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> storeFactionAs;
        public FactionDef factionDef;

        protected override void RunInt()
        {
            if (!TrySetVars(QuestGen.slate))
            {
                Log.Error("Could not resolve site parts.");
            }
        }

        protected override bool TestRunInt(Slate slate)
        {
            return TrySetVars(slate);
        }

        private bool TrySetVars(Slate slate)
        {
            Faction faction = FactionUtility.DefaultFactionFrom(factionDef);
            if (faction == null)
            {
                return false;
            }

            slate.Set(storeFactionAs.GetValue(slate), faction);
            return true;
        }
    }
}
