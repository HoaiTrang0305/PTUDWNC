using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models
{
    public class CategoryEditModel
    {
        public int Id { get; set; }

        //Tên chuyên mục, chủ đề
        public string Name { get; set; }

        //Tên định danh dùng để tạo URl
        public string UrlSlug { get; set; }

        //Mô tả thêm về chuyên mục
        public string Description { get; set; }

        //Đánh dấu chuyên mục được hiển thị trên menu
        public bool ShowOnMenu { get; set; }

        //Danh sách các bài viết thuộc chuyên mục
        public IList<Post> Posts { get; set; }
    }
}
