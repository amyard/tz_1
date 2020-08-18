using System.Runtime.Serialization;

namespace Backend.Enums
{
    public enum TransactionStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}
