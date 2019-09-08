namespace Tests.Shared
{
    public abstract class InMemoryBuilder<T> where T : class
    {
        public abstract T Instanciar();
    }
}
