using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

#nullable disable
namespace Roasio.AnimaSowing
{
    public class RitualPosition_AnimusStoneRitualSpot : RitualPosition
    {
        public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
        {
            IntVec3 cell;
            if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, CellRect.CenteredOn(spot, 0), 2f, 3f, p.Map, out cell))
                return new PawnStagePosition(cell, (Thing)null, Rot4.FromAngleFlat((spot - cell).AngleFlat), this.highlight);
            Thing thing = ritual.selectedTarget.Thing;
            CompAnimaSowable comp = thing != null ? thing.TryGetComp<CompAnimaSowable>() : (CompAnimaSowable)null;
            LocalTargetInfo spot1;
            if (comp == null || !comp.TryFindLinkSpot(p, out spot1))
                return new PawnStagePosition(IntVec3.Invalid, (Thing)null, Rot4.Invalid, this.highlight);
            Rot4 orientation = Rot4.FromAngleFlat((spot - spot1.Cell).AngleFlat);
            return new PawnStagePosition(spot1.Cell, (Thing)null, orientation, this.highlight);
        }
    }
}
