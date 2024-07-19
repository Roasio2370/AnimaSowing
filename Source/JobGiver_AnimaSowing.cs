using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;


#nullable disable
namespace Roasio.AnimaSowing
{
    public class JobGiver_AnimaSowing : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            PawnDuty duty = pawn.mindState.duty;
            if (duty == null)
                return (Job)null;
            if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly))
                return (Job)null;
            Thing thing = duty.focusSecond.Thing;
            CompAnimaSowable comp = thing != null ? thing.TryGetComp<CompAnimaSowable>() : (CompAnimaSowable)null;
            return comp == null || !comp.Report(pawn).Accepted ? (Job)null : JobMaker.MakeJob(JobDefOf.SowAnimaTree, duty.focusSecond, duty.focus);
        }
    }
}
