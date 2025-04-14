namespace slc.NIGHTSWIM.Core
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        string TooltipMessage { get; }

        void OnInteract();
    }
}