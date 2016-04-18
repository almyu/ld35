using UnityEngine;
using System.Collections;

public class Screenshake : MonoSingleton<Screenshake> {

    public Vector2 power = Vector2.one;
    public float duration = 0.2f;

    private Vector3 offset;
    private float timer;

    public static void Activate(float time) {
        if (instance) instance.timer += time;
    }

    public static void Activate() {
        if (instance) instance.timer += instance.duration;
    }

    private void Update() {
        var cam = Camera.main.transform;

        cam.position -= offset;
        offset = Vector3.zero;

        if (timer <= 0f) return;
        timer -= Time.unscaledDeltaTime;

        offset = Vector2.Scale(Random.onUnitSphere, power).WithZ(0f);
        offset = cam.transform.rotation * offset;
        cam.position += offset;
    }
}
