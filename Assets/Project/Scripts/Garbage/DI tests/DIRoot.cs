using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIRoot
{
    // container: a set of registered objects. Created as new objects, then builded with register methods
    //
    // lifetime:
    //   transient:  new instance every time.
    //   scoped:     involves opening and closing a request. One same instance is returned thoughout the request,
    //               but new instance is created with every new request.
    //   singleton:  creates one instance that is never destroyed.
    //
    // bind: ability to associate abstract type with concrete type
    //
    // resolve: retaining an object from the container

    // sequence:
    // 1. create a container builder, register everything and build
    // 2. in order to get instances, request a scope from the container
    // 3. use the obtained scope and its .Resolve method   
}

namespace Project.Dependecies
{
    public enum DependencyLifetime : byte
    {
        Transient, // new instance every time
        Scoped,    // one instance per scope
        Singleton  // one instance
    }

    public interface IScope
    {
        object Resolve(Type type);
    }

    public class DependencyContainer
    {
        #region Values
        private readonly Dictionary<Type, DependencyConfig> _configsRegistry;
        private readonly Dictionary<Type, object> _singletonRegistry = new Dictionary<Type, object>();

        public DependencyContainer(IEnumerable<DependencyConfig> configs)
        {
            _configsRegistry = configs.ToDictionary(x => x.Type);
        }
        #endregion

        #region Creating objects
        private object CreateInstance(Type type)
        {
            ConstructorInfo constructor = GetBestConstructor(type);
            ParameterInfo[] parameterInfos = constructor.GetParameters();
            object[] args = new object[parameterInfos.Length];

            for (int i = 0; i < args.Length; i++)
                args[i] = CreateInstance(parameterInfos[i].ParameterType);

            return constructor.Invoke(args);
        }
        private ConstructorInfo GetBestConstructor(Type type)
        {
            return type.GetConstructors()
            .Aggregate((prev, next) =>
            prev.GetParameters().Length < next.GetParameters().Length ?
            prev :
            next);
        }
        #endregion

        #region Registry interactions
        private DependencyConfig FindConfig(Type type)
        {
            return _configsRegistry[type];
        }

        private object FindOrCreate(Type type, Dictionary<Type, object> registry)
        {
            if (!registry.ContainsKey(type))
                registry.Add(type, CreateInstance(type));

            return registry[type];
        }

        private object FindSingleton(Type type) => FindOrCreate(type, _singletonRegistry);
        #endregion

        #region Scope
        public IScope GetScope() => new Scope(this);

        private class Scope : IScope
        {
            private readonly DependencyContainer _parentContainer;
            private readonly Dictionary<Type, object> _scopedRegistry = new Dictionary<Type, object>();

            public Scope(DependencyContainer parentContainer)
            {
                _parentContainer = parentContainer;
            }

            public object Resolve(Type type)
            {
                DependencyConfig config = _parentContainer.FindConfig(type);

                return config.Lifetime switch
                {
                    DependencyLifetime.Transient => _parentContainer.CreateInstance(type),
                    DependencyLifetime.Scoped => this.FindScoped(type),
                    DependencyLifetime.Singleton => _parentContainer.FindSingleton(type),
                    _ => default,
                };
            }

            private object FindScoped(Type type) => _parentContainer.FindOrCreate(type, _scopedRegistry);
        }
        #endregion
    }

    public class DependencyContainerBuilder
    {
        private readonly List<DependencyConfig> _registeredConfigs = new List<DependencyConfig>();

        public void Register(DependencyConfig config)
        {
            _registeredConfigs.Add(config);
        }

        public DependencyContainer Build()
        {
            return new DependencyContainer(_registeredConfigs);
        }
    }

    public class DependencyConfig
    {
        public readonly Type Type;
        public readonly DependencyLifetime Lifetime;

        public DependencyConfig(Type type, DependencyLifetime lifetime)
        {
            this.Type = type;
            this.Lifetime = lifetime;
        }
    }
}
