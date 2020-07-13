using System;

namespace OnlineGallery.BLL.Exceptions
{
    public class UserIdMismatchException : InvalidOperationException
    {
        private const string ConstMessage =
            "You cannot perform this action due to id mismatch, check currentUserId and providedId properties";

        public UserIdMismatchException(string currentUserId, string providedId)
            : base(ConstMessage)
        {
            CurrentUserId = currentUserId;
            ProvidedId = providedId;
        }

        public string CurrentUserId { get; set; }
        public string ProvidedId { get; set; }
    }
}