using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS {
    class Engine {
        public class Prototype {
            private List<Type> components;

            public Prototype AddComponent<ComponentType>()
                where ComponentType : Components.Component {
                Type type = typeof(ComponentType);
                if (!components.Contains(type)) {
                    components.Add(type);
                }
                return this;
            }

            internal Entity GetEntity() {
                Entity entity = new Entity();
                foreach (Type type in components) {
                    entity.AddComponent(type);
                }
                return entity;
            }
        }

        private Dictionary<Type, Systems.System> systems;
        private List<Entity> entities;
        private Dictionary<String, Prototype> prototypes;

        public Engine() {
            systems = new Dictionary<Type, Systems.System>();
            entities = new List<Entity>();
        }

        public Prototype AddPrototype(String name) {
            Prototype prototype = new Prototype();
            prototypes.Add(name, prototype);
            return prototype;
        }

        public Entity AddEntityFromPrototype(String name) {
            Prototype prototype;
            if (!prototypes.TryGetValue(name, out prototype)) {
                return null;
            }
            Entity entity = prototype.GetEntity();
            entities.Add(entity);
            return entity;
        }

        // Adds a new system of the given type. Does nothing if the
        // given system is already registered.
        public Systems.System AddSystem<SystemType>()
            where SystemType : Systems.System, new() {
            Type type = typeof(SystemType);

            if (!systems.ContainsKey(type)) {
                Systems.System system = new SystemType();
                systems.Add(type, system);
                return system;
            }
            return null;
        }

        public Entity AddEntity() {
            Entity entity = new Entity();
            entities.Add(entity);
            return entity;
        }

        private Entity[] GetEntities() {
            return entities.ToArray() as Entity[];
        }

        public void Update(GameTime gameTime) {
            foreach (Systems.System system in systems.Values) {
                system.Update(GetEntities(), 16f / gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        public void Render(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (Systems.System system in systems.Values) {
                system.Render(GetEntities(), spriteBatch, 16f / gameTime.ElapsedGameTime.Milliseconds);
            }
        }
    }
}
