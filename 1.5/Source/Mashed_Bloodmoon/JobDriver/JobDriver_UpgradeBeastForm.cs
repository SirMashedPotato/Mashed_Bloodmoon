using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Mashed_Bloodmoon
{
    public class JobDriver_UpgradeBeastForm : JobDriver_WolfsbloodAltar
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_General.Do(delegate
            {
                Page_UpgradeBeastForm page = new Page_UpgradeBeastForm(LycanthropeUtility.GetCompLycanthrope(pawn));
                Find.WindowStack.Add(page);
            });
        }
    }
}
