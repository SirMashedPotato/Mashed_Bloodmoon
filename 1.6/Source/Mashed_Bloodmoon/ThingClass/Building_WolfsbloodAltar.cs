using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Building_WolfsbloodAltar : Building
    {
        private const float maxBloodAmount = 1f;
        private const float defaultAddAmount = 0.1f;
        private const float defaultConsumeAmount = 0.1f;
        private float lycanthropeBloodAmount = 0;

        private Graphic fillStageOne = null;
        private Graphic fillStageTwo = null;
        private Graphic fillStageThree = null;
        private Graphic fillStageFour = null;

        private Graphic FillStageOne
        {
            get
            {
                if (fillStageOne == null)
                {
                    fillStageOne = GraphicDatabase.Get<Graphic_Single>(Graphic.path + "_25", Graphic.Shader, Graphic.drawSize, Color.white);
                }
                return fillStageOne;
            }
        }

        private Graphic FillStageTwo
        {
            get
            {
                if (fillStageTwo == null)
                {
                    fillStageTwo = GraphicDatabase.Get<Graphic_Single>(Graphic.path + "_50", Graphic.Shader, Graphic.drawSize, Color.white);
                }
                return fillStageTwo;
            }
        }

        private Graphic FillStageThree
        {
            get
            {
                if (fillStageThree == null)
                {
                    fillStageThree = GraphicDatabase.Get<Graphic_Single>(Graphic.path + "_75", Graphic.Shader, Graphic.drawSize, Color.white);
                }
                return fillStageThree;
            }
        }

        private Graphic FillStageFour
        {
            get
            {
                if (fillStageFour == null)
                {
                    fillStageFour = GraphicDatabase.Get<Graphic_Single>(Graphic.path + "_Full", ShaderDatabase.TransparentPostLight, Graphic.drawSize, Color.white);
                }
                return fillStageFour;
            }
        }


        public bool IsFull() => lycanthropeBloodAmount >= maxBloodAmount;
        public bool IsEmpty() => lycanthropeBloodAmount <= 0f;
        public float FillAmount() => lycanthropeBloodAmount;
        public bool CanAddBlood(float addAmount = defaultAddAmount) => lycanthropeBloodAmount + addAmount <= maxBloodAmount;
        public bool CanConsumeBlood(float consumeAmount = defaultConsumeAmount) => lycanthropeBloodAmount - consumeAmount >= 0f;

        public float AddBlood(float addAmount = defaultAddAmount)
        {
            lycanthropeBloodAmount += addAmount;
            //otherwise we end up with an altar that contains 0.8000001 instead of 0.8
            lycanthropeBloodAmount = (float)Math.Round(lycanthropeBloodAmount, 2);
            return addAmount;
        }

        public void ConsumeBlood(float consumeAmount = defaultConsumeAmount)
        {
            lycanthropeBloodAmount -= consumeAmount;
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);

            drawLoc.y += 1f;
            if (lycanthropeBloodAmount >= 1)
            {
                FillStageFour.Draw(drawLoc, Rotation, this);
            }
            if (lycanthropeBloodAmount >= 0.75)
            {
                FillStageThree.Draw(drawLoc, Rotation, this);
                return;
            }
            if (lycanthropeBloodAmount >= 0.25)
            {
                FillStageTwo.Draw(drawLoc, Rotation, this);
                return;
            }
            if (lycanthropeBloodAmount > 0)
            {
                FillStageOne.Draw(drawLoc, Rotation, this);
                return;
            }
        }

        public override string GetInspectString()
        {
            return "Mashed_Bloodmoon_AltarFillAmount".Translate(lycanthropeBloodAmount.ToStringPercent()).CapitalizeFirst();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref lycanthropeBloodAmount, "lycanthropeBloodAmount", 0f);
            base.ExposeData();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Empty Altar",
                    action = delegate ()
                    {
                        ConsumeBlood(lycanthropeBloodAmount);
                    },
                    Disabled = IsEmpty()
                };

                yield return new Command_Action
                {
                    defaultLabel = "Fill Altar",
                    action = delegate ()
                    {
                        AddBlood(maxBloodAmount);
                    },
                    Disabled = IsFull()
                };
            }
        }
    }
}
