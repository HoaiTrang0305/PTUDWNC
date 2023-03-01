﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }
       
        public async Task<Post> GetPostAsync(
            int year, 
            int month, 
            string slug,
            CancellationToken cancellationToken = default)
        {
           // throw new NotImplementedException();
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);
            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }
            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        //Tìm top n bải viết phổ được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts,
            CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        //Kiểm tra tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug,
            CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();

            return await _context.Set<Post>()
                .AnyAsync(x => x .Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        //Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(int postId,
            CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();

            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);
        }
        
                //Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục
                public async Task<IList<CategoryItem>> GetCategoryAsync(bool showOnMenu = false,
                    CancellationToken cancellationToken = default)
                {
                    IQueryable<Category> categories = _context.Set<Category>();
                    if (showOnMenu)
                    {
                        categories = categories.Where(x => x.ShowOnMenu);
                    }
                    return await categories
                        .OrderBy(x => x.Name)
                        .Select(x => new CategoryItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            UrlSlug = x.UrlSlug,
                            Description = x.Decsription,
                            ShowOnMenu = x.ShowOnMenu,
                            PostCount = x.Posts.Count(p => p.Published)
                        }).ToListAsync(cancellationToken);
                }
               //Lấy danh sách từ khóa và phân trang theo các tham số pagingParams
                public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
                    IPagingParams pagingParams,
                    CancellationToken cancellationToken = default)
                {
                    var tagQuery = _context.Set<Tag>()
                        .Select(x => new TagItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Decsription,
                            PostCount = x.Posts.Count(p => p.Published)
                        });
                    return await tagQuery
                        .ToPagedListAsync(pagingParams, cancellationToken);
                }
    }
}
