using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRM
{
    public enum BlendShape
    {
        Neutral,
        A,
        I,
        U,
        E,
        O,
        Blink,
        Blink_L,
        Blink_R,
        Angry,
        Fun,
        Joy,
        Sorrow,
        Surprised,
        LookUp,
        LookDown,
        LookLeft,
        LookRight,
    }

    [RequireComponent(typeof(FaceMovementTracker))]
    public class VRMBlendShapeController : MonoBehaviour
    {
        [SerializeField]
        private bool deactivateBlendShapeController = false;

        [SerializeField]
        private bool deactivateBlendShapeResetter = false;

        [SerializeField]
        [Range(1, 10)]
        private float blendShapeResetterSpeed = 3;

        [SerializeField]
        [Range(0.001f, 0.05f)]
        private float A_mouthVerticalOpennessMin = 0.006f;
        [SerializeField]
        [Range(0.001f, 0.05f)]
        private float A_mouthVerticalOpennessMax = 0.015f;

        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float O_mouthHorizontalOpennessMin = 0.042f;
        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float O_mouthHorizontalOpennessMax = 0.046f;

        [SerializeField]
        [Range(0.03f, 0.05f)]
        private float surprised_eyebrowRaisedHeightMin = 0.040f;
        [SerializeField]
        [Range(0.03f, 0.05f)]
        private float surprised_eyebrowRaisedHeightMax = 0.0465f;

        [SerializeField]
        [Range(0.03f, 0.05f)]
        private float angry_eyebrowRaisedHeightMin = 0.0355f;
        [SerializeField]
        [Range(0.03f, 0.05f)]
        private float angry_eyebrowRaisedHeightMax = 0.037f;

        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float fun_mouthHorizontalOpennessMin = 0.053f;
        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float fun_mouthHorizontalOpennessMax = 0.058f;

        [SerializeField]
        [Range(0.001f, 0.05f)]
        private float joy_mouthVerticalOpennessMin = 0.01f;
        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float joy_mouthHorizontalOpennessMin = 0.06f;
        [SerializeField]
        [Range(0.04f, 0.07f)]
        private float joy_mouthHorizontalOpennessMax = 0.069f;

        public BlendShape currentTargetMouthBlendShape { get; private set; }
        public BlendShape currentTargetExpressionBlendShape { get; private set; }

        private VRMBlendShapeProxy target;
        private FaceMovementTracker faceMovementTracker;

        private float mouthHorizontalOpenness;
        private float mouthVerticalOpenness;
        private float eyebrowRaisedHeight;

        private float mouthHorizontalOpennessNormalized;
        private float mouthVerticalOpennessNormalized;
        private float surprisedEyebrowRaisedHeightNormalized;
        private float angryEyebrowRaisedHeightNormalized;
        private float funMouthHorizontalOpennessNormalized;
        private float joyMouthHorizontalOpennessNormalized;

        void Awake() {
            target = GetComponent<VRMBlendShapeProxy>();
            faceMovementTracker = GetComponent<FaceMovementTracker>();
        }

        void Update() {
            UpdateFields();
            NeutralExpressionBalancer();

            if (!deactivateBlendShapeController) {
                ControlBlendShape();
            } else {
                currentTargetMouthBlendShape = BlendShape.Neutral;
                currentTargetExpressionBlendShape = BlendShape.Neutral;
            }

            if (!deactivateBlendShapeResetter) {
                ResetAllBlendShapes();
            }
        }

        private void UpdateFields() {
            mouthHorizontalOpenness = faceMovementTracker.GetMouthHorizontalOpenness();
            mouthVerticalOpenness = faceMovementTracker.GetMouthVerticalOpenness();
            eyebrowRaisedHeight = faceMovementTracker.GetEyebrowRaisedHeight();

            mouthHorizontalOpennessNormalized = Normalize(mouthHorizontalOpenness, O_mouthHorizontalOpennessMin, O_mouthHorizontalOpennessMax);
            mouthVerticalOpennessNormalized = Normalize(mouthVerticalOpenness, A_mouthVerticalOpennessMin, A_mouthVerticalOpennessMax);
            surprisedEyebrowRaisedHeightNormalized = Normalize(eyebrowRaisedHeight, surprised_eyebrowRaisedHeightMin, surprised_eyebrowRaisedHeightMax);
            angryEyebrowRaisedHeightNormalized = Normalize(eyebrowRaisedHeight, angry_eyebrowRaisedHeightMin, angry_eyebrowRaisedHeightMax);
            funMouthHorizontalOpennessNormalized = Normalize(mouthHorizontalOpenness, fun_mouthHorizontalOpennessMin, fun_mouthHorizontalOpennessMax);
            joyMouthHorizontalOpennessNormalized = Normalize(mouthHorizontalOpenness, joy_mouthHorizontalOpennessMin, joy_mouthHorizontalOpennessMax);

            // Mouth 
            if (mouthHorizontalOpenness > O_mouthHorizontalOpennessMax
                    && mouthVerticalOpenness > A_mouthVerticalOpennessMin) {
                currentTargetMouthBlendShape = BlendShape.A;
            } else if (mouthHorizontalOpenness < O_mouthHorizontalOpennessMax) {
                currentTargetMouthBlendShape = BlendShape.O;
            }

            // expression
            if (eyebrowRaisedHeight > surprised_eyebrowRaisedHeightMin) {
                currentTargetExpressionBlendShape = BlendShape.Surprised;
            } else if (eyebrowRaisedHeight < angry_eyebrowRaisedHeightMin) {
                currentTargetExpressionBlendShape = BlendShape.Angry;
            } else if (mouthVerticalOpenness < joy_mouthVerticalOpennessMin
                    && mouthHorizontalOpenness > fun_mouthHorizontalOpennessMin) {
                currentTargetExpressionBlendShape = BlendShape.Fun;
            } else if (mouthVerticalOpenness > joy_mouthVerticalOpennessMin
                    && mouthHorizontalOpenness > joy_mouthHorizontalOpennessMin) {
                currentTargetExpressionBlendShape = BlendShape.Joy;
            }
        }

        private void ControlBlendShape() {
            // Mouth
            if (currentTargetMouthBlendShape == BlendShape.A) {
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.A), mouthVerticalOpennessNormalized);
            } else if (currentTargetMouthBlendShape == BlendShape.O) {
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.O), 1 - mouthHorizontalOpennessNormalized);
            }

            // Expressions
            if (currentTargetExpressionBlendShape == BlendShape.Surprised) {
                target.ImmediatelySetValue(BlendShapeKey.CreateUnknown("Surprised"), surprisedEyebrowRaisedHeightNormalized);
            } else if (currentTargetExpressionBlendShape == BlendShape.Angry) {
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry), 1 - angryEyebrowRaisedHeightNormalized);
            } else if (currentTargetExpressionBlendShape == BlendShape.Fun) {
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun), funMouthHorizontalOpennessNormalized);
            } else if (currentTargetExpressionBlendShape == BlendShape.Joy) {
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy), joyMouthHorizontalOpennessNormalized);
            }
        }

        private void ResetAllBlendShapes() {
            if (currentTargetMouthBlendShape != BlendShape.A) {
                ResetBlendShape(BlendShapePreset.A);
            }
            if (currentTargetMouthBlendShape != BlendShape.I) {
                ResetBlendShape(BlendShapePreset.I);
            }
            if (currentTargetMouthBlendShape != BlendShape.U) {
                ResetBlendShape(BlendShapePreset.U);
            }
            if (currentTargetMouthBlendShape != BlendShape.E) {
                ResetBlendShape(BlendShapePreset.E);
            }
            if (currentTargetMouthBlendShape != BlendShape.O) {
                ResetBlendShape(BlendShapePreset.O);
            }
            if (currentTargetExpressionBlendShape != BlendShape.Fun) {
                ResetBlendShape(BlendShapePreset.Fun);
            }
            if (currentTargetExpressionBlendShape != BlendShape.Joy) {
                ResetBlendShape(BlendShapePreset.Joy);
            }
            if (currentTargetExpressionBlendShape != BlendShape.Surprised) {
                ResetBlendShape("Surprised");
            }
            if (currentTargetExpressionBlendShape != BlendShape.Angry) {
                ResetBlendShape(BlendShapePreset.Angry);
            }
        }

        private void ResetBlendShape(BlendShapePreset b) {
            float bValue = target.GetValue(BlendShapeKey.CreateFromPreset(b));
            if (bValue > 0) {
                float newValue = bValue - (blendShapeResetterSpeed * Time.deltaTime);
                if (newValue < 0) {
                    newValue = 0;
                }
                target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(b), newValue);
            }
        }

        private void ResetBlendShape(string s) {
            float bValue = target.GetValue(BlendShapeKey.CreateUnknown(s));
            if (bValue > 0) {
                float newValue = bValue - (blendShapeResetterSpeed * Time.deltaTime);
                if (newValue < 0) {
                    newValue = 0;
                }
                target.ImmediatelySetValue(BlendShapeKey.CreateUnknown(s), newValue);
            }
        }

        private float Normalize(float x, float min, float max) {
            float norm = (x - min) / (max - min);
            if (norm > 1) {
                norm = 1;
            }
            if (norm < 0) {
                norm = 0;
            }
            return norm;
        }

        /// <summary>
        /// The Neutral Blend Shape has a closed lip and the other Blend Shapes has closed lip as well.
        /// If Neutral and another Blend Shape are active, it makes the lip extra closed.
        /// For fix, this method will balance the value of Neutral and the other Blend Shape that are active. 
        /// </summary>
        private void NeutralExpressionBalancer() {
            target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), 1);

            float neutralVal = target.GetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral));
            float blinkVal = target.GetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink));
            float angryVal = target.GetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry));
            float funVal = target.GetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun));
            float surprisedVal = target.GetValue(BlendShapeKey.CreateUnknown("Surprised"));

            float newNeutralValue = neutralVal - (blinkVal + angryVal + funVal + surprisedVal);
            target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), newNeutralValue);
        }
    }
}