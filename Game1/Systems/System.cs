using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ECS.Systems {
    abstract class System {
        public bool Active { get; set; } = true;

        private Dictionary<String, Texture2D> textures;

        public System() {
            textures = new Dictionary<String, Texture2D>();
            LoadContent();
        }

        protected virtual void LoadContent() { }

        public void LoadTexture(Game1 game, String name, String filename) {
            textures.Add(name, game.Content.Load<Texture2D>(filename));
        }
        public Texture2D GetTexture(String name) {
            Texture2D texture;
            if (!textures.TryGetValue(name, out texture)) {
                return null;
            }
            return texture;
        }

        public virtual void Update(Entity[] entities, float dt) { }
        public virtual void Render(Entity[] entities, SpriteBatch spriteBatch, float dt) { }
    }
}
