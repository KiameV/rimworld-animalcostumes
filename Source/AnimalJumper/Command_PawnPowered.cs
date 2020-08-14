using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AnimalCostumes
{
	public class Command_AnimalCostumePawnPowered : Command_VerbTarget
	{
		private readonly CompPawnPowered comp;

		public Color? overrideColor;

		public override string TopRightLabel => comp.LabelRemaining;

		public override Color IconDrawColor => overrideColor ?? base.IconDrawColor;

		public Command_AnimalCostumePawnPowered(CompPawnPowered comp)
		{
			this.comp = comp;
		}

		public override void GizmoUpdateOnMouseover()
		{
			verb.DrawHighlight(verb.caster);
		}

		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
            if (!(other is Command_AnimalCostumePawnPowered c))
            {
                return false;
            }
            return comp.parent.def == c.comp.parent.def;
		}
	}
}
