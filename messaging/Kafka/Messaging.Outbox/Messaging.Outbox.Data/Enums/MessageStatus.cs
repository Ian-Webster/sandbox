namespace Messaging.Outbox.Data.Enums
{
    public enum MessageStatus
    {
        NotSet = 0,
        Saved = 1,
        NotPersisted = 2,
        PossiblyPersisted = 3,
        Persisted = 4
    }
}
