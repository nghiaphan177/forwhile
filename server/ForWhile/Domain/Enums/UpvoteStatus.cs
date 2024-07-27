using System.ComponentModel;

namespace ForWhile.Domain.Enums
{
    /// <summary>
    /// Represents the status of an upvote action.
    /// </summary>
    public enum UpvoteStatus
    {
        [Description("Indicates that the item has been downvoted.")]
        Downvoted,

        [Description("Indicates that a previous upvote or downvote has been neutralized.")]
        Neutralized,

        [Description("Indicates that the item has been upvoted.")]
        Upvoted
    }
}
