using BlazorApp.Entities;
using System.Security.Claims;

namespace BlazorApp.Utils
{
    public static class CoursePermissions
    {
        public static bool CanEditMetadata(Course course, ClaimsPrincipal user)
        {
            int currentUserId = user.GetID() ?? 0;
            bool isAdmin = user.IsAdmin();
            bool isAuthor = course.AuthorId == currentUserId;
            bool isPending = course.ApprovedBy == null || course.ApprovedBy == 0;

            // Admin always can edit metadata, author only if approved
            return isAdmin || (isAuthor && !isPending);
        }

        public static bool CanManageSteps(Course course, ClaimsPrincipal user)
        {
            int currentUserId = user.GetID() ?? 0;
            bool isAdmin = user.IsAdmin();
            bool isTeacher = user.IsTeacher();
            bool isAuthor = course.AuthorId == currentUserId;
            bool isPending = course.ApprovedBy == null || course.ApprovedBy == 0;

            // Only approved courses can have steps managed
            // Author or admin+teacher
            return !isPending && (isAuthor || (isAdmin && isTeacher));
        }

        public static bool IsPendingDraft(Course course) => course.ApprovedBy == null || course.ApprovedBy == 0;
    }
}
