using RimWorld;
using Verse;

namespace AnimalCostumes
{
	class Verb_Jump : RimWorld.Verb_Jump
	{		
		protected override bool TryCastShot()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Items with jump capability are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 550187797);
				return false;
			}

            if (base.CasterPawn == null || !(base.DirectOwner is CompPawnPowered ccpp))
            {
                return false;
            }

            IntVec3 cell = currentTarget.Cell;
			Map map = base.CasterPawn.Map;
			ccpp.UsedOnce();
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ACThingDefOf.ACPawnJumper, base.CasterPawn, cell);
			if (pawnFlyer != null)
			{
				GenSpawn.Spawn(pawnFlyer, cell, map);
				return true;
			}
			return false;
		}

		
		/*private float cachedEffectiveRange = -1f;

		public override void ExposeData()
		{
			base.ExposeData();
		}

		protected override float EffectiveRange
		{
			get
			{
				if (cachedEffectiveRange < 0f)
				{
					if (base.DirectOwner is CompAnimalCostumePawnPowered ccpp)
							cachedEffectiveRange = ccpp.parent.GetStatValue(StatDefOf.JumpRange);
				}
				return cachedEffectiveRange;
			}
		}

		public override bool MultiSelect => true;

		public override bool Available()
		{
			return base.CasterPawn != null && (base.DirectOwner as CompAnimalCostumePawnPowered)?.CanBeUsed == true;
		}

		protected override bool TryCastShot()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Items with jump capability are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 550187797);
				return false;
			}

			CompAnimalCostumePawnPowered ccpp = base.DirectOwner as CompAnimalCostumePawnPowered;

			if (base.CasterPawn == null || ccpp == null)
			{
				return false;
			}

			IntVec3 cell = currentTarget.Cell;
			Map map = base.CasterPawn.Map;
			ccpp.UsedOnce();
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ACThingDefOf.ACPawnJumper, base.CasterPawn, cell);
			if (pawnFlyer != null)
			{
				GenSpawn.Spawn(pawnFlyer, cell, map);
				return true;
			}
			return false;
		}

		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Map map = CasterPawn.Map;
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear_NewTemp(target.Cell, CasterPawn, AcceptableDestination);
			Job job = JobMaker.MakeJob(JobDefOf.CastJump, intVec);
			job.verbToUse = this;
			if (CasterPawn.jobs.TryTakeOrderedJob(job))
			{
				MoteMaker.MakeStaticMote(intVec, map, ThingDefOf.Mote_FeedbackGoto);
			}
			bool AcceptableDestination(IntVec3 c)
			{
				if (ValidJumpTarget(map, c))
				{
					return CanHitTargetFrom(caster.Position, c);
				}
				return false;
			}
		}

		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (caster == null)
			{
				return false;
			}
			if (!CanHitTarget(target) || !ValidJumpTarget(caster.Map, target.Cell))
			{
				return false;
			}
			if (!ReloadableUtility.CanUseConsideringQueuedJobs(CasterPawn, base.EquipmentSource))
			{
				return false;
			}
			return true;
		}

		public override void OnGUI(LocalTargetInfo target)
		{
			if (CanHitTarget(target) && ValidJumpTarget(caster.Map, target.Cell))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && ValidJumpTarget(caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(caster.Position, EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(caster.Position, c, caster.Map) && ValidJumpTarget(caster.Map, c));
		}

		public static bool ValidJumpTarget(Map map, IntVec3 cell)
		{
			if (!cell.IsValid || !cell.InBounds(map))
			{
				return false;
			}
			if (cell.Impassable(map) || !cell.Walkable(map) || cell.Fogged(map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			Building_Door building_Door;
			if (edifice != null && (building_Door = (edifice as Building_Door)) != null && !building_Door.Open)
			{
				return false;
			}
			return true;
		}*/
	}
}