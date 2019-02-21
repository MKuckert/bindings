namespace Bindings.Core.Binding.Binders
{
    public interface INamedInstanceLookup<out T>
    {
        T Find(string name);
    }
}