namespace XamlingCore.Portable.Workflow.Flow
{
    public enum XFlowStates
    {
        Success = 0,
        Fail = 1,
        InProgress = 2, 
        WaitingForNetwork = 4,
        DisconnectedProcessing = 8,
        WaitingToStart = 16,
        WaitingForNextStage = 32,
        WaitingForRetry = 64
    }
}
