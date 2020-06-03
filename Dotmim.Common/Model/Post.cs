using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotmim.Common.Model
{
    public class Post
    {
        public Post()
        {
            Tags = new HashSet<Tag>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Excerpt { get; set; }

        public string BlogContent { get; set; }

        public DateTime? PubDate { get; set; }

        public DateTime? LastModified { get; set; }

        public bool IsPublished { get; set; } = true;

        public ICollection<Tag> Tags{ get; set; }

        public string GetLink()
        {
            return $"/blog/{Slug}/";
        }

        public bool AreCommentsOpen(int commentsCloseAfterDays)
        {
            if (!PubDate.HasValue)
                PubDate = DateTime.UtcNow;

            return PubDate.Value.AddDays(commentsCloseAfterDays) >= DateTime.UtcNow;
        }

        public static string CreateSlug(string title)
        {
            title = title.ToLowerInvariant().Replace(" ", "_");
            title = RemoveDiacritics(title);
            title = RemoveReservedUrlCharacters(title);

            return title.ToLowerInvariant();
        }

        static string RemoveReservedUrlCharacters(string text)
        {
            var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "'", "{", "}", "|", "~", "`", "+" };

            foreach (var chr in reservedCharacters)
                text = text.Replace(chr, "");

            return text;
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
