using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Systems {
    class BallControl : System {
        public override void Update(Entity[] entities, float dt) {
            foreach (Entity entity in entities) {
                Components.Position posComp = entity.GetComponent<Components.Position>();
                Components.Velocity velComp = entity.GetComponent<Components.Velocity>();
                if (posComp != null && velComp != null) {
                    posComp.pos += velComp.vel * dt;
                }
            }
        }
    }
}
