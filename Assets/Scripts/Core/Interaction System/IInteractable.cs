namespace FaS.DiverGame
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        string TooltipMessage { get; }

        void OnInteract();
    }
}