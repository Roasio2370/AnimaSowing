using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Roasio.AnimaSowing
{
    public class CompProperties_AnimaSowable: CompProperties
    {
        public int neededPsyLevel;
        public PreceptDef preceptDef;
        public MeditationFocusDef requiredFocusDef;

        public CompProperties_AnimaSowable() => this.compClass = typeof(CompAnimaSowable);
    }
}
