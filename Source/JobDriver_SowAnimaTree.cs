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
    public class JobDriver_SowAnimaTree : JobDriver
    {
        public const int LinkTimeTicks = 15000;
        public const int EffectsTickInterval = 720;
        protected const TargetIndex AnimaSowableInd = TargetIndex.A;
        protected const TargetIndex LinkSpotInd = TargetIndex.B;

        private Thing AnimaSowableThing => this.TargetA.Thing;

        private CompAnimaSowable AnimaSowable => this.AnimaSowableThing.TryGetComp<CompAnimaSowable>();

        private LocalTargetInfo LinkSpot => this.job.targetB;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve((LocalTargetInfo)this.AnimaSowableThing, this.job, errorOnFailed: errorOnFailed) && this.pawn.Reserve(this.LinkSpot, this.job, errorOnFailed: errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            JobDriver_SowAnimaTree driverLinkPsylinkable = this;
            if (ModLister.CheckRoyalty("Psylinkable"))
            {
                // ISSUE: reference to a compiler-generated method
                //driverLinkPsylinkable.AddFailCondition(new Func<bool>(driverLinkPsylinkable.MakeNewToils()));
                yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
                Toil toil = Toils_General.Wait(15000);
                // ISSUE: reference to a compiler-generated method
                //toil.tickAction = new Action(driverLinkPsylinkable.MakeNewToils<toil>());
                toil.handlingFacing = false;
                toil.socialMode = RandomSocialMode.Off;
                yield return toil;
            }
        }
    }
}
