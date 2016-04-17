using UnityEngine;

namespace LD35 {

    public struct HerdingTarget {
        public static readonly HerdingTarget none = new HerdingTarget();

        public Sheep sheep;
        public Vector2 polarPosition;
    }


    public class HerdingTactics {

        public static HerdingTarget FindTarget(Vector2 currentPolarPos) {
            if (Sheep.sheepList.Count == 0) return HerdingTarget.none;

            var target = new HerdingTarget();
            var maxRadiusSq = 0f;

            foreach (var sheep in Sheep.sheepList) {
                var radiusSq = sheep.planarPosition.sqrMagnitude;
                if (radiusSq < maxRadiusSq) continue;

                maxRadiusSq = radiusSq;
                target.sheep = sheep;
            }

            target.polarPosition = World.GetPolar(target.sheep.planarPosition);
            target.polarPosition.x += 2f;

            return target;
        }
    }
}
