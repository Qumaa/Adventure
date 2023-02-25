using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;

public class GroundSensorVisualisation : MonoBehaviour, IVisualisible
{
    #region Values
    // public: Name
    // private, protected: _name
    // small scope: name
    // events: OnName-ed

    // Editor values
    [SerializeField] private Vector2 _size;

    // Internal values
    private IGroundSensor _visualise;
    private Vector2 _halfHeight, _halfWidth;

    // References


    // Properties
    private Vector2 _pos => transform.position;

    #endregion

    #region Mono and inherited methods

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // get references
        this.TryGetReference(ref _visualise);

        // set up values
        _halfHeight = new Vector2(0, _size.y / 2);
        _halfWidth = new Vector2(_size.x / 2, 0);
    }

    private void FixedUpdate()
    {
        Visualise();
    }

    #endregion

    #region GroundSensorVisualisation logic



    #endregion

    #region Math, shortcuts and utility



    #endregion

    // interfaces implementations
    public void Visualise()
    {
        Extensions.DrawRay(_pos, _halfWidth, _visualise.Right); // right
        Extensions.DrawRay(_pos, -_halfWidth, _visualise.Left); // left

        Extensions.DrawRay(_pos, _halfHeight, _visualise.Ceiling); // up
        Extensions.DrawRay(_pos, -_halfHeight, _visualise.Ground); // down
    }
}
