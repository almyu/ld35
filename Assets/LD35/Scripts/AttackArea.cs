using UnityEngine;

namespace LD35 {

    public class AttackArea : MonoBehaviour {

        public float range = 3f;
        public SpriteRenderer rangeSprite, markerSprite;
        public Color invalidColor = Color.red, validColor = Color.yellow + Color.green;

        [HideInInspector]
        public Sheep victim;

        private Vector3 initialScale;

        private void Awake() {
            initialScale = rangeSprite.transform.localScale;
        }

        private void OnEnable() {
            rangeSprite.transform.localScale = initialScale * range;
        }

        private void Update() {
            victim = Sheep.GetAnyInRange(transform.position, range);
        }

        private void LateUpdate() {
            rangeSprite.color = victim ? validColor : invalidColor;
            markerSprite.enabled = victim;
            if (victim) markerSprite.transform.position = victim.planarPosition.WithY(transform.position.y);
        }

        public bool Attack() {
            if (!victim) return false;

            // HACK
            Shepherd.instance.transform.position = victim.planarPosition.WithY(transform.position.y);

            victim.Eat();
            return true;
        }
    }
}
