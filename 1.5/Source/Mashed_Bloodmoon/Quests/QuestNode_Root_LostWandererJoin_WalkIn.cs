using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Mashed_Bloodmoon
{
    public class QuestNode_Root_LostWandererJoin_WalkIn : QuestNode_Root_WandererJoin_WalkIn
    {
        private string signalAccept;
        private string signalReject;
        private const int TimeoutTicks = 60000;

        public override Pawn GeneratePawn()
        {
            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind: PawnKindDefOf.Villager,context: PawnGenerationContext.NonPlayer, 
                mustBeCapableOfViolence: true, 
                dontGiveWeapon: true,
                allowPregnant: false,
                allowFood: false));

            if (Rand.Chance(0.75f))
            {
                pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant).Severity = 0.1f;
            }

            if (!pawn.IsWorldPawn())
            {
                Find.WorldPawns.PassToWorld(pawn);
            }
            return pawn;
        }

        /// <summary>
        /// Copy pasted and not even edited :/
        /// </summary>
        protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
        {
            signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            quest.Signal(signalAccept, delegate
            {
                quest.SetFaction(Gen.YieldSingle(pawn), Faction.OfPlayer);
                quest.PawnsArrive(Gen.YieldSingle(pawn), null, map.Parent);
                QuestGen_End.End(quest, QuestEndOutcome.Success);
            });
            quest.Signal(signalReject, delegate
            {
                quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining);
                QuestGen_End.End(quest, QuestEndOutcome.Fail);
            });
        }

        /// <summary>
        /// Copy pasted with slight edits :/
        /// </summary>
        public override void SendLetter(Quest quest, Pawn pawn)
        {
            TaggedString title = "Mashed_Bloodmoon_LostWanderer_Label".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            TaggedString letterText = "Mashed_Bloodmoon_LostWanderer_Desc".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref letterText);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref letterText, ref title, pawn);

            ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(title, letterText, LetterDefOf.AcceptJoiner);
            choiceLetter_AcceptJoiner.signalAccept = signalAccept;
            choiceLetter_AcceptJoiner.signalReject = signalReject;
            choiceLetter_AcceptJoiner.quest = quest;
            choiceLetter_AcceptJoiner.StartTimeout(TimeoutTicks);
            Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner);
        }
    }
}
