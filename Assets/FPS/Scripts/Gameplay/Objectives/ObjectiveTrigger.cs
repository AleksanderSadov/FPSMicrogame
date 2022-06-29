using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveTrigger : Objective
    {
        protected override void Start()
        {
            base.Start();

            EventManager.AddListener<CompleteTriggerEvent>(OnCompleteTrigger);
        }

        void OnCompleteTrigger(CompleteTriggerEvent evt)
        {
            if (IsCompleted)
                return;

            CompleteObjective(string.Empty,string.Empty, "Objective complete : " + Title);
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<CompleteTriggerEvent>(OnCompleteTrigger);
        }
    }
}