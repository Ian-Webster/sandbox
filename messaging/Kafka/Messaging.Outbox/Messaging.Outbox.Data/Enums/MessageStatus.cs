namespace Messaging.Outbox.Data.Enums
{
    public enum MessageStatus
    {
        NotSet,
        Saved,
        NotPersisted,
        PossiblyPersisted,
        Persisted
    }
}
