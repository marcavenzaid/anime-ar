using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRM
{
    [RequireComponent(typeof(VRMBlendShapeController))]
    [RequireComponent(typeof(Blinker))]
    public class BlinkerController : MonoBehaviour
    {
        [SerializeField]
        private float disableBlinkJoyValue = 0.6f;
        [SerializeField]
        private float disableBlinkSurprisedValue = 0.7f;

        private VRMBlendShapeProxy target;
        //private VRMBlendShapeController vrmBlendShapeController;
        private Blinker blinker;

        private bool disableBlinker;

        void Awake() {
            target = GetComponent<VRMBlendShapeProxy>();
            //vrmBlendShapeController = GetComponent<VRMBlendShapeController>();
            blinker = GetComponent<Blinker>();

            disableBlinker = false;
        }

        void Update() {
            float joyValue = target.GetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy));
            float surprisedValue = target.GetValue(BlendShapeKey.CreateUnknown("Surprised"));

            disableBlinker = false;

            if (joyValue >= disableBlinkJoyValue) {
                disableBlinker = true;
            }

            if (surprisedValue >= disableBlinkSurprisedValue) {
                disableBlinker = true;
            }

            if (disableBlinker) {
                DisableBlinker();
            } else {
                EnableBlinker();
            }
        }

        private void EnableBlinker() {
            target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink), 0);
            blinker.enabled = true;
        }

        private void DisableBlinker() {
            target.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink), 0);
            blinker.enabled = false;
        }
    }
}