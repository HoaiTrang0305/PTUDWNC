using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {

        public static WebApplication MapCategoryEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();


            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts",
                GetPostsByCategoriesSlug)
                .WithName("GetPostsByCategoriesSlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
               .WithName("AddNewCategory")
               .RequireAuthorization()
               .Produces(401)
               .Produces<ApiResponse<CategoryItem>>();

            /*routeGroupBuilder.MapPost("/{id:int}/avatar", SetCategoryPicture)
                .WithName("SetCategoryPicture")
                .RequireAuthorization()
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<string>>();*/

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateAnCategory")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
               .WithName("DeleteAnCategory")
               .RequireAuthorization()
               .Produces(401)
               .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories(
            [AsParameters] CategoryFilterModel model,
            ICategoryRepository categoryRepository)
        {
            var categoriesList = await categoryRepository
                .GetPagedCategoriesAsync(model, model.Name);
            //Nãy không tham chiếu cái "AuthorFilterModel : PagingModel"
            var paginationResult =
                new PaginationResult<CategoryItem>(categoriesList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetCategoryDetails(
            int id,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            var category = await categoryRepository.GetCachedCategoryByIdAsync(id);
            return category == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy loại có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }

        private static async Task<IResult> GetPostsByCategoryId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategoryId = id,
                PublishedOnly = true,
            };

            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByCategoriesSlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            //IValidator<AuthorEditModel> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            /*var validationResult=await validator.ValidateAsync(model);

            if(!validationResult.IsValid)
            {
                return Results.BadRequest(
                    validationResult.Errors.ToResponse());
            }*/
            if (await categoryRepository
                .IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            await categoryRepository.AddOrUpdateAsync(category);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }

        /*private static async Task<IResult> SetCategoryPicture(
            int id, IFormFile imageFile,
            ICategoryRepository categoryRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }
            await categoryRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }*/

        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IValidator<CategoryEditModel> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest,
                    validationResult));
            }

            if (await categoryRepository
                .IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await categoryRepository.AddOrUpdateAsync(category)
                ? Results.Ok(ApiResponse.Success("Category is updated",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }
        private static async Task<IResult> DeleteCategory(
            int id, ICategoryRepository categoryRepository)
        {
            return await categoryRepository.DeleteCategoryAsync(id)
                ? Results.Ok(ApiResponse.Success("Category is deleted",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Cound not find author"));
        }
    }
}
