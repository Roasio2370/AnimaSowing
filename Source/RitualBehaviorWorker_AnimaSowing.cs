using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

#nullable disable
namespace Roasio.AnimaSowing
{
    public class RitualBehaviorWorker_AnimaSowing : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_AnimaSowing()
        {

        }

        public RitualBehaviorWorker_AnimaSowing(RitualBehaviorDef def) : base(def) { }

        public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
        {
            TaggedString explanation = "AnimaSowingExplanationBase".Translate();
            return explanation;
        }

        public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
        {
            int count = assignments.SpectatorsForReading.Count;
            return Mathf.RoundToInt((float)ritual.behavior.def.durationTicks.max / RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)(count + 1))).ToStringTicksToPeriod(false);
        }
    }
}
