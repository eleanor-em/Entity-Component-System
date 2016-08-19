using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Colliders {
    abstract class Collider {
        public Vector2 Pos { get; set; }
        public abstract bool Intersects(Collider other);
    }
}
