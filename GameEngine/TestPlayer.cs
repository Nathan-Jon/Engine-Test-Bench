using DemonstrationEngine.Collision_Management;
using DemonstrationEngine.Physics;
using DemonstrationEngine.StateMachines;
using DemonstrationEngine.StateMachines.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace DemonstrationEngine
{
    class TestPlayer : PhysicsObject, ICollidable
    {

        private readonly IStateMachine<IPhysics> stateMachine;

        public float ForceX = 5;
        public float ForceY = 5;


        public TestPlayer()
        {
           stateMachine = new StateMachine<IPhysics>(this);

            stateMachine.AddState(new MoveLeft<IPhysics>(), "left");
            stateMachine.AddState(new MoveRight<IPhysics>(), "right");
            stateMachine.AddState(new FallState<IPhysics>(), "down");
            stateMachine.AddState(new JumpState<IPhysics>(), "up");

            stateMachine.AddMethodTransition(right, "left", "right");
            stateMachine.AddMethodTransition(left, "right", "left");
            stateMachine.AddMethodTransition(left, "right", "left");
            stateMachine.AddMethodTransition(left, "right", "left");
        }



        //State Methods
        private bool right()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D))
            {
                return true;
            }
            return false;

        }
        private  bool left()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                return true;
            }
            return false;
        }
        private  bool down()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S))
            {
                return true;
            }
            return false;
        }
        private  bool up()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W))
            {
                return true;
            }
            return false;
        }

        //public void KeyBoardMove()
        //{
        //    KeyboardState state = Keyboard.GetState();

        //    if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
        //    {
        //        ApplyForce(new Vector2(-ForceX,0));
        //    }
        //    if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
        //    {
        //        ApplyForce(new Vector2(ForceX, 0));
        //    }
        //    if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
        //    {
        //        ApplyForce(new Vector2(0, -ForceY));
        //    }
        //    if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
        //    {
        //        ApplyForce(new Vector2(0,ForceY));
        //    }
        //}
      
        public void CollisionDetection()
        {
            //Side Of Screen//

            if (Position.X >= 850)
            {
                Position = (new Vector2(850, Position.Y));
            }
            if (Position.X <= 0)
            {
                Position = (new Vector2(0, Position.Y));
            }
            if (Position.Y >= 550)
            {
                Position = (new Vector2(Position.X, 550));
            }
            if (Position.Y <= 0)
            {
                Position = (new Vector2(Position.X, 0));
            }

        }

        public override void Update()
        {
            HitBox = new Rectangle((int)Position.X - 25, (int)Position.Y - 25, Texture.Width * 2, Texture.Height * 2);
            //KeyBoardMove();          
            CollisionDetection();
            UpdatePhysics();
            stateMachine.Update();

        }

        public float Radius()
        {
            float radius = Texture.Width / 2;
            return radius;
        }
    }
}
