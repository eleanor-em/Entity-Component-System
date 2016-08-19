using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS.Systems {
    class Renderer : System {
        public const float ScaleFactor = 0.2f;

        public override void Render(Entity[] entities, SpriteBatch spriteBatch, float dt) {
            foreach (Entity entity in entities) {
                Components.Position pos = entity.GetComponent<Components.Position>();
                Components.Velocity vel = entity.GetComponent<Components.Velocity>();
                Components.BallSprite bs = entity.GetComponent<Components.BallSprite>();
                
                if (pos != null && bs != null) {
                    Vector2 loc = pos.pos;
                    if (vel != null) {
                        loc += vel.vel / 2 * dt;
                    }

                    float scale = 1f;
                    Components.Collidable collider = entity.GetComponent<Components.Collidable>();
                    if (collider != null) {
                        scale = ScaleFactor * collider.mass;
                    }

                    spriteBatch.Draw(GetTexture("ball"), loc, null,
                                     Color.White, 0f, new Vector2(24, 24), scale,
                                     SpriteEffects.None, 0);
                }
            }
        }
    }
}
