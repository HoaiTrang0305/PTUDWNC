using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder:IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Post.Any()) return;

            var authors = AddAuthor();
            var categories = AddCategory();
            var tags = AddTags();
            var posts=AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthor() {
            var authors = new List<Author>()
            {
                new ()
                {
                    FullName="Jason Mouth",
                    UrlSlug="jason-mouth",
                    Email="json@gmail.com",
                    JoinedDate=new DateTime(2022,10,21)
                },
                new ()
                {
                    FullName="Jessica Wonder",
                    UrlSlug="jesscia-wonder",
                    Email="jessica665@motip.com",
                    JoinedDate=new DateTime(2020,4,19)
                }
            };
            _dbContext.Author.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private IList<Category> AddCategory()
        {
            var categories = new List<Category>()
            {
                new () {Name=".NET Core", Decsription=".NET Core", UrlSlug="",}
            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }
        

        private IList<Tag> AddTags() 
        {
            var tags = new List<Tag>()
           {
               new() {Name="Google",Decsription="Google appication",UrlSlug="",Posts=new List<Post>(){ } }


           };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        
        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDesciption = "David and friends has a great repos ",
                    Decsription = "Here's a few great DON'T and DO examples ",
                    Meta = "David and friends has a great repository filled ",
                    UrlSlug = "aspnet - core - diagnostic - scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                }
            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        } 
        
    }
}
