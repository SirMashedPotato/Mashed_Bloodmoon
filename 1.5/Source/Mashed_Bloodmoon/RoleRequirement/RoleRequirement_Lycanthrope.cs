using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RoleRequirement_Lycanthrope : RoleRequirement
    {
        public override string GetLabel(Precept_Role role)
        {
            return "Mashed_Bloodmoon_MustBeLycanthrope".Translate();
        }

        public override bool Met(Pawn p, Precept_Role role)
        {
            return LycanthropeUtility.PawnIsLycanthrope(p);
        }
    }
}
