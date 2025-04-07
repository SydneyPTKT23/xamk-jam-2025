namespace slc.NIGHTSWIM
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        string TooltipMessage { get; }

        void OnInteract();
    }
}