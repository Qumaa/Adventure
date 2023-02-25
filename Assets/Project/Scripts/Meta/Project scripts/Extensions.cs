using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private const string REFERENCE_FAILED = "Couldn't get {0} component for {1} object. Reference it manually or add a corresponding component";
    private const string REFERENCE_UNABLE = "{0} is empty and is not a component. Reference it manually to {1}";

    /// <summary>
    /// This method tries to get a reference that would correspond a given object.
    /// If object is not null, no actions are taken.
    /// If object can only be assigned manually or is missing, corresponding logs are printed to console.
    /// </summary>
    /// <typeparam name="TComponent">The object's type</typeparam>
    /// <param name="mono">The mono behaviour object</param>
    /// <param name="reference">The object</param>
    /// <param name="override">Override the value if it is not empty?</param>
    /// <returns>Returns a bool of whether the value has or not been affected</returns>
    public static bool TryGetReference<TComponent>
        (this MonoBehaviour mono, ref TComponent reference) 
    {
        // don't do anything if value is filled
        if (reference != null) return false;

        reference = mono.TryGetReference<TComponent>();

        return !reference.Equals(default(TComponent));
    }

    public static TComponent TryGetReference<TComponent>(this MonoBehaviour mono)
    {
        // check if generic can be assigned
        // pass if type is interface
        if (!typeof(TComponent).IsInterface && !typeof(Component).IsAssignableFrom(typeof(TComponent)))
        {
            // abort if not
            Debug.LogWarning(string.Format(REFERENCE_UNABLE, typeof(TComponent).Name, mono.gameObject.name));
            return default;
        }

        // return component
        if (mono.TryGetComponent(out TComponent component)) return component;

        // couldn't get component
        Debug.LogWarning(string.Format(REFERENCE_FAILED, typeof(TComponent).Name, mono.gameObject.name));
        return default;
    }

    public static void DrawRay(Vector2 from, Vector2 offset, bool triggered = false) =>
        Extensions.DrawLine(from, from + offset, triggered);

    public static void DrawLine(Vector2 from, Vector2 to, bool triggered = false) =>
        Debug.DrawLine(from, to, triggered ? Color.red : Color.green);
}
