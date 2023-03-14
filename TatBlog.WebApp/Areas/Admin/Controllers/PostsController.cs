using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{

	public class PostsController
	{
		private readonly IBlogRepository _blogRepository;

		public PostsController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IActionResult> Index(PostFilterModel model)
		{
			var postQuery = new PostQuery()
			{
				Keyword = model.KeyWord,
				CategoryId = model.CategoryId,
				AuthorId = model.AuthorId,
				Year = model.Year,
				Month = model.Mouth
			};

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, 1, 1);
			await PopulatePosstFilterModelAsync(model);

			return View(model);
		}
		private async Task PopulatePosstFilterModelAsync(PostFilterModel model)
		{
			var authors = await _blogRepository.GetAuthorAsync();
			var categories= await _blogRepository.GetCategoryAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});
			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}
	}


}
