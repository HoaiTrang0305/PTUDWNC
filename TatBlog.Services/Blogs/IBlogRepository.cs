using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        //Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync
            (int year, 
            int month, 
            string slug,
            CancellationToken cancellationToken = default);

        //Tìm top N bài viết được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(int numPosts,
            CancellationToken cancellationToken = default);

        //Kiểm tra tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(int postId, string slug,
            CancellationToken cancellationToken = default);

        //Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(int postId,
            CancellationToken cancellationToken = default);

       //Lấy danh sách chuyên mục và số lượng bài viết thuộc từng chuyên mục
        Task<IList<CategoryItem>> GetCategoryAsync(bool showOnMenu = false,
            CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /*Task<Tag> GetTag(string slug,
            CancellationToken cancellationToken = default);*/


        //Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết
        //quả trả về kiểu IList<TagItem>
        Task<IList<TagItem>> GetTags(CancellationToken cancellationToken = default);


    }
}
