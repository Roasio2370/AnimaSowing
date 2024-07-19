using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Roasio.AnimaSowing
{
    public class CompAnimaSowable : ThingComp
    {
        public CompProperties_AnimaSowable Props => (CompProperties_AnimaSowable) this.props;

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
        {
            if (!pawn.Dead && !pawn.Drafted)
            {
                string label = (string)"BeginAnimaTreeSowingRitualFloatMenu".Translate();
                AcceptanceReport acceptanceReport = this.Report(pawn);
                if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
                    label = label + ": " + acceptanceReport.Reason;
                yield return new FloatMenuOption(label, (Action)(() =>
                {
                    Precept_Ritual preceptRitual = (Precept_Ritual)null;
                    for (int index = 0; index < pawn.Ideo.PreceptsListForReading.Count; ++index)
                    {
                        if (pawn.Ideo.PreceptsListForReading[index].def == Props.preceptDef)
                        {
                            preceptRitual = (Precept_Ritual)pawn.Ideo.PreceptsListForReading[index];
                            break;
                        }
                    }
                    if (preceptRitual == null)
                        return;
                    Find.WindowStack.Add(preceptRitual.GetRitualBeginWindow((TargetInfo)(Thing)this.parent, selectedPawn: pawn));
                }))
                {
                    Disabled = !acceptanceReport.Accepted
                };
            }
        }

        public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
        {
            spot = MeditationUtility.FindMeditationSpot(pawn).spot;
            if (this.CanUseSpot(pawn, spot))
                return true;
            int num1 = GenRadial.NumCellsInRadius(2.9f);
            int num2 = GenRadial.NumCellsInRadius(3.9f);
            for (int index = num1; index < num2; ++index)
            {
                IntVec3 spot1 = this.parent.Position + GenRadial.RadialPattern[index];
                if (this.CanUseSpot(pawn, (LocalTargetInfo)spot1))
                {
                    spot = (LocalTargetInfo)spot1;
                    return true;
                }
            }
            spot = (LocalTargetInfo)IntVec3.Zero;
            return false;
        }

        private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
        {
            IntVec3 cell = spot.Cell;
            return (double)cell.DistanceTo(this.parent.Position) <= 3.9000000953674316 && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly);
        }

        public AcceptanceReport Report(Pawn pawn, LocalTargetInfo? knownSpot = null, bool checkSpot = true)
        {
            if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
                return (AcceptanceReport)false;
            if (!CheckFertility())
                return new AcceptanceReport((string)"BeginAnimaSowingRitualNeedFertility".Translate((NamedArgument)ThingDefOf.Plant_TreeAnima.fertility));
            if (!this.Props.requiredFocusDef.CanPawnUse(pawn))
                return new AcceptanceReport((string)"BeginLinkingRitualNeedFocus".Translate((NamedArgument)this.Props.requiredFocusDef.label));
            if (pawn.GetPsylinkLevel() < Props.neededPsyLevel)
                return new AcceptanceReport((string)"BeginLinkingRitualRequiresPsylinkLevel".Translate(Props.neededPsyLevel));
            if (!pawn.Map.reservationManager.CanReserve(pawn, (LocalTargetInfo)(Thing)this.parent))
            {
                Pawn pawn1 = pawn.Map.reservationManager.FirstRespectedReserver((LocalTargetInfo)(Thing)this.parent, pawn);
                return new AcceptanceReport((string)(pawn1 == null ? "Reserved".Translate() : "ReservedBy".Translate((NamedArgument)pawn.LabelShort, (NamedArgument)(Thing)pawn1)));
            }
            if (checkSpot)
            {
                if (knownSpot.HasValue)
                {
                    if (!this.CanUseSpot(pawn, knownSpot.Value))
                        return new AcceptanceReport((string)"BeginLinkingRitualNeedLinkSpot".Translate());
                }
                else if (!this.TryFindLinkSpot(pawn, out LocalTargetInfo _spot))
                    return new AcceptanceReport((string)"BeginLinkingRitualNeedLinkSpot".Translate());
            }
            return AcceptanceReport.WasAccepted;
        }

        public void FinishSowingRitual()
        {
            Verse.GenSpawn.Spawn(ThingDefOf.Plant_TreeAnima, parent.Position, parent.Map,WipeMode.Vanish);
            parent.DeSpawn(DestroyMode.Vanish);
        }

        public bool CheckFertility()
        {
            if (parent.Map == null)
                return false;
            else if (parent.Position.GetFertility(parent.Map) >= ThingDefOf.Plant_TreeAnima.plant.fertilityMin) return true;
            return false;
        }
    }
}
