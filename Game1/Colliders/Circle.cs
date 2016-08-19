using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Colliders {
    class Circle : Collider {
        public float Radius { get; set; }

        private bool IntersectsCircle(Circle circle) {
            return (Pos - circle.Pos).Length() < Radius + circle.Radius;
        }

        public override bool Intersects(Collider other) {
            if (other is Circle) {
                return IntersectsCircle(other as Circle);
            }
            return false;
        }
    }
}
