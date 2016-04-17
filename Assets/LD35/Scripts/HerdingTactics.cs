using UnityEngine;

namespace LD35 {

    public struct HerdingTarget {
        public static readonly HerdingTarget none = new HerdingTarget();

        public Sheep sheep;
        public Vector2 polarPosition;
    }


    public class HerdingTactics {

        public static HerdingTarget FindTarget(Vector2 currentPolarPos, float radiusPrio = 1f, float anglePrio = 1f) {
            if (Sheep.sheepList.Count == 0) return HerdingTarget.none;

            var target = new HerdingTarget();
            var maxWeight = 0f;

            foreach (var sheep in Sheep.sheepList) {
                var polarPos = World.GetPolar(sheep.planarPosition);
                var angularDist = World.AngularDistance(currentPolarPos.y, polarPos.y);

                var weight = polarPos.x * radiusPrio + (1f - angularDist / 180f) * anglePrio;
                if (weight < maxWeight) continue;

                maxWeight = weight;
                target.sheep = sheep;
                target.polarPosition = polarPos;
            }
            return target;
        }
    }
}
