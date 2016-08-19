using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Systems {
    class BallControl : System {
        public override void Update(Entity[] entities, float dt) {
            // handle physics
            foreach (Entity entity in entities) {
                // Apply velocity
                Components.Position posComp = entity.GetComponent<Components.Position>();
                Components.Velocity velComp = entity.GetComponent<Components.Velocity>();
                Components.Collidable collider = entity.GetComponent<Components.Collidable>();

                if (posComp != null && velComp != null && collider != null) {
                    // bounce the ball off the sides
                    if (posComp.pos.X < 0 || posComp.pos.X > Game1.Width) {
                        velComp.vel.X *= -1;
                        if (posComp.pos.X < 0) {
                            posComp.pos.X *= -1;
                        } else {
                            posComp.pos.X -= 2 * (posComp.pos.X - Game1.Width);
                        }
                    }
                    if (posComp.pos.Y < 0 || posComp.pos.Y > Game1.Height) {
                        velComp.vel.Y *= -1;
                        if (posComp.pos.Y < 0) {
                            posComp.pos.Y *= -1;
                        } else {
                            posComp.pos.Y -= 2 * (posComp.pos.Y - Game1.Height);
                        }
                    }
                    posComp.pos += velComp.vel * dt;

                    collider.collider.Pos = posComp.pos;
                }
            }
            // handle collisions
            foreach (Entity entity in entities) {
                Components.Position posComp = entity.GetComponent<Components.Position>();
                Components.Velocity velComp = entity.GetComponent<Components.Velocity>();
                Components.Collidable collider = entity.GetComponent<Components.Collidable>();

                if (posComp != null && velComp != null && collider != null) {
                    foreach (Entity other in entities) {
                        Components.Collidable otherCollider = other.GetComponent<Components.Collidable>();
                        if (other != entity && otherCollider != null
                            && collider.collider.Intersects(otherCollider.collider)) {
                            // calculate impulse
                            float v_x = velComp.vel.X;
                            float v_y = velComp.vel.Y;
                            float ov_x = 0;
                            float ov_y = 0;
                            // account for other impulse if the other object has velocity
                            Components.Position otherPosComp = other.GetComponent<Components.Position>();
                            Components.Velocity otherVelComp = other.GetComponent<Components.Velocity>();
                            if (otherVelComp != null) {
                                ov_x = otherVelComp.vel.X;
                                ov_y = otherVelComp.vel.Y;
                            }
                            // calculate and apply impulse
                            velComp.vel.X = (v_x * (collider.mass - otherCollider.mass)
                                             + 2 * otherCollider.mass * ov_x)
                                             / (collider.mass + otherCollider.mass);
                            velComp.vel.Y = (v_y * (collider.mass - otherCollider.mass)
                                             + 2 * otherCollider.mass * ov_y)
                                             / (collider.mass + otherCollider.mass);
                            posComp.pos.X += velComp.vel.X;
                            posComp.pos.Y += velComp.vel.Y;
                            // apply impulse to other object
                            if (otherVelComp != null) {
                                otherVelComp.vel.X = (ov_x * (otherCollider.mass - collider.mass)
                                                      + 2 * collider.mass * v_x)
                                                      / (collider.mass + otherCollider.mass);
                                otherVelComp.vel.Y = (ov_y * (otherCollider.mass - collider.mass)
                                                      + 2 * collider.mass * v_y)
                                                      / (collider.mass + otherCollider.mass);
                                if (otherPosComp != null) {
                                    otherPosComp.pos.X += velComp.vel.X;
                                    otherPosComp.pos.Y += velComp.vel.Y;
                                }
                            }                            
                        }
                    }
                }
            }
        }
    }
}
