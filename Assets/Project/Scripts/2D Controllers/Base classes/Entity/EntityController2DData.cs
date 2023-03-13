using UnityEngine;
using Project.States;

namespace Project.Controller2D
{
    public class EntityController2DData<TGroundSensor> where TGroundSensor : IGroundSensor
    {
        public readonly CapsuleCollider2D Capsule;
        public readonly EntityController2DSettings Settings;
        public readonly IStateManager StateManager;
        public readonly TGroundSensor Sensor;
        public readonly Rigidbody2DHandlerFacade HandlerFacade;

        // constructor
        public EntityController2DData(
            CapsuleCollider2D capsule,
            EntityController2DSettings settings,
            IStateManager stateManager,
            TGroundSensor groundSensor,
            Rigidbody2DHandlerFacade handlerFacade)
        {
            Capsule = capsule;
            Settings = settings;
            StateManager = stateManager;
            Sensor = groundSensor;
            HandlerFacade = handlerFacade;
        }
    }

}
