using RimWorld;
using Roasio.AnimaSowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

#nullable disable
namespace Roasio.AnimaSowing
{
    public class RitualObligationTargetWorker_AnimaSowing : RitualObligationTargetWorker_AnimaTree
    {
        public RitualObligationTargetWorker_AnimaSowing()
        {

        }

        public RitualObligationTargetWorker_AnimaSowing(RitualObligationTargetFilterDef def) : base(def)
        {

        }

        protected override RitualTargetUseReport CanUseTargetInternal(
      TargetInfo target,
      RitualObligation obligation)
        {
            CompAnimaSowable comp = target.Thing.TryGetComp<CompAnimaSowable>();
            if (comp == null)
                return (RitualTargetUseReport)false;
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            foreach (Pawn pawn in target.Map.mapPawns.FreeColonistsSpawned)
            {
                if (comp.Props.requiredFocusDef.CanPawnUse(pawn))
                    flag1 = true;
                if (pawn.GetPsylinkLevel()>=comp.Props.neededPsyLevel)
                    flag2 = true;
                if (target.Thing.Position.GetFertility(target.Thing.Map) > ThingDefOf.Plant_TreeAnima.plant.fertilityMin)
                    flag3 = true;
            }
            if (!flag1)
                return (RitualTargetUseReport)"RitualTargetNoPawnsWithNeededFocus".Translate((NamedArgument)comp.Props.requiredFocusDef);
            if (!flag2)
                return (RitualTargetUseReport)"RitualTargetNoPawnsWithNeededPsyLevel".Translate((NamedArgument)comp.Props.neededPsyLevel);
            if (!flag3)
                return (RitualTargetUseReport)"RitualTargetAnimaSowingNeedFertility".Translate((NamedArgument)(ThingDefOf.Plant_TreeAnima.plant.fertilityMin*100));
            return (RitualTargetUseReport)true;
        }

        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            yield return (string)"RitualTargetAnimaSowingRitualInfo".Translate();
        }
    }
}
