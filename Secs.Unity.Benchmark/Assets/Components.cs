using Scellecs.Morpeh;

namespace DefaultNamespace
{
    public struct Player : IComponent
    {
        public int age;
    }
    
    public struct Local : IComponent
    {
        public int i;
    }
    
    public struct Remote : IComponent
    {
        public int i;
    }
    
    public struct Health : IComponent
    {
        public int i;
    }
    
    public struct Position : IComponent
    {
        public float x;
        public float y;
        public float z;
    }
    
    public struct Velocity : IComponent
    {
        
    }
}