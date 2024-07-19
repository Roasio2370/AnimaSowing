using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

#nullable disable
namespace Roasio.AnimaSowing
{
    public class RitualOutcomeEffectWorker_AnimaTreeSowing: RitualOutcomeEffectWorker_FromQuality
    {
        RitualOutcomeComp_ParticipantCount participantCount;
        public RitualOutcomeEffectWorker_AnimaTreeSowing() { }
        public RitualOutcomeEffectWorker_AnimaTreeSowing(RitualOutcomeEffectDef def) : base(def) 
        {
            foreach (RitualOutcomeComp comp in def.comps)
            {
                if(comp.GetType()==typeof(RitualOutcomeComp_ParticipantCount))
                    participantCount = (RitualOutcomeComp_ParticipantCount)comp;
            }
        }

        public override bool SupportsAttachableOutcomeEffect => false;

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            float quality = this.GetQuality(jobRitual, progress);
            Pawn pawn = jobRitual.PawnWithRole("organizer");
            Thing thing = jobRitual.selectedTarget.Thing;
            CompAnimaSowable comp = thing != null ? thing.TryGetComp<CompAnimaSowable>() : (CompAnimaSowable)null;
            if (comp == null) Log.Message("null component");
            comp?.FinishSowingRitual();
            string str = (string)"LetterTextSowingRitualCompleted".Translate(pawn.Named("PAWN"), jobRitual.selectedTarget.Thing.Named("LINKABLE"));
            string text = str + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            Find.LetterStack.ReceiveLetter("LetterLabelAnimaTreeSowingRitualCompleted".Translate(), (TaggedString)text, LetterDefOf.RitualOutcomePositive, new LookTargets(new TargetInfo[2]
            {
        (TargetInfo) thing,
        (TargetInfo) jobRitual.selectedTarget.Thing
            }));
        }
    }
}
