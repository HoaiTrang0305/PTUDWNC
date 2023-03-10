﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Blogs
{
	public class PostQuery
	{
		public int AuthorId { get; set; } = -1;
		public int CategoryId { get; set; } = -1;
		public string CategorySlug { get; set; } = "";
		public string AuthorSlug { get; set; } = "";
		public string TagSlug { get; set; } = "";
		public int Year { get; set; } = -1;
		public int Month { get; set; } = -1;
		public bool PublishedOnly { get; set; } = true;
		public string Keyword { get; set; } = "";
	}
}