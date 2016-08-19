using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS {
    using Components;

    class Entity {
        private int id;
        private Dictionary<Type, Component> components;

        private static int _maxId = 0;

        public int ID { get { return id; } }

        public Entity() {
            components = new Dictionary<Type, Component>();

            id = _maxId;
            ++_maxId;
        }

        // Adds a new component of the given type. Does nothing if the
        // given component is already attached to the entity.
        // Allows chaining.
        public Entity AddComponent<ComponentType>()
            where ComponentType : Component, new() {
            Type type = typeof(ComponentType);
            if (!components.ContainsKey(type)) {
                components.Add(type, new ComponentType());
            }
            return this;
        }

        public Entity AddComponent(Type type) {
            if (!components.ContainsKey(type)) {
                components.Add(type, Activator.CreateInstance(type) as Component);
            }
            return this;
        }

        public bool ContainsType(Type type) {
            return components.ContainsKey(type);
        }

        public Component GetComponent(Type type) {
            Component component;
            if (components.TryGetValue(type, out component)) {
                return component;
            }
            return null;
        }

        // Gets the given component, or null if there is none.
        public ComponentType GetComponent<ComponentType>()
            where ComponentType : Component {
            Component component;
            if (!components.TryGetValue(typeof(ComponentType),
                                        out component)) {
                return null;
            }
            if (!component.Active) {
                return null;
            }
            return component as ComponentType;
        }

        // Disables the given component. Allows chaining.
        public Entity DisableComponent<ComponentType>()
            where ComponentType : Component {
            ComponentType component = GetComponent<ComponentType>();
            if (component != null) {
                component.Active = false;
            }
            return this;
        }

        // Enables the given component. Allows chaining.
        public Entity EnableComponent<ComponentType>()
            where ComponentType : Component {
            ComponentType component = GetComponent<ComponentType>();
            if (component != null) {
                component.Active = true;
            }
            return this;
        }

        public override string ToString() {
            string str = base.ToString() + " (id " + id + ")";
            foreach (var entry in components) {
                Component component = entry.Value;
                str += "\n\t" + component.ToString();
            }
            return str;
        }
    }
}
