using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Roasio.AnimaSowing
{
    public class RitualRoleAnimaFarmer : RitualRole
    {
        public override bool AppliesToPawn(
      Pawn p,
      out string reason,
      TargetInfo selectedTarget,
      LordJob_Ritual ritual = null,
      RitualRoleAssignments assignments = null,
      Precept_Ritual precept = null,
      bool skipReason = false)
        {
            CompProperties_AnimaSowable props = selectedTarget.Thing.def.GetCompProperties<CompProperties_AnimaSowable>();

            if (!this.AppliesIfChild(p, out reason, skipReason))
                return false;
            if (!p.Faction.IsPlayerSafe())
            {
                if (!skipReason)
                    reason = (string)"MessageRitualRoleMustBeColonist".Translate((NamedArgument)this.Label);
                return false;
            }
            if (ritual != null)
            {
                if (p == ritual.Organizer)
                    return true;
            }
            else if (assignments != null && assignments.Required(p))
                return true;

            if (!props.requiredFocusDef.CanPawnUse(p))
            {
                if (!skipReason)
                    reason = (string)"AnimaSowingMessageRitualRoleMustHaveFocus".Translate(props.requiredFocusDef.defName);
                return false;
            }
            if (p.GetPsylinkLevel() < props.neededPsyLevel)
            {
                if (!skipReason)
                    reason = (string)"AnimaSowingMessageRitualRoleNeedPsyLevel".Translate(props.neededPsyLevel);
                return false;
            }

            if (p.psychicEntropy.IsPsychicallySensitive)
                return true;
            if (!skipReason)
                reason = (string)"RitualTargetAnimaTreeMustBePsychicallySensitive".Translate();
            return false;
        }

        public override bool AppliesToRole(
          Precept_Role role,
          out string reason,
          Precept_Ritual ritual = null,
          Pawn p = null,
          bool skipReason = false)
        {
            reason = (string)null;
            return false;
        }
    }
}
