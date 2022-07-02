using RimWorld;
using Verse;

namespace AnimalCostumes
{
    [DefOf]
    public static class ACThoughtDefOf
    {
        public static TraitDef AnimalCostumes_Furry;

        static ACThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TraitDefOf));
        }
    }

    public class ThoughtWorker_WornThoughts : ThoughtWorker
    {
        public ThoughtWorker_WornThoughts() { }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Trait furryTrait = p.story.traits.GetTrait(ACThoughtDefOf.AnimalCostumes_Furry);
            if (furryTrait != null)
            {
                var bodyPieces = CostumeType.None;
                string breed = "";
                bool sameBreed = false;
                foreach (Apparel a in p.apparel.WornApparel)
                {
                    if (a.def is AnimalCostumeDef def)
                    {
                        if (breed.NullOrEmpty())
                            breed = def.CostumeBreed;
                        else
                            sameBreed = breed.Equals(def.CostumeBreed);

                        bodyPieces |= def.CostumeType;
                    }
                }

                if (!breed.NullOrEmpty())
                {
                    if ((bodyPieces & CostumeType.Body) != 0 && (bodyPieces & CostumeType.Head) != 0)
                    {
                        if (furryTrait.Degree == 0)
                            return ThoughtState.ActiveAtStage(6);
                        else if (furryTrait.Degree == 1 && sameBreed)
                            return ThoughtState.ActiveAtStage(8);
                    }
                    else if ((bodyPieces & CostumeType.Tail) != 0 && (bodyPieces & CostumeType.Ears) != 0)
                    {
                        if (furryTrait.Degree == 0)
                            return ThoughtState.ActiveAtStage(3);
                        else if (furryTrait.Degree == 1 && sameBreed)
                            return ThoughtState.ActiveAtStage(7);
                    }
                    else if (furryTrait.Degree == 0)
                    {
                        if ((bodyPieces & CostumeType.Body) != 0)
                            return ThoughtState.ActiveAtStage(5);
                        if ((bodyPieces & CostumeType.Head) != 0)
                            return ThoughtState.ActiveAtStage(4);
                        if ((bodyPieces & CostumeType.Tail) != 0)
                            return ThoughtState.ActiveAtStage(2);
                        if ((bodyPieces & CostumeType.Ears) != 0)
                            return ThoughtState.ActiveAtStage(1);
                    }
                    else if (sameBreed && furryTrait.Degree == 1 &&
                        ((bodyPieces & CostumeType.Body) != 0 && (bodyPieces & CostumeType.Ears) != 0 ||
                         (bodyPieces & CostumeType.Tail) != 0 && (bodyPieces & CostumeType.Head) != 0))
                    {
                        return ThoughtState.ActiveAtStage(7);
                    }
                }
                return ThoughtState.ActiveAtStage(0);
            }
            return ThoughtState.Inactive;
        }
    }

    public class ThoughtWorker_Milkable : ThoughtWorker
    {
        public ThoughtWorker_Milkable() { }
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            CompMilkable_AC milkable = null;
            bool hasCowHood = false;
            foreach (Apparel a in p.apparel.WornApparel)
            {
                var def = a.def as AnimalCostumeDef;
                if (def != null)
                {
                    if (def.CostumeType == CostumeType.Head && def.CostumeBreed == "Cow")
                    {
                        hasCowHood = true;
                    }
                    else if ((a as AnimalCostume).CompThingGenerator is CompMilkable_AC m)
                    {
                        milkable = m;
                        break;
                    }
                }
            }

            if (milkable != null)
            {
                bool isFemale = hasCowHood || p.gender == Gender.Female;
                if (milkable.Fullness > 0.75f)
                {
                    return ThoughtState.ActiveAtStage(isFemale ? 5 : 0);
                }
                else if (milkable.WasJustMilked)
                {
                    if (milkable.Fullness < 0.1f)
                    {
                        return ThoughtState.ActiveAtStage(isFemale ? 6 : 1);
                    }
                    milkable.WasJustMilked = false;
                }
                else if (milkable.Fullness > 0.5f)
                    return ThoughtState.ActiveAtStage(7);
            }
            return ThoughtState.Inactive;
        }
    }

    public class ThoughtWorker_Shedding : ThoughtWorker
    {
        public ThoughtWorker_Shedding() { }
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            foreach (Apparel a in p.apparel.WornApparel)
            {
                if (a is AnimalCostume ac && ac.CompThingGenerator is CompShearable_AC s)
                {
                    if (s.Fullness > 0.85f)
                    {
                        return ThoughtState.ActiveAtStage(0);
                    }
                    else if (s.WasJustShedded)
                    {
                        if (s.Fullness < 15f)
                        {
                            return ThoughtState.ActiveAtStage(1);
                        }
                        s.WasJustShedded = false;
                    }
                    return ThoughtState.Inactive;
                }
            }
            return ThoughtState.Inactive;
        }
    }
}