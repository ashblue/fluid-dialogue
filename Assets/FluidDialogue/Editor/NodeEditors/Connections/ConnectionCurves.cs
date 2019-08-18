using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial class Connection {
        private bool _exampleCurveActive;
        private Vector2 _exampleCurveTarget;

        public void SetExampleCurve (Vector2 position) {
            _exampleCurveActive = true;
            _exampleCurveTarget = position;
        }

        public void ClearCurveExample () {
            _exampleCurveActive = false;
        }

        private void PaintCurve (Vector2 destination) {
            var curveMaxDistance = Vector2.Distance(_rect.center, destination) / 200;
            var curveWeight = Mathf.Lerp(0, 50, curveMaxDistance);

            Handles.DrawBezier(
                _rect.center,
                destination,
                _rect.center + Vector2.right * curveWeight,
                destination + Vector2.left * curveWeight,
                Color.cyan,
                null,
                2f
            );
            GUI.changed = true;
        }
    }
}
