public interface IInteractable
{
    void Interact(); // Triggered on interaction
    void OnPlayerApproach(); // Triggered when the player gets close
    void OnPlayerLeave(); // Triggered when the player leaves proximity
}
