namespace TatBlog.WebApi.Models
{
    public class PostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string UrlSlug { get; set; }

        public string ImageUrl { get; set; }

        public string ViewCount { get; set;}

        public int PostedDate { get; set;}

        public DateTime? ModifiedDate { get; set;}

        public CategoryDto Category { get; set;}

        public AuthorDto Author { get; set;}

        public IList<TagDto> Tags { get; set;}
    }
}
